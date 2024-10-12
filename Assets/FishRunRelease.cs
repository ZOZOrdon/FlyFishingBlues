using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FishRunRelease : MonoBehaviour
{
    public Button releaseButton;                 // 触发释放的按钮
    public float delayBeforeRelease = 2f;        // 按钮按下后等待的秒数
    public Transform escapePoint;                // 撤离点
    public float moveSpeed = 5f;                 // 移动速度

    public MonoBehaviour componentToActivate;    // 需要激活的组件
    public string previousSceneName;             // 要加载的上一个场景名称
    public float arrivalThreshold = 0.5f;        // 到达撤离点的阈值

    private Rigidbody rb;
    private bool isRunning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("未在鱼的游戏对象上找到Rigidbody组件。");
            return;
        }

        if (releaseButton != null)
        {
            releaseButton.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("未设置释放按钮。");
        }

        // 确保组件在开始时被禁用
        if (componentToActivate != null)
        {
            componentToActivate.enabled = false;
        }
    }

    void OnButtonClick()
    {
        if (!isRunning)
        {
            StartCoroutine(ReleaseAfterDelay());
        }
    }

    IEnumerator ReleaseAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeRelease);

        // 激活指定组件
        if (componentToActivate != null)
        {
            componentToActivate.enabled = true;
        }

        // 设置Rigidbody为Kinematic
        rb.isKinematic = true;

        // 旋转鱼面向撤离点
        Vector3 direction = (escapePoint.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f);
        }

        isRunning = true;
    }

    void Update()
    {
        if (isRunning && escapePoint != null)
        {
            Vector3 direction = (escapePoint.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // 检查是否到达撤离点
            float distance = Vector3.Distance(transform.position, escapePoint.position);
            if (distance <= arrivalThreshold)
            {
                LoadPreviousScene();
            }
        }
    }

    void LoadPreviousScene()
    {
        if (!string.IsNullOrEmpty(previousSceneName))
        {
            SceneManager.LoadScene(previousSceneName);
        }
        else
        {
            Debug.LogError("未设置 previousSceneName。");
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // 引入 SceneManager 命名空间

public class FishLanding : MonoBehaviour
{
    public float activationDelay = 2f; // 激活脚本后的延迟时间
    public GameObject fishStaminaCanvas; // FishStaminaCanvas 组件
    public Transform escapePoint; // 撤离点的位置
    public float moveSpeed = 5f; // 鱼移动的速度

    private Rigidbody fishRigidbody;
    private FishStaminaBar staminaBar;
    private Canvas canvasComponent; // FishStaminaCanvas 上的 Canvas 组件
    private FishDragLine fishDragLine; // FishDragLine 组件
    private Animator characterAnimator; // 场景中 Character 的 Animator 组件 

    private Collider waterSurfaceTriggerCollider; // WaterSurfaceTrigger 的碰撞体
    private Collider fishLandPointCollider; // FishLandPoint 的碰撞体
    private bool isInWater = false; // 鱼是否在水中

    private void Start()
    {
        fishRigidbody = GetComponent<Rigidbody>();
        staminaBar = FishStaminaBar.instance;

        if (staminaBar == null)
        {
            Debug.LogError("FishStaminaBar instance is not found. Please ensure FishStaminaBar script is attached to an active GameObject in the scene.");
            return;
        }

        // 获取 FishDragLine 组件
        fishDragLine = GameObject.Find("FlyLine").GetComponent<FishDragLine>();
        if (fishDragLine == null)
        {
            Debug.LogError("FishDragLine component not found on 'FlyLine' GameObject.");
        }

        // 获取 WaterSurfaceTrigger 的碰撞体
        GameObject waterSurfaceTrigger = GameObject.Find("WaterSurfaceTrigger");
        if (waterSurfaceTrigger != null)
        {
            waterSurfaceTriggerCollider = waterSurfaceTrigger.GetComponent<Collider>();
            if (waterSurfaceTriggerCollider == null)
            {
                Debug.LogError("Collider component not found on 'WaterSurfaceTrigger' GameObject.");
            }
        }
        else
        {
            Debug.LogError("'WaterSurfaceTrigger' GameObject not found in the scene.");
        }

        // 获取 FishLandPoint 的碰撞体
        GameObject fishLandPoint = GameObject.Find("FishLandPoint");
        if (fishLandPoint != null)
        {
            fishLandPointCollider = fishLandPoint.GetComponent<Collider>();
            if (fishLandPointCollider == null)
            {
                Debug.LogError("Collider component not found on 'FishLandPoint' GameObject.");
            }
        }
        else
        {
            Debug.LogError("'FishLandPoint' GameObject not found in the scene.");
        }

        // 获取 FishStaminaCanvas 上的 Canvas 组件
        if (fishStaminaCanvas != null)
        {
            canvasComponent = fishStaminaCanvas.GetComponent<Canvas>();
            if (canvasComponent == null)
            {
                Debug.LogError("Canvas component not found on FishStaminaCanvas.");
            }
            else
            {
                canvasComponent.enabled = false; // 开始时禁用 Canvas 组件
            }
        }
        else
        {
            Debug.LogError("FishStaminaCanvas is not assigned in the inspector.");
        }

        // 获取 Character 的 Animator 组件
        GameObject character = GameObject.Find("Character");
        if (character != null)
        {
            characterAnimator = character.GetComponent<Animator>();
            if (characterAnimator == null)
            {
                Debug.LogError("Animator component not found on 'Character' GameObject.");
            }
        }
        else
        {
            Debug.LogError("'Character' GameObject not found in the scene.");
        }

        // 开始延迟协程
        StartCoroutine(ActivateStaminaBar());
    }

    private IEnumerator ActivateStaminaBar()
    {
        // 延迟指定的时间
        yield return new WaitForSeconds(activationDelay);

        // 启用 FishStaminaCanvas 上的 Canvas 组件
        if (canvasComponent != null)
        {
            canvasComponent.enabled = true;
        }
        else
        {
            Debug.LogError("Canvas component is null. Cannot enable.");
        }

        // 开始检测耐力值的协程
        StartCoroutine(CheckStamina());
    }

    private IEnumerator CheckStamina()
    {
        while (true)
        {
            if (staminaBar == null)
            {
                Debug.LogError("staminaBar is null.");
                yield break;
            }

            if (fishDragLine == null)
            {
                Debug.LogError("fishDragLine is null.");
                yield break;
            }

            if (staminaBar.currentStamina > 0 && isInWater)
            {
                // 耐力值不为0 且 鱼在水中
                fishRigidbody.isKinematic = true;

                // 调用 fishDragLine.StartStruggling() 伸长绳子
                fishDragLine.StartStruggling();

                // 调整方向面对撤离点
                Vector3 direction = (escapePoint.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

                // 朝撤离点移动
                transform.position += direction * moveSpeed * Time.deltaTime;

                // 设置 Character 动画参数为 true
                if (characterAnimator != null)
                {
                    characterAnimator.SetBool("IsDraging", true);
                }
            }
            else
            {
                // 耐力值为0 或 鱼不在水中
                fishRigidbody.isKinematic = false;

                // 调用 fishDragLine.StopStruggling() 停止伸长绳子
                fishDragLine.StopStruggling();

                // 设置 Character 动画参数为 false
                if (characterAnimator != null)
                {
                    characterAnimator.SetBool("IsDraging", false);
                }
            }

            yield return null;
        }
    }

    // 当鱼的碰撞体进入触发器时调用
    private void OnTriggerEnter(Collider other)
    {
        if (other == waterSurfaceTriggerCollider)
        {
            isInWater = true;
        }

        // 新增判定：检测是否与 FishLandPoint 发生碰撞
        if (other == fishLandPointCollider)
        {
            LoadNextScene();
        }
    }

    // 当鱼的碰撞体退出触发器时调用
    private void OnTriggerExit(Collider other)
    {
        if (other == waterSurfaceTriggerCollider)
        {
            isInWater = false;
        }
    }

    // 加载下一个场景的方法
    private void LoadNextScene()
    {
        // 获取当前活动场景
        Scene currentScene = SceneManager.GetActiveScene();
        // 计算下一个场景的索引
        int nextSceneIndex = currentScene.buildIndex + 1;

        // 检查下一个场景是否存在于构建设置中
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogError("No next scene found. Please ensure the next scene is added to the Build Settings.");
        }
    }
}

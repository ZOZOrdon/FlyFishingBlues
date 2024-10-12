using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraRelease : MonoBehaviour
{
    public Transform target; // 跟随的目标（鱼）
    public Vector3 offset;   // 摄像机与目标的偏移量
    public Button triggerButton; // 触发调整摄像机的按钮
    public float delayBeforeAdjustment = 2f; // 点击按钮后等待的秒数

    // 调整摄像机的目标位置和旋转
    public Vector3 targetPositionOffset;
    public Vector3 targetRotationEuler;
    public float adjustmentDuration = 1.0f; // 调整摄像机的动画持续时间

    private bool shouldAdjust = false;
    private bool isAdjusting = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float elapsedTime = 0f;

    void Start()
    {
        if (triggerButton != null)
        {
            triggerButton.onClick.AddListener(OnButtonClick);
        }
    }

    void OnButtonClick()
    {
        if (!isAdjusting)
        {
            StartCoroutine(AdjustCameraAfterDelay());
        }
    }

    IEnumerator AdjustCameraAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeAdjustment);
        StartCoroutine(AdjustCamera());
    }

    IEnumerator AdjustCamera()
    {
        isAdjusting = true;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        Vector3 finalPosition = target.position + targetPositionOffset;
        Quaternion finalRotation = Quaternion.Euler(targetRotationEuler);

        elapsedTime = 0f;

        while (elapsedTime < adjustmentDuration)
        {
            float t = elapsedTime / adjustmentDuration;
            transform.position = Vector3.Lerp(initialPosition, finalPosition, t);
            transform.rotation = Quaternion.Slerp(initialRotation, finalRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = finalPosition;
        transform.rotation = finalRotation;
        shouldAdjust = true;
        isAdjusting = false;
    }

    void LateUpdate()
    {
        if (target != null && shouldAdjust)
        {
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 5f);
            transform.LookAt(target);
        }
    }
}

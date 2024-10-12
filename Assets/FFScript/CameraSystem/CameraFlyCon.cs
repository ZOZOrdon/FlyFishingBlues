using UnityEngine;
using System.Collections;

public class CameraFlyCon : MonoBehaviour
{
    public Transform target; // 跟随的目标（鱼）
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    // Viewport Rect 动画相关参数
    [Header("Viewport Rect Animation Settings")]
    public float initialViewportX = 0.99f; // 初始X值
    public float initialViewportY = 0.99f; // 初始Y值
    public float finalViewportX = 0.6f;    // 最终X值
    public float finalViewportY = 0.6f;    // 最终Y值
    public float viewportAnimationDuration = 1.0f; // 动画持续时间（秒）

    private Camera targetCamera; // 摄像机组件
    private FishAttraction fishAttraction; // FishAttraction组件
    private bool isCameraActivated = false;

    private void Start()
    {
        // 获取摄像机组件
        targetCamera = GetComponent<Camera>();
        if (targetCamera == null)
        {
            Debug.LogError("未在该游戏对象上找到Camera组件。");
            return;
        }

        // 初始时禁用摄像机组件
        targetCamera.enabled = false;

        // 设置摄像机的初始 Viewport Rect
        targetCamera.rect = new Rect(initialViewportX, initialViewportY, targetCamera.rect.width, targetCamera.rect.height);

        // 检查目标是否已设置
        if (target != null)
        {
            // 获取FishAttraction组件
            fishAttraction = target.GetComponent<FishAttraction>();
            if (fishAttraction == null)
            {
                Debug.LogError("未在目标上找到FishAttraction组件。");
                return;
            }
        }
        else
        {
            Debug.LogError("未设置目标。");
        }
    }

    private void LateUpdate()
    {
        if (target == null || fishAttraction == null) return;

        // 当鱼被吸引时激活摄像机
        if (fishAttraction.isAttracted && !isCameraActivated)
        {
            ActivateCamera();
        }

        // 平滑跟随目标并面向目标
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }

    private void ActivateCamera()
    {
        targetCamera.enabled = true;
        isCameraActivated = true;
        Debug.Log("摄像机已激活。");

        // 启动 Viewport Rect 动画协程
        StartCoroutine(AnimateViewportRect());
    }

    private IEnumerator AnimateViewportRect()
    {
        float elapsedTime = 0f;

        // 获取当前 Viewport Rect
        Rect startRect = targetCamera.rect;

        // 定义目标 Viewport Rect
        Rect endRect = new Rect(finalViewportX, finalViewportY, startRect.width, startRect.height);

        while (elapsedTime < viewportAnimationDuration)
        {
            // 计算插值因子
            float t = elapsedTime / viewportAnimationDuration;

            // 平滑插值 Viewport Rect 的 X 和 Y
            float currentX = Mathf.Lerp(initialViewportX, finalViewportX, t);
            float currentY = Mathf.Lerp(initialViewportY, finalViewportY, t);

            // 设置新的 Viewport Rect
            targetCamera.rect = new Rect(currentX, currentY, startRect.width, startRect.height);

            // 增加已用时间
            elapsedTime += Time.deltaTime;

            // 等待下一帧
            yield return null;
        }

        // 确保最终 Viewport Rect 达到目标值
        targetCamera.rect = new Rect(finalViewportX, finalViewportY, targetCamera.rect.width, targetCamera.rect.height);
    }
}

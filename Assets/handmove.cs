using UnityEngine;

public class handmove : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float height;
    public float originalHeight = 1.5f;
    public float lowdownhand = 0.6f;
    public float distance = 0.5f; // Z轴的距离（根据相机的设置调整）
    private Animator animator;
    private bool isGrabbing = false; // 标记是否按下了左键（抓取状态）

    public bool allowHandMovementWhileGrabbing = true; // 控制抓取时是否允许手部移动

    private HandGrabber handGrabber;

    void Start()
    {
        // 获取 Animator 组件
        animator = GetComponent<Animator>();

        // 获取 HandGrabber 脚本
        handGrabber = GetComponent<HandGrabber>();
    }

    void Update()
    {
        // 检测鼠标左键按下
        if (Input.GetMouseButtonDown(0))
        {
            // 播放抓的动画
            animator.Play("GrabHold");

            // 标记为抓取状态
            isGrabbing = true;

            // 将手的Y轴减少0.6
            MoveHandDown();

            // 尝试抓取物体
            if (handGrabber != null)
            {
                handGrabber.TryGrabObject();
            }
        }

        // 检测鼠标左键松开
        if (Input.GetMouseButtonUp(0))
        {
            // 播放放手的动画
            animator.Play("GrabRelease");

            // 恢复到默认的Y轴高度
            ResetHandY();

            // 重置抓取状态
            isGrabbing = false;

            // 释放物体
            if (handGrabber != null)
            {
                handGrabber.ReleaseObject();
            }
        }

        // 让 GameObject 随鼠标移动
        MoveWithMouse();
    }

    // 让 GameObject 作为光标移动
    void MoveWithMouse()
    {
        // 如果不允许抓取时移动且正在抓取，则不更新手部位置
        if (isGrabbing && !allowHandMovementWhileGrabbing)
        {
            return;
        }

        // 获取鼠标位置并将其转换为世界坐标
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = distance; // 设置Z轴距离（根据相机的设置调整）

        // 将鼠标的屏幕坐标转换为世界坐标
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // 设置手部的高度
        if (!isGrabbing)
        {
            worldPosition.y = originalHeight;  // 在非抓取状态时，锁定Y轴为原高度
        }
        else
        {
            worldPosition.y = Mathf.Lerp(worldPosition.y, height, Time.deltaTime * moveSpeed);
        }

        // 更新手部的位置
        transform.position = worldPosition;
    }

    // 将手的Y轴减少0.6
    void MoveHandDown()
    {
        // 减少Y轴高度
        height = originalHeight - lowdownhand;
    }

    // 恢复手的默认Y轴高度
    void ResetHandY()
    {
        height = originalHeight; // 恢复到初始的Y轴高度
    }
}

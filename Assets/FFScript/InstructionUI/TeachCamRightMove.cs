using UnityEngine;

public class TeachCamRightMove : MonoBehaviour
{
    public Animator characterAnimator; // 角色的Animator
    public string animationName = "CastIntoWater"; // 需要检测的动画名称
    public float moveSpeed = 2f; // 摄像机移动的速度
    public float moveDuration = 3f; // 摄像机移动的持续时间（秒）

    public GameObject uiElement1; // 第一个要激活的UI元素
    public GameObject uiElement2; // 第二个要激活的UI元素

    private bool isMoving = false;
    private bool hasMoved = false; // 标志位，表示摄像机是否已经完成移动
    private float moveTimer = 0f;

    void Update()
    {
        // 检查角色Animator是否在播放指定的动画，并且摄像机还未移动过
        if (characterAnimator != null && characterAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName) && !hasMoved)
        {
            if (!isMoving)
            {
                // 如果动画正在播放且摄像机未移动，开始移动摄像机
                isMoving = true;
                moveTimer = moveDuration;
            }
        }

        // 如果摄像机正在移动
        if (isMoving)
        {
            // 计算摄像机的移动
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

            // 递减移动计时器
            moveTimer -= Time.deltaTime;

            // 如果移动计时器小于等于0，停止移动
            if (moveTimer <= 0)
            {
                isMoving = false;
                hasMoved = true; // 标记摄像机已经完成移动

                // 激活指定的UI元素
                if (uiElement1 != null)
                {
                    uiElement1.SetActive(true);
                }
                if (uiElement2 != null)
                {
                    uiElement2.SetActive(true);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

public class SlackLengthController : MonoBehaviour
{
    // Obi Rope 组件
    ObiRopeCursor cursor;
    ObiRope rope;

    // 控制速度和绳子的长度状态
    public float speed = 1f;
    public float smallSlack = 2f;
    public float largeSlack = 5f;
    public float maxLength = 15f; // 新增：绳子的最大长度

    // 新增：Retrieve功能相关的公共变量
    public float retrieveAmount = 1f; // 每次Retrieve动画伸长的长度
    public string retrieveAnimationStateName = "Retrieve"; // Retrieve动画状态名称

    // 引用 Character 上的 Animator 组件
    public GameObject character; // Character GameObject
    private Animator animator; // Character 上的 Animator

    // 动画状态名称
    private string swingLeftAnimationStateName = "SwingLeft";
    private string swingRightAnimationStateName = "SwingRight";

    // 内部状态
    private bool hasRetrieved = false; // 标志当前动画周期是否已经应用了Retrieve

    void Start()
    {
        cursor = GetComponentInChildren<ObiRopeCursor>();
        rope = cursor.GetComponent<ObiRope>();

        // 设置初始长度为 largeSlack
        float initialChange = largeSlack - rope.restLength;
        cursor.ChangeLength(initialChange);

        // 获取 Character GameObject 上的 Animator 组件
        if (character != null)
        {
            animator = character.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("Character GameObject is not assigned.");
        }
    }

    void Update()
    {
        // 确保 Animator 已正确获取
        if (animator == null) return;

        // 获取当前动画状态信息
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 缩短绳子到 smallSlack（播放 "SwingRight" 动画时）
        if (stateInfo.IsName(swingRightAnimationStateName))
        {
            if (rope.restLength > smallSlack)
            {
                float changeAmount = -speed * Time.deltaTime;
                // 以一定速度缩短到 smallSlack
                cursor.ChangeLength(Mathf.Max(changeAmount, smallSlack - rope.restLength));
            }
        }
        // 延长绳子到 largeSlack（播放 "SwingLeft" 动画时）
        else if (stateInfo.IsName(swingLeftAnimationStateName))
        {
            if (rope.restLength < largeSlack)
            {
                float changeAmount = speed * Time.deltaTime;
                // 以一定速度延长到 largeSlack
                cursor.ChangeLength(Mathf.Min(changeAmount, largeSlack - rope.restLength));
            }
        }
        // 处理 Retrieve 动画
        else if (stateInfo.IsName(retrieveAnimationStateName))
        {
            if (!hasRetrieved)
            {
                // 计算新的目标长度，确保不超过 maxLength
                float newLength = Mathf.Min(rope.restLength + retrieveAmount, maxLength);
                float actualChange = newLength - rope.restLength;
                if (actualChange > 0)
                {
                    cursor.ChangeLength(actualChange);
                    Debug.Log($"Rope Length after Retrieve: {rope.restLength}");
                }
                hasRetrieved = true; // 标记为已应用
            }
        }
        else
        {
            // 如果当前动画不是 Retrieve，重置标志
            if (hasRetrieved)
            {
                hasRetrieved = false;
            }
        }
    }
}

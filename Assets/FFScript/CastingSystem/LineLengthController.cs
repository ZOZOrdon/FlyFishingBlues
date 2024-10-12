using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

public class LineLengthController : MonoBehaviour
{
    // Obi Rope 组件
    private ObiRopeCursor ropeCursor;
    private ObiRope rope;

    // 控制绳子的长度和增长参数
    public float initialLength = 5f; // 游戏开始时的绳子长度

    public float maxLength = 10f; // 最大长度
    public float growthAmount = 1f; // 每次增长的长度
    public float growthSpeed = 1f; // 增长速度

    // 回收相关的公共变量
    public float RetrieveSpeed = 1f; // 回收速度
    public float RetrieveAmount = 1f; // 每次回收的长度
    public float MinLength = 2f; // 鱼线的最小长度

    // 新增拉鱼相关的公共变量
    public float landingSpeed = 2f; // 拉鱼的速度
    public float landingAmount = 2f; // 每次拉鱼回收的长度

    // 内部状态
    private bool isGrowing = false; // 当前是否正在增长
    private bool isRetrieving = false; // 当前是否正在回收
    private bool isLanding = false; // 当前是否正在拉鱼
    private float targetLength; // 目标长度

    // 动画组件
    public Animator animator; // 角色的Animator组件

    // 用于检测动画状态变化
    private bool wasLiftRodPlaying = false;

    // 引用FishStaminaBar组件
    public FishStaminaBar fishStaminaBar;

    void Start()
    {
        // 初始化绳子组件
        ropeCursor = GetComponent<ObiRopeCursor>();
        rope = GetComponent<ObiRope>();
        ropeCursor.ChangeLength(initialLength);

        // 输出当前长度
        Debug.Log($"Initial Rope Length: {rope.restLength}");

        // 如果没有在Inspector中指定Animator，尝试自动获取
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator组件未找到，请在Inspector中指定或将此脚本附加到有Animator的对象上。");
            }
        }

        // 如果没有在Inspector中指定FishStaminaBar，尝试自动获取
        if (fishStaminaBar == null)
        {
            fishStaminaBar = FindObjectOfType<FishStaminaBar>();
            if (fishStaminaBar == null)
            {
                Debug.LogError("FishStaminaBar组件未找到，请在Inspector中指定或确保场景中存在该组件。");
            }
        }
    }

    void Update()
    {
        // 监听 D 键用于增长鱼线
        if (Input.GetKeyDown(KeyCode.D) && !isGrowing && !isRetrieving && !isLanding)
        {
            // 计算新的目标长度
            targetLength = Mathf.Min(rope.restLength + growthAmount, maxLength);
            // 只有当目标长度大于当前长度时才进行增长
            if (targetLength > rope.restLength)
            {
                isGrowing = true;
            }
        }

        // 监听 S 键用于回收鱼线
        if (Input.GetKeyDown(KeyCode.S) && !isRetrieving && !isGrowing && !isLanding)
        {
            // 计算新的目标长度
            targetLength = Mathf.Max(rope.restLength - RetrieveAmount, MinLength);
            // 只有当目标长度小于当前长度时才进行回收
            if (targetLength < rope.restLength)
            {
                isRetrieving = true;
            }
        }

        // 检测 "LiftRod" 动画的播放
        if (animator != null)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            bool isLiftRodPlaying = stateInfo.IsName("LiftRod");

            // 当 "LiftRod" 动画开始播放时，并且鱼的耐力值小于等于0，触发拉鱼动作
            if (isLiftRodPlaying && !wasLiftRodPlaying && !isGrowing && !isRetrieving && !isLanding)
            {
                // 检查FishStaminaBar的currentStamina是否小于等于0
                if (fishStaminaBar != null && fishStaminaBar.currentStamina <= 0)
                {
                    // 计算新的目标长度
                    targetLength = Mathf.Max(rope.restLength - landingAmount, MinLength);
                    // 只有当目标长度小于当前长度时才进行拉鱼
                    if (targetLength < rope.restLength)
                    {
                        isLanding = true;
                    }
                }
            }

            wasLiftRodPlaying = isLiftRodPlaying;
        }

        // 控制绳子的增长
        if (isGrowing)
        {
            if (rope.restLength < targetLength)
            {
                float changeAmount = growthSpeed * Time.deltaTime;
                ropeCursor.ChangeLength(Mathf.Min(changeAmount, targetLength - rope.restLength));
            }
            else
            {
                // 到达目标长度后停止增长
                isGrowing = false;
                // 输出当前长度
                Debug.Log($"Rope Length after growth: {rope.restLength}");
            }
        }

        // 控制绳子的回收
        if (isRetrieving)
        {
            if (rope.restLength > targetLength)
            {
                float changeAmount = RetrieveSpeed * Time.deltaTime;
                // 负值表示减少长度
                ropeCursor.ChangeLength(-Mathf.Min(changeAmount, rope.restLength - targetLength));
            }
            else
            {
                // 到达目标长度后停止回收
                isRetrieving = false;
                // 输出当前长度
                Debug.Log($"Rope Length after retrieval: {rope.restLength}");
            }
        }

        // 控制绳子的拉鱼动作
        if (isLanding)
        {
            if (rope.restLength > targetLength)
            {
                float changeAmount = landingSpeed * Time.deltaTime;
                // 负值表示减少长度
                ropeCursor.ChangeLength(-Mathf.Min(changeAmount, rope.restLength - targetLength));
            }
            else
            {
                // 到达目标长度后停止拉鱼
                isLanding = false;
                // 输出当前长度
                Debug.Log($"Rope Length after landing: {rope.restLength}");
            }
        }
    }
}

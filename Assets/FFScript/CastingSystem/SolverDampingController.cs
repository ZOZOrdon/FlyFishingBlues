using UnityEngine;
using Obi;

public class SolverDampingController : MonoBehaviour
{
    // ObiSolver 组件
    private ObiSolver solver;

    // Damping 设置结构
    [System.Serializable]
    public class DampingSetting
    {
        [Tooltip("绳子的长度（仅在空格键按下时有效）")]
        public float length; // 绳子长度

        [Tooltip("对应的 damping 值")]
        public float damping; // 对应的 damping 值

        [Tooltip("对应的 Gravity Y 轴值")]
        public float gravityY; // 对应的 Gravity Y 轴值
    }

    // 五个 damping 设置（基于绳子长度）
    [Header("Length-Based Damping Settings")]
    public DampingSetting firstDamping;
    public DampingSetting secondDamping;
    public DampingSetting thirdDamping;
    public DampingSetting fourthDamping;
    public DampingSetting fifthDamping;

    // 新增：优先级最高的 IfCastingDampingGravity 设置（不依赖绳子长度）
    [Header("Highest Priority Setting (IfCastingDamping&Gravity)")]
    [Tooltip("当空格键未被按下时应用的 damping 值")]
    public float castingDamping; // 仅 damping 值

    [Tooltip("当空格键未被按下时应用的 Gravity Y 值")]
    public float castingGravityY; // 仅 Gravity Y 值

    void Start()
    {
        // 获取 ObiSolver 组件
        solver = GetComponent<ObiSolver>();

        if (solver == null)
        {
            Debug.LogError("ObiSolver component not found on this GameObject.");
        }
    }

    void Update()
    {
        // 如果没有找到 ObiSolver，则不继续执行
        if (solver == null) return;

        // 检查空格键是否被按下
        bool isSpacePressed = Input.GetKey(KeyCode.Space);

        if (!isSpacePressed)
        {
            // 空格键未被按下，应用最高优先级的 damping 和 gravity 设置
            ApplyCastingDampingAndGravity();
        }
        else
        {
            // 空格键被按下，继续原有的根据绳子长度控制 damping 和 gravity 的逻辑
            // 获取关联的第一个 ObiRope 的长度
            ObiRope rope = GetFirstRope();
            if (rope == null) return;

            float currentLength = rope.restLength;

            // 调试信息：输出当前绳子的长度
            Debug.Log($"Current Rope Length: {currentLength}");

            // 根据绳子长度设置 damping 和 gravity
            UpdateDampingAndGravityBasedOnLength(currentLength);
        }
    }

    // 获取第一个 ObiRope 实例
    private ObiRope GetFirstRope()
    {
        foreach (var actor in solver.actors)
        {
            if (actor is ObiRope rope)
            {
                return rope;
            }
        }
        return null;
    }

    // 根据绳子长度更新 damping 和 gravity
    void UpdateDampingAndGravityBasedOnLength(float currentLength)
    {
        // 匹配当前绳子的长度并设置 damping 和 gravity
        if (currentLength >= firstDamping.length && currentLength < secondDamping.length)
        {
            ApplyDampingAndGravity(firstDamping, "firstDamping");
        }
        else if (currentLength >= secondDamping.length && currentLength < thirdDamping.length)
        {
            ApplyDampingAndGravity(secondDamping, "secondDamping");
        }
        else if (currentLength >= thirdDamping.length && currentLength < fourthDamping.length)
        {
            ApplyDampingAndGravity(thirdDamping, "thirdDamping");
        }
        else if (currentLength >= fourthDamping.length && currentLength < fifthDamping.length)
        {
            ApplyDampingAndGravity(fourthDamping, "fourthDamping");
        }
        else if (currentLength >= fifthDamping.length)
        {
            ApplyDampingAndGravity(fifthDamping, "fifthDamping");
        }
    }

    // 应用最高优先级的 damping 和 gravity 设置（不依赖绳子长度）
    void ApplyCastingDampingAndGravity()
    {
        solver.parameters.damping = castingDamping;
        solver.gravity = new Vector3(solver.gravity.x, castingGravityY, solver.gravity.z);
        solver.PushSolverParameters(); // 强制更新参数
        Debug.Log($"[IfCastingDamping&Gravity] Damping updated to {castingDamping}, Gravity Y updated to {castingGravityY}");
    }

    // 通用的方法来应用 damping 和 gravity，并输出调试信息
    void ApplyDampingAndGravity(DampingSetting setting, string settingName)
    {
        solver.parameters.damping = setting.damping;
        solver.gravity = new Vector3(solver.gravity.x, setting.gravityY, solver.gravity.z);
        solver.PushSolverParameters(); // 强制更新参数
        Debug.Log($"[{settingName}] Damping updated to {setting.damping}, Gravity Y updated to {setting.gravityY} for rope length {setting.length}");
    }
}

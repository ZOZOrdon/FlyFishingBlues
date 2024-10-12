using UnityEngine;
using Obi;

public class FishBiteHook : MonoBehaviour
{
    public Transform flyhook; // 飞钩的 Transform 引用
    public Transform exit1; // Exit1 的 Transform 引用
    public float moveSpeed = 3f; // 鱼向飞钩移动的速度
    public float stopDistance = 0.5f; // 鱼停止移动的最小距离
    public float waitTime = 1f; // 等待时间
    public float escapeSpeed = 5f; // 鱼逃跑的速度
    public bool isFishBite = false; // 鱼是否咬钩

    private Animator fishAnimator; // 鱼的 Animator 组件
    private Animator characterAnimator; // 场景中 Character 的 Animator 组件 
    private bool isMovingToHook = true; // 标记是否在向飞钩移动
    private float waitTimer = 0f; // 等待计时器
    private FishDragLine fishDragLine; // 引用 FishDragLine 脚本

    void Start()
    {
        GameObject flyhookObject = GameObject.Find("flyhook");
        if (flyhookObject != null)
        {
            flyhook = flyhookObject.transform;
        }
        else
        {
            Debug.LogError("未找到名为 'flyhook' 的 GameObject！");
        }

        fishAnimator = GetComponent<Animator>(); // 自动查找鱼的 Animator 组件
        if (fishAnimator == null)
        {
            Debug.LogError("未找到鱼的 Animator 组件！");
        }

        GameObject characterObject = GameObject.Find("Character"); // 自动查找场景中的 Character
        if (characterObject != null)
        {
            characterAnimator = characterObject.GetComponent<Animator>();
            if (characterAnimator == null)
            {
                Debug.LogError("未找到 Character 的 Animator 组件！");
            }
        }
        else
        {
            Debug.LogError("未找到名为 'Character' 的 GameObject！");
        }

        fishDragLine = GameObject.Find("FlyLine").GetComponent<FishDragLine>(); // 获取 FishDragLine 组件
        if (fishDragLine == null)
        {
            Debug.LogError("未找到 FishDragLine 组件，请确认该脚本附加在 FlyLine 上！");
        }
    }

    void Update()
    {
        if (isMovingToHook)
        {
            MoveToHook();
        }
        else
        {
            EscapeToExit();
        }
    }

    private void MoveToHook()
    {
        float distanceToHook = Vector3.Distance(transform.position, flyhook.position);
        if (distanceToHook > stopDistance)
        {
            Vector3 direction = (flyhook.position - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, flyhook.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            AttachFishToFlyline(); // 绑定鱼到 FlyLine
            isMovingToHook = false; // 到达最小距离，开始等待
            waitTimer = waitTime; // 重置等待计时器

            // 触发“TroutBite”动画
            if (fishAnimator != null)
            {
                fishAnimator.SetTrigger("TroutBite");
            }
        }
    }


    private void EscapeToExit()
    {
        isFishBite = true;
        if (waitTimer > 0)
        {
            waitTimer -= Time.deltaTime; // 减少等待时间
        }
        else
        {
            // 检查 Character 是否播放了 "SetTheHook" 动画
            if (characterAnimator.GetCurrentAnimatorStateInfo(0).IsName("SetTheHook"))
            {
                if (fishDragLine != null)
                {
                    fishDragLine.StopDragging();
                }

                // 设置鱼的 Rigidbody 的 IsKinematic 为 false
                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false; // 解除 Kinematic 状态
                }

                this.enabled = false; // 禁用当前脚本
                GetComponent<FishLanding>().enabled = true; // 激活 FishLanding 脚本
                return; // 结束当前方法
            }

            // 逃跑开始时切换动画
            
            characterAnimator.SetBool("FishOn", true); // 设置 Character 动画的 FishOn 为 true
  

            // 开始拉出鱼线
            if (fishDragLine != null)
            {
                fishDragLine.StartDragging();
            }

            Vector3 direction = (exit1.position - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, exit1.position, escapeSpeed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(direction); // 朝向 Exit1

            // 检查是否到达 Exit1
            if (Vector3.Distance(transform.position, exit1.position) < stopDistance)
            {
                Destroy(transform.parent.gameObject); // 销毁鱼的父物体
                Debug.Log("鱼的父物体 'Trout1SpawnPrefab2' 已被销毁！");
                // 解除鱼线的延长
                if (fishDragLine != null)
                {
                    fishDragLine.StopDragging();
                }
            }
        }
    }

    private void AttachFishToFlyline()
    {
        // 自动查找名为 "FlyLine" 的对象
        GameObject flyLine = GameObject.Find("FlyLine");

        if (flyLine != null)
        {
            // 获取 FlyLine 对象上的所有 ObiParticleAttachment 组件
            ObiParticleAttachment[] attachments = flyLine.GetComponents<ObiParticleAttachment>();

            if (attachments.Length >= 3) // 确保有至少三个 Obi Particle Attachment
            {
                // 给第三个 Obi Particle Attachment 设置目标为鱼自身
                attachments[2].target = this.transform; // 将鱼的 Transform 作为目标
                Debug.Log("鱼已经被绑定到 FlyLine 的第三个 Obi Particle Attachment！");
            }
            else
            {
                Debug.LogError("FlyLine 上没有足够的 Obi Particle Attachment 组件！");
            }
        }
        else
        {
            Debug.LogError("未找到名为 'FlyLine' 的对象！");
        }
    }
}

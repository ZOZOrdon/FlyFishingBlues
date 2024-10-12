using UnityEngine;

public class AnglerAnimationController : MonoBehaviour
{
    // Animator组件
    private Animator animator;

    // 动画参数名称
    private readonly string PRESSING_SPACE = "PressingSpace";
    private readonly string SWING_LEFT = "SwingLeft";
    private readonly string SWING_RIGHT = "SwingRight";
    private readonly string RETRIEVE = "Retrieve";
    private readonly string LIFT_ROD = "LiftRod";
    private readonly string SETTHEHOOK = "SetTheHook";
    void Start()
    {
        // 获取Animator组件
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator组件未找到！");
        }
    }

    void Update()
    {
        HandleInput();
    }

    /// <summary>
    /// 处理用户输入并设置Animator参数
    /// </summary>
    private void HandleInput()
    {
        if (animator == null)
            return;

        // 处理空格键按下和释放
        bool isPressingSpace = Input.GetKey(KeyCode.Space);
        animator.SetBool(PRESSING_SPACE, isPressingSpace);

        // 处理A键按下
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("A键被按下，触发 SwingLeft 动画");
            animator.SetBool(SWING_LEFT, true);
            // 启动协程以在动画后重置参数
            StartCoroutine(ResetSwingParameter(SWING_LEFT));
        }

        // 处理D键按下
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("D键被按下，触发 SwingRight 动画");
            animator.SetBool(SWING_RIGHT, true);
            // 启动协程以在动画后重置参数
            StartCoroutine(ResetSwingParameter(SWING_RIGHT));
        }

        // 处理S键按下
        if (Input.GetKeyDown(KeyCode.S))
        {
            // 仅在PressingSpace为false时允许触发Retrieve
            if (!isPressingSpace)
            {
                Debug.Log("S键被按下，触发 Retrieve 动画");
                animator.SetTrigger(RETRIEVE);
                // 不需要启动协程重置Trigger，因为Trigger会自动重置
            }
            else
            {
                Debug.Log("S键被按下，但 PressingSpace 为 true，无法触发 Retrieve 动画");
            }
        }


        if (Input.GetKeyDown(KeyCode.W) && animator.GetBool("FishOn"))
        {
            
            // animator.SetBool(PRESSING_W, true);
            animator.SetTrigger(SETTHEHOOK);


        }

        if (Input.GetKey(KeyCode.A))
        {

            animator.SetBool(LIFT_ROD, true);
            // 不需要启动协程重置Trigger，因为Trigger会自动重置

        }


        if (Input.GetKeyUp(KeyCode.A))
        {

            animator.SetBool(LIFT_ROD, false);
            // 不需要启动协程重置Trigger，因为Trigger会自动重置

        }


    }

    /// <summary>
    /// 协程：在指定时间后重置Swing参数
    /// </summary>
    /// <param name="parameter">要重置的参数名称</param>
    /// <returns></returns>
    private System.Collections.IEnumerator ResetSwingParameter(string parameter)
    {
        // 等待动画过渡完成（根据动画长度调整等待时间）
        // 这里假设每个Swing动画持续0.5秒
        yield return new WaitForSeconds(0.5f);
        animator.SetBool(parameter, false);
    }

    /// <summary>
    /// 动画事件调用的方法，用于重置Swing参数
    /// 如果使用动画事件，可以通过调用此方法来重置参数，而不需要协程
    /// </summary>
    /// <param name="parameter">要重置的参数名称</param>
    public void OnAnimationEnd(string parameter)
    {
        if (animator != null)
        {
            animator.SetBool(parameter, false);
            Debug.Log($"{parameter} 参数已重置");
        }
    }
}

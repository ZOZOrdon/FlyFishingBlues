using UnityEngine;
using Obi;

public class FishDragLine : MonoBehaviour
{
    public float dragSpeed = 1f; // 拖动时绳子增长速度
    public float retrieveSpeed = 1f; // 收回时绳子缩短速度
    public float struggleSpeed = 1f; // 挣扎时绳子增长速度
    public float pullSpeed = 1f; // 拉动时绳子缩短速度

    private ObiRope rope; // 引用 ObiRope 组件
    private ObiRopeCursor ropeCursor; // 引用 ObiRopeCursor 组件
    public bool isDragging = false; // 标记拖动状态
    public bool isRetrieving = false; // 标记收回状态
    public bool isStruggling = false; // 标记挣扎状态
    public bool isPulling = false; // 标记拉动状态
    private Animator characterAnimator; // 引用角色动画

    void Start()
    {
        // 获取 ObiRope 组件
        rope = GetComponent<ObiRope>();
        // 获取 ObiRopeCursor 组件
        ropeCursor = GetComponent<ObiRopeCursor>();

        if (rope == null || ropeCursor == null)
        {
            Debug.LogError("未找到 ObiRope 或 ObiRopeCursor 组件，请确认该脚本附加在 FlyLine 上！");
        }
    }

    void Update()
    {
        // 根据不同状态来延长或缩短绳子
        if (isDragging && rope != null && ropeCursor != null)
        {
            ExtendRope(dragSpeed); // 拖动绳子
        }
        if (isRetrieving && rope != null && ropeCursor != null)
        {
            ExtendRope(-retrieveSpeed); // 收回绳子
        }
        if (isStruggling && rope != null && ropeCursor != null)
        {
            ExtendRope(struggleSpeed); // 挣扎时绳子增长
        }
        if (isPulling && rope != null && ropeCursor != null)
        {
            ExtendRope(-pullSpeed); // 拉动绳子
        }

        // 检查“SetTheHook”动画是否正在播放，如果正在播放则停止所有操作
        if (characterAnimator != null && characterAnimator.GetCurrentAnimatorStateInfo(0).IsName("SetTheHook"))
        {
            StopAllActions(); // 停止所有动作
        }
    }

    // 拖动绳子
    public void StartDragging()
    {
        isDragging = true;
    }

    public void StopDragging()
    {
        isDragging = false;
    }

    // 收回绳子
    public void StartRetrieving()
    {
        isRetrieving = true;
    }

    public void StopRetrieving()
    {
        isRetrieving = false;
    }

    // 挣扎
    public void StartStruggling()
    {
        isStruggling = true;
    }

    public void StopStruggling()
    {
        isStruggling = false;
    }

    // 拉动绳子
    public void StartPulling()
    {
        isPulling = true;
    }

    public void StopPulling()
    {
        isPulling = false;
    }

    // 停止所有绳子的操作
    public void StopAllActions()
    {
        isDragging = false;
        isRetrieving = false;
        isStruggling = false;
        isPulling = false;
    }

    // 绳子延长或缩短的逻辑
    private void ExtendRope(float speed)
    {
        ropeCursor.ChangeLength(speed * Time.deltaTime); // 使用 ObiRopeCursor 来改变绳子的长度
        Debug.Log("绳子状态变化，当前长度变化: " + speed * Time.deltaTime);
    }
}

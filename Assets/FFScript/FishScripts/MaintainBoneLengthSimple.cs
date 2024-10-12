using UnityEngine;

public class MaintainBoneLengthSimple : MonoBehaviour
{
    private Transform parentBone;     // 父骨骼
    private float initialLength;      // 初始骨骼长度

    void Start()
    {
        // 获取父骨骼
        parentBone = transform.parent;

        if (parentBone != null)
        {
            // 计算并存储初始长度
            initialLength = Vector3.Distance(parentBone.position, transform.position);
        }
        else
        {
            Debug.LogWarning("MaintainBoneLengthSimple: 此骨骼没有父骨骼。");
        }
    }

    void LateUpdate()
    {
        if (parentBone != null)
        {
            // 计算父骨骼到当前骨骼的方向
            Vector3 direction = (transform.position - parentBone.position).normalized;

            // 更新当前骨骼的位置，保持与父骨骼的距离恒定
            transform.position = parentBone.position + direction * initialLength;
        }
    }
}

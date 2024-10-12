using UnityEngine;

public class CamFollowFly : MonoBehaviour
{
    public Transform flyHook;               // FlyHook的Transform
    private Vector3 initialLocalOffset;     // 摄像机和FlyHook之间的初始本地偏移量
    private Quaternion initialRotation;     // 摄像机的初始旋转

    void Start()
    {
        if (flyHook != null)
        {
            // 计算摄像机在FlyHook本地坐标系中的初始偏移量
            initialLocalOffset = flyHook.InverseTransformPoint(transform.position);

            // 记录摄像机的初始旋转，以保持摄像机不旋转
            initialRotation = transform.rotation;
        }
        else
        {
            Debug.LogError("请在Inspector中指定FlyHook对象的Transform。");
        }
    }

    void LateUpdate()
    {
        if (flyHook != null)
        {
            // 计算摄像机在FlyHook本地坐标系中的新位置
            Vector3 newLocalPosition = initialLocalOffset;

            // 如果你只想在X和Y轴上跟随，可以确保Z轴偏移保持不变
            // newLocalPosition.z = initialLocalOffset.z;

            // 将本地位置转换回世界坐标系
            Vector3 newWorldPosition = flyHook.TransformPoint(newLocalPosition);

            // 更新摄像机的位置
            transform.position = newWorldPosition;

            // 保持摄像机的初始旋转，防止摄像机跟随FlyHook旋转
            transform.rotation = initialRotation;
        }
    }
}

// HandGrabber.cs
using UnityEngine;

public class HandGrabber : MonoBehaviour
{
    public string grabButton = "Fire1"; // 抓取按键（默认是鼠标左键）
    public Transform holdPoint;         // 抓取物体后放置的位置（手部的某个子对象）
    public float grabRange = 2f;        // 抓取范围
    public LayerMask grabbableLayer;    // 可抓取物体的层

    private Grabbable grabbedObject = null; // 当前抓取的物体

    void Update()
    {
        if (Input.GetButtonDown(grabButton))
        {
            if (grabbedObject == null)
            {
                TryGrabObject();
            }
            else
            {
                ReleaseObject();
            }
        }
    }

    public void TryGrabObject()
    {
        // 在抓取范围内进行球形检测，寻找可抓取的物体
        Collider[] hits = Physics.OverlapSphere(transform.position, grabRange, grabbableLayer);
        foreach (var hit in hits)
        {
            Grabbable grabbable = hit.GetComponent<Grabbable>();
            if (grabbable != null)
            {
                GrabObject(grabbable);
                break;
            }
        }
    }

    void GrabObject(Grabbable grabbable)
    {
        grabbedObject = grabbable;

        // 禁用物体的物理效果
        Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // 将物体设置为手部的子对象，放置在 holdPoint 位置
        grabbedObject.transform.SetParent(transform); // 将物体设置为手部的子对象
        grabbedObject.transform.localPosition = Vector3.zero;
        grabbedObject.transform.localRotation = Quaternion.identity;

    }

    public void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            // 解除父子关系
            grabbedObject.transform.SetParent(null);

            // 启用物体的物理效果
            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            grabbedObject = null;
        }
    }

    // 可选：可视化抓取范围
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, grabRange);
    }
}

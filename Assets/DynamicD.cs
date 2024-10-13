using UnityEngine;

public class DynamicD : MonoBehaviour
{
    // 定义需要激活的UI对象
    public GameObject uiText;
    // 需要检测的物体A
    public GameObject objectA;  // 带有Trigger的物体A

    private Collider objectACollider;

    private void Start()
    {
        // 确保UI文本默认处于不激活状态
        if (uiText != null)
        {
            uiText.SetActive(false);
        }
        else
        {
            Debug.LogWarning("UI Text object is not assigned!");
        }

        // 获取物体A的Collider组件
        if (objectA != null)
        {
            objectACollider = objectA.GetComponent<Collider>();

            if (objectACollider == null || !objectACollider.isTrigger)
            {
                Debug.LogError("Object A does not have a Trigger Collider!");
            }
        }
        else
        {
            Debug.LogError("Object A is not assigned!");
        }
    }

    // 当物体A的触发器被触发时调用
    private void OnTriggerEnter(Collider other)
    {
        // 检测是否为其他物体触发
        if (other != objectACollider)
        {
            // 激活UI
            if (uiText != null)
            {
                uiText.SetActive(true);
                Debug.Log("Object A triggered by another object. UI Activated.");
            }
        }
    }
}

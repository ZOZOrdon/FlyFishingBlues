using DynamicMeshCutter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerForEscape : MonoBehaviour
{
    public fishstruggling fishstruggling;
    public float escapeInterval = 2f; // 每隔2秒调用一次EscapeJump
    private float timer = 0f;

 
    private void OnTriggerEnter(Collider other)
    {

        // 检查是否是带有 "fish" 标签的物体
        if (other.gameObject.tag == "fish")
        {
            // 获取 fishstruggling 组件
            if (fishstruggling != null)
            {
                Debug.Log("there you are");
                fishstruggling.EscapeJump();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // 检查是否是带有 "fish" 标签的物体
        if (other.gameObject.tag == "fish" )
        {
            // 更新计时器
            timer += Time.deltaTime;

            // 当计时器达到逃脱间隔时，调用 EscapeJump 方法
            if (timer >= escapeInterval)
            {
                Debug.Log("runrunforyourlife");
                fishstruggling.EscapeJump();
                timer = 0f; // 重置计时器
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 当鱼离开触发区域时，重置 fishstruggling 和计时器
        if (other.gameObject.tag == "fish" )
        {
            fishstruggling = null;
            timer = 0f;
            Debug.Log("Fish left the area");
        }
    }
}

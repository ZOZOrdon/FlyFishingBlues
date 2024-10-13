using UnityEngine;

public class BlinksUIOnPause : MonoBehaviour
{
    public float blinkDuration = 1f;  // 闪烁的周期时长
    public float minAlpha = 0f;  // 最小透明度
    public float maxAlpha = 1f;  // 最大透明度

    private CanvasGroup canvasGroup;  // CanvasGroup组件
    private float timer = 0f;

    void Start()
    {
        // 获取CanvasGroup组件
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("未能找到CanvasGroup组件，请将此脚本附加到一个带有CanvasGroup的UI对象上。");
        }
    }

    void Update()
    {
        if (canvasGroup != null)
        {
            // 使用UnscaledDeltaTime以不受Time.timeScale的影响
            timer += Time.unscaledDeltaTime;

            // 计算透明度的变化 (0-1的渐变循环)
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(timer / blinkDuration, 1f));

            // 设置CanvasGroup的Alpha值
            canvasGroup.alpha = alpha;
        }
    }
}

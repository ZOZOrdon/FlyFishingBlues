using UnityEngine;

public class UIFadeIn : MonoBehaviour
{
    // 淡入持续时间（秒）
    public float duration = 2f;

    // 内部计时器
    private float timer = 0f;

    // CanvasGroup组件，用于控制UI透明度
    private CanvasGroup canvasGroup;

    void Start()
    {
        // 获取或添加CanvasGroup组件
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // 初始化透明度为0（完全透明）
        canvasGroup.alpha = 0f;
    }

    void Update()
    {
        if (timer < duration)
        {
            // 增加计时器
            timer += Time.deltaTime;

            // 计算当前透明度
            float alpha = Mathf.Clamp01(timer / duration);

            // 设置CanvasGroup的透明度
            canvasGroup.alpha = alpha;
        }
        else
        {
            // 确保透明度为1（完全不透明）
            canvasGroup.alpha = 1f;

            // 淡入完成后，可以禁用该脚本（可选）
            // enabled = false;
        }
    }
}

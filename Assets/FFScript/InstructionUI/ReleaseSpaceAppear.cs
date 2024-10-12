using UnityEngine;
using UnityEngine.UI;

public class ReleaseSpaceAppear : MonoBehaviour
{
    public Slider ropeProgressBar; // 引用RopeProgressBar Slider组件
    public float fadeDuration = 1.0f; // 平滑显现的持续时间，单位为秒

    private CanvasGroup canvasGroup; // CanvasGroup用于控制UI透明度
    private bool isFadingIn = false; // 是否正在平滑显现

    void Start()
    {
        // 获取CanvasGroup组件，如果没有则自动添加
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // 初始设置透明度为0（完全透明）
        canvasGroup.alpha = 0f;
    }

    void Update()
    {
        // 检查RopeProgressBar的值是否大于0.8
        if (ropeProgressBar != null && ropeProgressBar.value > 0.8f && !isFadingIn)
        {
            // 开始平滑显现
            StartCoroutine(FadeIn());
        }
    }

    private System.Collections.IEnumerator FadeIn()
    {
        isFadingIn = true;
        float elapsedTime = 0f;

        // 平滑过渡，持续fadeDuration时间
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终透明度为1
        canvasGroup.alpha = 1f;
        isFadingIn = false;
    }
}

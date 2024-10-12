using System.Collections;
using UnityEngine;

public class DynamicInputSpace : MonoBehaviour
{
    private CanvasGroup canvasGroup;  // 自动获取CanvasGroup组件
    public float fadeInDuration = 2f;  // UI逐渐显现的时间
    public float blinkDuration = 1f;   // UI闪烁的时间（一次透明度变化的时间）
    public float maxAlpha = 0.7f;      // 闪烁时的最大透明度
    public float fadeOutDuration = 1.5f; // 玩家按住空格时UI淡出的时间

    private bool spaceHeld = false;    // 空格键是否被按住
    private Coroutine blinkCoroutine;  // 用于控制闪烁协程

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();  // 自动获取CanvasGroup组件
        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup组件未找到，请确保该UI对象上存在CanvasGroup组件。");
            return;
        }

        canvasGroup.alpha = 0f;  // 初始透明度设为0
        StartCoroutine(FadeIn());  // 开始UI的渐显
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spaceHeld = true;  // 记录空格键被按下
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);  // 停止闪烁
            }
            StartCoroutine(FadeOut());  // 开始淡出
        }
    }

    // 渐显效果
    IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer < fadeInDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeInDuration);  // 从0%到100%透明度
            timer += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;  // 确保透明度为100%
        StartCoroutine(Blink());  // 开始闪烁
    }

    // 闪烁效果
    IEnumerator Blink()
    {
        while (!spaceHeld)  // 只要空格没按住就一直闪烁
        {
            // 渐显到最大透明度
            float timer = 0f;
            while (timer < blinkDuration / 2f)
            {
                canvasGroup.alpha = Mathf.Lerp(0f, maxAlpha, timer / (blinkDuration / 2f));
                timer += Time.deltaTime;
                yield return null;
            }

            // 渐隐到0透明度
            timer = 0f;
            while (timer < blinkDuration / 2f)
            {
                canvasGroup.alpha = Mathf.Lerp(maxAlpha, 0f, timer / (blinkDuration / 2f));
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }

    // 淡出效果
    IEnumerator FadeOut()
    {
        float timer = 0f;
        while (timer < fadeOutDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeOutDuration);  // 从100%到0%透明度
            timer += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;  // 确保透明度为0
    }
}

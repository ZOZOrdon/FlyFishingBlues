using UnityEngine;
using UnityEngine.UI;
using Obi;

public class RopeProgressBar : MonoBehaviour
{
    // 引用 ObiRope 组件
    public ObiRope rope;
    // 引用进度条组件（Slider）
    public Slider progressBar;

    // 定义绳子长度的最小值和最大值
    public float minRopeLength = 5f;
    public float maxRopeLength = 16f;

    void Update()
    {
        // 获取当前绳子长度
        float currentRopeLength = rope.restLength;

        // 将绳子长度映射到 0 到 1 的范围
        float progress = (currentRopeLength - minRopeLength) / (maxRopeLength - minRopeLength);

        // 确保进度值在 0 到 1 之间
        progress = Mathf.Clamp01(progress);

        // 更新进度条的值
        progressBar.value = progress;
    }
}

using UnityEngine;

public class FishSway : MonoBehaviour
{
    // 速度阈值
    [Header("速度阈值设置")]
    public float slowSpeedThreshold = 0.5f;
    public float midSpeedThreshold = 1.5f;

    // 慢速摆动参数
    [Header("慢速摆动参数")]
    public float slowSwayAmplitude = 5f;
    public float slowSwayFrequency = 1f;

    // 中速摆动参数
    [Header("中速摆动参数")]
    public float midSwayAmplitude = 10f;
    public float midSwayFrequency = 2f;

    // 快速摆动参数
    [Header("快速摆动参数")]
    public float fastSwayAmplitude = 15f;
    public float fastSwayFrequency = 3f;

    private float swayAmplitude;
    private float swayFrequency;

    private Quaternion initialLocalRotation;
    private Vector3 lastPosition;
    private float speed;

    void Start()
    {
        // 记录初始局部旋转
        initialLocalRotation = transform.localRotation;

        // 初始化上一次位置
        lastPosition = transform.position;

        // 设置初始摆动参数为慢速
        swayAmplitude = slowSwayAmplitude;
        swayFrequency = slowSwayFrequency;
    }

    void Update()
    {
        // 计算当前帧的速度（单位：单位/秒）
        speed = Vector3.Distance(transform.position, lastPosition) / Time.deltaTime;

        // 根据速度调整摆动参数
        if (speed < slowSpeedThreshold)
        {
            swayAmplitude = slowSwayAmplitude;
            swayFrequency = slowSwayFrequency;
        }
        else if (speed < midSpeedThreshold)
        {
            swayAmplitude = midSwayAmplitude;
            swayFrequency = midSwayFrequency;
        }
        else
        {
            swayAmplitude = fastSwayAmplitude;
            swayFrequency = fastSwayFrequency;
        }

        // 计算当前时间的角度
        float angle = Mathf.Sin(Time.time * swayFrequency) * swayAmplitude;

        // 创建一个绕 Z 轴的旋转（保持绕 Z 轴旋转不变）
        Quaternion swayRotation = Quaternion.Euler(0f, 0f, angle);

        // 将摆动旋转应用到初始旋转上
        transform.localRotation = initialLocalRotation * swayRotation;

        // 更新上一次位置
        lastPosition = transform.position;
    }
}
 

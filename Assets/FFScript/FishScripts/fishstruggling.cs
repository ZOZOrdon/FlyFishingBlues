using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class fishstruggling : MonoBehaviour
{
    [Header("挣扎参数")]
    public float struggleForce = 5f; // 垂直跳跃力度
    public float horizontalMoveForce = 2f; // 水平移动力度
    public float struggleFrequency = 1f; // 挣扎频率
    public float groundLevel = 0f; // 地面高度
    public float maxHorizontalMovement = 5f; // 水平移动范围
    public float maxRotationZ = 80f; // Z 轴旋转角度限制
    public float maxJumpHeight = 2f; // 最大跳跃高度
    public float escapeJumpForce = 10f; // 跳入水中的向上力度
    public float zMoveForce = 1f; // Z 轴前后移动的力度

    private Vector3 initialPosition;
    private Rigidbody ParentRb;
    private bool isOnGround = true;
    private float timer;
    private float zRotation;
    public bool isEscaping = false; // 用于标记鱼是否已经跳入水中

    void Start()
    {
        // 获取父物体上的 Rigidbody
        ParentRb = GetComponentInParent<Rigidbody>();

        // 初始化位置和计时器
        initialPosition = ParentRb.transform.position;
        timer = 0f;

        // 添加一定的空气阻力，防止无限累积速度
        ParentRb.drag = 1f;
        ParentRb.angularDrag = 1f;
    }

    void Update()
    {
        // 计算时间
        timer += Time.deltaTime;

        // 如果鱼在地面上并且没有进入逃脱状态
        if (isOnGround && !isEscaping)
        {
            if (timer >= struggleFrequency)
            {
                Struggle();
                timer = 0f; // 重置计时器
                struggleFrequency = UnityEngine.Random.Range(0.2f, 1.5f);
            }
        }

        // 限制鱼的高度
        Vector3 clampedPosition = ParentRb.transform.position;
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, groundLevel, groundLevel + maxJumpHeight);
        ParentRb.transform.position = clampedPosition;
    }

    // 挣扎行为
    void Struggle()
    {
        // 添加一个随机的垂直力（挣扎跳跃）
        ParentRb.AddForce(Vector3.up * UnityEngine.Random.Range(0.5f, struggleForce), ForceMode.Impulse);

        // 添加一个随机的水平力（左右挣扎移动）
        float randomDirection = UnityEngine.Random.Range(-0.2f, 0.9f);
        float randomZdirection= UnityEngine.Random.Range(-0.1f, 0.1f);
        Vector3 horizontalForce = new Vector3(randomDirection * horizontalMoveForce, 0, randomZdirection * horizontalMoveForce);
        ParentRb.AddForce(horizontalForce, ForceMode.Impulse);

        // 限制鱼的水平移动范围
        Vector3 clampedPosition = ParentRb.transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, initialPosition.x - maxHorizontalMovement, initialPosition.x + maxHorizontalMovement);
        ParentRb.transform.position = clampedPosition;
        float randomNum = UnityEngine.Random.Range(15f, 120f);
        // 初始化包含 -90f 和 90f 的数组
        float[] Orientationside = { -90f, 90f };
        System.Random random = new System.Random();

        // 随机选择 -90f 或 90f
        float randomOrientation = Orientationside[random.Next(0, 2)];
        ParentRb.transform.rotation = Quaternion.Euler(0, randomNum, randomOrientation);
    }

    // 鱼跳入水中的行为
    public void EscapeJump()
    {
        // 标记鱼已经进入逃脱状态
        isEscaping = true;

        // 向上、向右、和向前施加力
        ParentRb.AddForce((Vector3.up * escapeJumpForce) + (Vector3.right * escapeJumpForce) + (Vector3.forward * zMoveForce), ForceMode.Impulse);

        // 让鱼的头朝向正 X 方向（调整旋转）
        float randomNum = UnityEngine.Random.Range(45f, 120f);
        ParentRb.transform.rotation = Quaternion.Euler(0, randomNum, 0); // 将 Z 轴旋转设置为 0
        Collider parentCollider = GetComponentInParent<Collider>();

        // 如果父物体上存在 Collider，则删除它
 
        // 物理系统接管剩下的运动
    }

    // 碰撞检测，检查是否着地
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" && !isEscaping)
        {
            isOnGround = true;
            ParentRb.velocity = Vector3.zero; // 重置速度，防止累积过多力
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag != "Ground")
        {
            isOnGround = false;
        }
    }
}

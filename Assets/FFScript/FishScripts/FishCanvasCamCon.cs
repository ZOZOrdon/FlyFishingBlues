using UnityEngine;

public class FishStruggle : MonoBehaviour
{
    [Header("挣扎参数")]
    public float struggleForce = 5f; // 垂直跳跃力度
    public float horizontalMoveForce = 2f; // 水平移动力度
    public float struggleFrequency = 1f; // 挣扎频率
    public float groundLevel = 0f; // 地面高度
    public float maxHorizontalMovement = 5f; // 水平移动范围
    public float maxRotationZ = 80f; // Z 轴旋转角度限制

    private Vector3 initialPosition;
    private Rigidbody parentRb;
    private bool isOnGround = true;
    private float timer;
    private float zRotation;

    void Start()
    {
        // 获取父物体上的 Rigidbody
        parentRb = GetComponentInParent<Rigidbody>();

        // 获取初始位置
        initialPosition = parentRb.transform.position;

        // 初始化计时器
        timer = 0f;
    }

    void Update()
    {
        // 计算时间
        timer += Time.deltaTime;

        // 如果鱼在地面上
        if (isOnGround)
        {
            // 每到挣扎频率时，执行一次挣扎
            if (timer >= struggleFrequency)
            {
                Struggle();
                timer = 0f; // 重置计时器
            }
        }

        // 限制 Z 轴的旋转
        zRotation = Mathf.Clamp(transform.localEulerAngles.z, -maxRotationZ, maxRotationZ);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, zRotation);
    }

    // 挣扎行为
    void Struggle()
    {
        // 添加一个随机的垂直力（挣扎跳跃）
        parentRb.AddForce(Vector3.up * struggleForce, ForceMode.Impulse);

        // 添加一个随机的水平力（左右挣扎移动）
        float randomDirection = Random.Range(-1f, 1f);
        Vector3 horizontalForce = new Vector3(randomDirection * horizontalMoveForce, 0, 0);
        parentRb.AddForce(horizontalForce, ForceMode.Impulse);

        // 检查水平位置是否超出范围，限制鱼的水平移动
        Vector3 clampedPosition = parentRb.transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, initialPosition.x - maxHorizontalMovement, initialPosition.x + maxHorizontalMovement);
        parentRb.transform.position = clampedPosition;
    }

    // 碰撞检测，检查是否着地
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isOnGround = true;
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

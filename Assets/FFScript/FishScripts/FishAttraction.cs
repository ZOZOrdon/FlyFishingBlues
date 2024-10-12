using UnityEngine;
using UnityEngine.Splines;

public class FishAttraction : MonoBehaviour
{
    private Transform flyhook;
    public Transform exit1;
    public Transform exit2;
    public float moveSpeed = 2f;
    public float returnMoveSpeed = 3f;
    public float stopDistance = 0.5f;
    public float attractionDuration = 5f;
    public float waterSurfaceHeight = 1f; // 水面高度
    public float BiteChance = 0.5f; // 鱼咬钩的概率（范围：0-1）

    private SplineAnimate splineAnimate;
    public bool isAttracted = false;
    private bool isReturning = false;
    private float attractionTimer = 0f;
    private Transform currentTarget;

    void Start()
    {
        GameObject flyhookObject = GameObject.Find("flyhook");
        if (flyhookObject != null)
        {
            flyhook = flyhookObject.transform;
        }
        else
        {
            Debug.LogError("未找到名为 'flyhook' 的 GameObject！");
        }

        splineAnimate = GetComponent<SplineAnimate>();
    }

    void Update()
    {
        if (isAttracted && flyhook != null)
        {
            attractionTimer += Time.deltaTime;
            if (attractionTimer >= attractionDuration)
            {
                CheckBite();
            }
            else
            {
                FollowFlyhook();
            }
        }

        if (isReturning)
        {
            MoveTowardsTarget();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == flyhook)
        {
            splineAnimate.enabled = false;
            isAttracted = true;
            attractionTimer = 0f;
        }
    }

    private void FollowFlyhook()
    {
        float distanceToFlyhook = Vector3.Distance(transform.position, flyhook.position);
        if (distanceToFlyhook > stopDistance)
        {
            Vector3 direction = (flyhook.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);

            Vector3 targetPosition = Vector3.MoveTowards(transform.position, flyhook.position, moveSpeed * Time.deltaTime);
            targetPosition.y = Mathf.Min(targetPosition.y, waterSurfaceHeight);
            transform.position = targetPosition;
        }
        else
        {
            Vector3 direction = (flyhook.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);
        }
    }

    private void CheckBite()
    {
        float randomValue = Random.value; // 获取0到1之间的随机值
        if (randomValue < BiteChance)
        {
            // 鱼咬钩，激活FishBiteHook脚本并停用本脚本
            FishBiteHook biteHook = GetComponent<FishBiteHook>();
            if (biteHook != null)
            {
                biteHook.enabled = true;
            }
            this.enabled = false; // 停用本脚本
        }
        else
        {
            ExitAttraction(); // 不咬钩，继续执行脚本
        }
    }

    private void ExitAttraction()
    {
        isAttracted = false;
        isReturning = true;
        currentTarget = exit1;
    }

    private void MoveTowardsTarget()
    {
        if (currentTarget == null) return;
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
        if (distanceToTarget > stopDistance)
        {
            Vector3 direction = (currentTarget.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * returnMoveSpeed);

            Vector3 targetPosition = Vector3.MoveTowards(transform.position, currentTarget.position, returnMoveSpeed * Time.deltaTime);
            targetPosition.y = Mathf.Min(targetPosition.y, waterSurfaceHeight);
            transform.position = targetPosition;
        }
        else
        {
            if (currentTarget == exit1)
            {
                currentTarget = exit2;
            }
            else if (currentTarget == exit2)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}

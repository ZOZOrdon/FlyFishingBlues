using UnityEngine;

public class FishMassCon : MonoBehaviour
{
    // 水中属性
    public float InWaterMass = 1f;
    public float InWaterDrag = 2f;
    public float InWaterAngularDrag = 1f;

    // 水外属性
    public float OutWaterMass = 1f;
    public float OutWaterDrag = 0.5f;
    public float OutWaterAngularDrag = 0.5f;

    private Rigidbody fishRigidbody;
    private Collider waterSurfaceTrigger;

    void Start()
    {
        // 获取鱼的刚体组件
        fishRigidbody = GetComponent<Rigidbody>();

        if (fishRigidbody == null)
        {
            Debug.LogError("未找到鱼的Rigidbody组件！");
            return;
        }

        // 自动寻找名为"WaterSurfaceTrigger"的对象
        GameObject waterObject = GameObject.Find("WaterSurfaceTrigger");

        if (waterObject != null)
        {
            waterSurfaceTrigger = waterObject.GetComponent<Collider>();

            if (waterSurfaceTrigger == null)
            {
                Debug.LogError("WaterSurfaceTrigger对象上未找到Collider组件！");
            }
        }
        else
        {
            Debug.LogError("未找到名为'WaterSurfaceTrigger'的对象！");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == waterSurfaceTrigger)
        {
            // 进入水中，设置水中属性
            SetFishProperties(InWaterMass, InWaterDrag, InWaterAngularDrag);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other == waterSurfaceTrigger)
        {
            // 离开水面，设置水外属性
            SetFishProperties(OutWaterMass, OutWaterDrag, OutWaterAngularDrag);
        }
    }

    void SetFishProperties(float mass, float drag, float angularDrag)
    {
        fishRigidbody.mass = mass;
        fishRigidbody.drag = drag;
        fishRigidbody.angularDrag = angularDrag;
    }
}

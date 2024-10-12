using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject troutLocationPrefab; // 需要生成的TroutLocation1预制件
    public GameObject troutLocation1; // 场景中的TroutLocation1对象
    public float checkInterval = 1f; // 检查TroutLocation1是否存在的时间间隔
    public Vector3 spawnPosition = new Vector3(0, 0, 0); // 生成新TroutLocation1的位置

    void Start()
    {
        // 定时检查TroutLocation1是否被销毁
        InvokeRepeating("CheckTroutLocation", checkInterval, checkInterval);
    }

    void CheckTroutLocation()
    {
        // 如果TroutLocation1对象被销毁（即不存在），生成新的TroutLocation1
        if (troutLocation1 == null)
        {
            SpawnNewTroutLocation();
        }
    }

    void SpawnNewTroutLocation()
    {
        // 在指定位置生成新的TroutLocation1
        troutLocation1 = Instantiate(troutLocationPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("新TroutLocation1已生成！");
    }
}

using UnityEngine;
using UnityEngine.UI;

public class PushFishOnClick : MonoBehaviour
{
    public Button pushButton; // 需要设置的按钮
    public GameObject fish;   // 需要推动的Fish物体
    public float pushForce = 10f; // 推力的大小
    public Camera mainCamera; // 需要调整FOV的摄像机
    public float targetFOV = 70f; // 目标FOV值
    public float fovSmoothSpeed = 2f; // FOV调整的平滑速度

    private Rigidbody fishRigidbody;

    void Start()
    {
        if (fish != null)
        {
            fishRigidbody = fish.GetComponent<Rigidbody>();
        }

        if (pushButton != null)
        {
            pushButton.onClick.AddListener(OnButtonClick);
        }
    }

    void OnButtonClick()
    {
        ApplyPushForce();
        StartCoroutine(SmoothAdjustFOV());
    }

    void ApplyPushForce()
    {
        if (fishRigidbody != null)
        {
            Vector3 pushDirection = fish.transform.forward;
            fishRigidbody.AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }
    }

    System.Collections.IEnumerator SmoothAdjustFOV()
    {
        while (Mathf.Abs(mainCamera.fieldOfView - targetFOV) > 0.1f)
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, fovSmoothSpeed * Time.deltaTime);
            yield return null;
        }

        mainCamera.fieldOfView = targetFOV;
    }
}

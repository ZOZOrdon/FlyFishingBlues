using UnityEngine;
using System.Collections.Generic;

public class CameraFOVController : MonoBehaviour
{
    [System.Serializable]
    public class AnimationFOVSettings
    {
        public string animationStateName; // 动画状态名称
        public float fovIncreaseAmount = 5f; // 视野增加量
        public float fovSmoothTime = 0.5f; // 视野平滑过渡时间
        public float maxFOV = 90f; // 最大视野角度
    }

    public Animator characterAnimator; // 角色的Animator组件
    public List<AnimationFOVSettings> animationFOVSettingsList; // 动画与FOV设置的列表

    private Camera targetCamera;
    private Dictionary<int, AnimationFOVSettings> animationSettingsDict;
    private float defaultFOV;
    private float targetFOV;
    private float fovVelocity = 0f;
    private int currentAnimationHash = 0;

    void Start()
    {
        // 获取摄像机组件
        targetCamera = GetComponent<Camera>();
        if (targetCamera == null)
        {
            Debug.LogError("未在该游戏对象上找到Camera组件。");
            return;
        }

        // 检查是否已指定角色的Animator
        if (characterAnimator == null)
        {
            Debug.LogError("未指定角色的Animator组件。");
            return;
        }

        // 保存摄像机的默认FOV
        defaultFOV = targetCamera.fieldOfView;
        targetFOV = defaultFOV;

        // 初始化动画状态哈希值与设置的字典
        animationSettingsDict = new Dictionary<int, AnimationFOVSettings>();
        foreach (var settings in animationFOVSettingsList)
        {
            int animationHash = Animator.StringToHash(settings.animationStateName);
            if (!animationSettingsDict.ContainsKey(animationHash))
            {
                animationSettingsDict.Add(animationHash, settings);
            }
            else
            {
                Debug.LogWarning($"动画状态名称 '{settings.animationStateName}' 重复，已忽略。");
            }
        }
    }

    void Update()
    {
        if (characterAnimator == null || targetCamera == null) return;

        // 获取当前动画状态信息
        AnimatorStateInfo stateInfo = characterAnimator.GetCurrentAnimatorStateInfo(0);
        int currentStateHash = stateInfo.shortNameHash;

        // 检查当前动画是否在列表中
        if (animationSettingsDict.ContainsKey(currentStateHash))
        {
            // 如果当前动画发生变化，更新目标FOV
            if (currentAnimationHash != currentStateHash)
            {
                currentAnimationHash = currentStateHash;
                AnimationFOVSettings settings = animationSettingsDict[currentStateHash];
                 
                // 计算新的目标FOV，确保不超过最大值
                targetFOV = Mathf.Min(targetFOV + settings.fovIncreaseAmount, settings.maxFOV);
                // 重置fovVelocity，以免SmoothDamp受之前的速度影响
                fovVelocity = 0f;
            }
        
        
        }

        // 平滑过渡摄像机的FOV到目标值
        float smoothTime = animationSettingsDict.ContainsKey(currentStateHash) ? animationSettingsDict[currentStateHash].fovSmoothTime : 0.5f;
        targetCamera.fieldOfView = Mathf.SmoothDamp(targetCamera.fieldOfView, targetFOV, ref fovVelocity, smoothTime);
    }

    // 如果需要在其他地方重置摄像机的FOV，可以调用此方法
    public void ResetCameraFOV()
    {
        targetFOV = defaultFOV;
    }
}

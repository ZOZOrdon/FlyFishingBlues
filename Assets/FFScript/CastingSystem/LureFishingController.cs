using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LureFishingController : MonoBehaviour
{
    private Animator animator;
    private bool isControlling = false;
    private bool isCharging = false;
    private bool isSideCast = true; // true为侧抛，false为顺抛

    [Header("UI Settings")]
    public TextMeshProUGUI castingModeText; // 显示当前抛投模式的文本
    public float castingModeDisplayTime = 2f; // 模式提示显示时间
    private float modeDisplayTimer = 0f;

    [Header("Casting Settings")]
    public float maxChargeTime = 2f; // 最大蓄力时间
    private float currentChargeTime = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        UpdateCastingModeUI();
    }

    void Update()
    {
        // 切换抛投模式
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleCastingMode();
        }

        // 只有在按住空格键时才能操作
        if (Input.GetKey(KeyCode.Space))
        {
            isControlling = true;

            // 蓄力
            if (Input.GetKey(KeyCode.A))
            {
                if (!isCharging)
                {
                    StartCharging();
                }
                else
                {
                    ContinueCharging();
                }
            }
            // 释放蓄力，执行抛投
            else if (Input.GetKeyDown(KeyCode.D) && isCharging)
            {
                ExecuteCast();
            }
        }
        else
        {
            isControlling = false;
            if (isCharging)
            {
                ResetCharging();
            }
        }

        // 更新UI显示时间
        UpdateModeDisplay();
    }

    private void ToggleCastingMode()
    {
        isSideCast = !isSideCast;
        UpdateCastingModeUI();
        modeDisplayTimer = castingModeDisplayTime;
    }

    private void UpdateCastingModeUI()
    {
        if (castingModeText != null)
        {
            castingModeText.text = isSideCast ? "侧抛模式" : "顺抛模式";
            castingModeText.gameObject.SetActive(true);
        }
    }

    private void UpdateModeDisplay()
    {
        if (modeDisplayTimer > 0)
        {
            modeDisplayTimer -= Time.deltaTime;
            if (modeDisplayTimer <= 0 && castingModeText != null)
            {
                castingModeText.gameObject.SetActive(false);
            }
        }
    }

    private void StartCharging()
    {
        isCharging = true;
        currentChargeTime = 0f;
        // 播放相应的蓄力动画
        if (isSideCast)
        {
            animator.SetTrigger("SideCastCharge");
        }
        else
        {
            animator.SetTrigger("OverheadCastCharge");
        }
    }

    private void ContinueCharging()
    {
        if (currentChargeTime < maxChargeTime)
        {
            currentChargeTime += Time.deltaTime;
            // 可以在这里添加蓄力进度的视觉反馈
        }
    }

    private void ExecuteCast()
    {
        float chargePercentage = currentChargeTime / maxChargeTime;
        // 设置动画参数，控制抛投力度
        animator.SetFloat("CastPower", chargePercentage);
        
        // 播放相应的抛投动画
        if (isSideCast)
        {
            animator.SetTrigger("SideCastRelease");
        }
        else
        {
            animator.SetTrigger("OverheadCastRelease");
        }

        ResetCharging();
    }

    private void ResetCharging()
    {
        isCharging = false;
        currentChargeTime = 0f;
        animator.SetFloat("CastPower", 0f);
    }
} 
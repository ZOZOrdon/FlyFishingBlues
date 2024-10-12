using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyhookMassController : MonoBehaviour
{
    public BoxCollider waterSurfaceCollider;  // 引用WaterSurfaceCollider的Box Collider
    public float defaultMass = 1.0f;          // flyhook的默认质量
    public float waterMass = 0.5f;            // flyhook在水中的质量
    public float castingMass = 0.8f;          // flyhook在抛竿时的质量
    private Rigidbody flyhookRigidbody;       // flyhook的刚体组件
    public bool isInWater = false;           // 标记flyhook是否在水中
    private Animator characterAnimator;       // Character的动画组件

    void Start()
    {
        // 获取flyhook的刚体组件
        flyhookRigidbody = GetComponent<Rigidbody>();
        if (flyhookRigidbody == null)
        {
            Debug.LogError("Flyhook上缺少Rigidbody组件！");
        }

        // 设置初始质量为默认质量
        flyhookRigidbody.mass = defaultMass;

        // 获取名为"Character"的GameObject并获取其Animator组件
        GameObject characterObject = GameObject.Find("Character");
        if (characterObject != null)
        {
            characterAnimator = characterObject.GetComponent<Animator>();
            if (characterAnimator == null)
            {
                Debug.LogError("Character对象上缺少Animator组件！");
            }
        }
        else
        {
            Debug.LogError("场景中未找到名为'Character'的GameObject！");
        }
    }

    void Update()
    {
        // 如果flyhook在水中，优先使用waterMass
        if (isInWater)
        {
            flyhookRigidbody.mass = waterMass;
        }
        // 如果Character正在播放"CastIntoWater"动画，使用castingMass
        else if (characterAnimator != null && characterAnimator.GetCurrentAnimatorStateInfo(0).IsName("CastIntoWater"))
        {
            flyhookRigidbody.mass = castingMass;
            Debug.Log($"Flyhook 正在抛竿，质量设置为 {castingMass}");
        }
        // 否则，使用默认质量
        else
        {
            flyhookRigidbody.mass = defaultMass;
        }
    }

    // 触发器检测进入水中
    private void OnTriggerEnter(Collider other)
    {
        // 判断flyhook是否进入WaterSurfaceCollider
        if (other == waterSurfaceCollider)
        {
            isInWater = true;
            flyhookRigidbody.mass = waterMass;
            Debug.Log($"Flyhook 进入水中，质量设置为 {waterMass}");
        }
    }

    // 触发器检测离开水面
    private void OnTriggerExit(Collider other)
    {
        // 判断flyhook是否离开WaterSurfaceCollider
        if (other == waterSurfaceCollider)
        {
            isInWater = false;
            flyhookRigidbody.mass = defaultMass;
            Debug.Log($"Flyhook 离开水中，质量恢复为默认值 {defaultMass}");
        }
    }
}

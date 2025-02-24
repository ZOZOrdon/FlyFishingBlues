using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class ChoicCanvas : MonoBehaviour
{
    public NPCConversation Conversation;
    public Image CameraImage;
    public Vector3 targetPosition; // 目标位置
    public Vector2 targetSizeDelta; // 目标大小（使用 Size Delta）
    public float duration = 1.0f; // 动画持续时间
    public Ease easeType = Ease.InOutQuad; // 动画缓动类型

    public void ControllImageVisiablity()//diaglue里面调用这个方法
    {
     

        CameraImage.gameObject.SetActive(true);
 
    
    }
    public void ConrtolImageSize()
    { 
    RectTransform RectImage=CameraImage.GetComponent<RectTransform>();
        Vector3 targetLocalPosition = RectImage.parent.TransformPoint(targetPosition);
        RectImage.DOMove(targetLocalPosition, duration).SetEase(easeType);
       RectImage.DOScale(targetSizeDelta, duration).SetEase(easeType);
    }
   
    private void Start()
    {
        ConversationManager.Instance.StartConversation(Conversation);
    }
}

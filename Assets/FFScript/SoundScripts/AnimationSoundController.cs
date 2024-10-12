using UnityEngine;

public class AnimationSoundController : MonoBehaviour
{
    public Animator animator; // 动画组件
    public AudioSource audioSource; // 用于播放动画对应音效的音频源组件

    [System.Serializable]
    public class AnimationSoundPair
    {
        public string animationClipName; // 动画剪辑名称
        public AudioClip soundEffect; // 对应的音效
    }

    public AnimationSoundPair[] animationSoundPairs; // 动画和音效的对应数组

    // 新增部分
    public AudioClip dragSoundEffect; // Drag音效
    public AudioSource dragAudioSource; // 用于播放Drag音效的音频源组件
    public FishDragLine fishDragLine; // 对FishDragLine脚本的引用

    private string currentClipName = ""; // 当前播放的动画剪辑名称

    void Update()
    {
        // 检测动画并播放对应音效
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);

        if (clipInfo.Length > 0)
        {
            string newClipName = clipInfo[0].clip.name;

            if (currentClipName != newClipName)
            {
                currentClipName = newClipName;

                // 查找匹配的动画剪辑名称
                foreach (AnimationSoundPair pair in animationSoundPairs)
                {
                    if (pair.animationClipName == newClipName)
                    {
                        audioSource.clip = pair.soundEffect;
                        audioSource.Play();
                        break;
                    }
                }
            }
        }

        // 新增部分：检测FishDragLine的布尔变量并播放或停止Drag音效
        if (fishDragLine != null && dragSoundEffect != null && dragAudioSource != null)
        {
            if (fishDragLine.isDragging || fishDragLine.isStruggling)
            {
                if (!dragAudioSource.isPlaying)
                {
                    dragAudioSource.clip = dragSoundEffect;
                    dragAudioSource.Play();
                }
            }
            else
            {
                if (dragAudioSource.isPlaying)
                {
                    dragAudioSource.Stop();
                }
            }
        }
    }
}

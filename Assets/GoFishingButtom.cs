using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoFishingButtom : MonoBehaviour
{
    // 引用按钮组件
    public Button nextButton;

    void Start()
    {
        if (nextButton != null)
        {
            // 为按钮的点击事件添加监听器
            nextButton.onClick.AddListener(LoadNextScene);
        }
        else
        {
            Debug.LogError("Next Button is not assigned in the inspector.");
        }
    }

    // 加载下一个场景的方法
    void LoadNextScene()
    {
        // 获取当前活动场景的索引
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // 计算下一个场景的索引
        int nextSceneIndex = currentSceneIndex + 1;

        // 检查下一个场景是否存在
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // 加载下一个场景
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("已经是最后一个场景，无法加载下一个场景。");
        }
    }
}

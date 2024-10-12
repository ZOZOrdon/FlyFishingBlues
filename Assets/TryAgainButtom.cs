using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TryAgainButtom : MonoBehaviour
{
    // 引用按钮组件
    public Button reloadButton;

    void Start()
    {
        if (reloadButton != null)
        {
            // 为按钮的点击事件添加监听器
            reloadButton.onClick.AddListener(ReloadCurrentScene);
        }
        else
        {
            Debug.LogError("Reload Button is not assigned in the inspector.");
        }
    }

    // 重新加载当前场景的方法
    void ReloadCurrentScene()
    {
        // 获取当前活动场景的名称
        string sceneName = SceneManager.GetActiveScene().name;
        // 重新加载场景
        SceneManager.LoadScene(sceneName);
    }
}

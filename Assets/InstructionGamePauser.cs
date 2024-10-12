using UnityEngine;

public class InstructionGamePauser : MonoBehaviour
{
    private bool isPaused = false;  // 用于检测游戏是否已暂停
    public KeyCode resumeKey;  // 公共变量，用于指定恢复游戏的按键

    void OnTriggerEnter(Collider other)
    {
        // 当检测到触发器碰撞时，暂停游戏
        if (!isPaused)
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        // 设置游戏暂停
        Time.timeScale = 0f;  // 暂停游戏，0为暂停，1为正常速度
        isPaused = true;
        Debug.Log("Game Paused");
    }

    void ResumeGame()
    {
        // 恢复游戏
        Time.timeScale = 1f;  // 恢复游戏
        isPaused = false;
        Debug.Log("Game Resumed");

        // 将自身的GameObject失活
        gameObject.SetActive(false);
    }

    void Update()
    {
        // 检测玩家是否同时按下空格键和指定的resumeKey以恢复游戏
        if (isPaused && Input.GetKey(KeyCode.Space) && Input.GetKeyDown(resumeKey))
        {
            ResumeGame();
        }
    }
}

using UnityEngine;

public class InstructionGamePauser : MonoBehaviour
{
    private bool isPaused = false; // 用于检测游戏是否已暂停

    void OnTriggerEnter(Collider other)
    {
        // 当检测到触发器碰撞时，暂停游戏
        if (!isPaused)  // 如果游戏尚未暂停
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        // 设置游戏暂停
        Time.timeScale = 0f; // 暂停游戏，0为暂停，1为正常速度
        isPaused = true;
        Debug.Log("Game Paused");
    }

    void ResumeGame()
    {
        // 恢复游戏
        Time.timeScale = 1f; // 恢复游戏
        isPaused = false;
        Debug.Log("Game Resumed");
    }

    void Update()
    {
        // 检测玩家按下空格并按下D以恢复游戏
        if (isPaused && Input.GetKey(KeyCode.Space) && Input.GetKeyDown(KeyCode.D))
        {
            ResumeGame();
        }
    }
}

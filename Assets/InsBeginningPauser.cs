using UnityEngine;
using System.Collections;

public class InsBeginningPauser : MonoBehaviour
{
    private bool isTimeFrozen = true;
    public float countdownDuration = 3f;

    // UI components
    public GameObject uiComponent1;
    public GameObject uiComponent2;

    void Start()
    {
        // Start countdown before freezing time
        StartCoroutine(CountdownThenFreeze());
    }

    IEnumerator CountdownThenFreeze()
    {
        yield return new WaitForSecondsRealtime(countdownDuration);

        // Freeze time after countdown
        Time.timeScale = 0f;
    }

    void Update()
    {
        // Check if the player is holding down the space key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Deactivate the first UI component and activate the second UI component
            if (uiComponent1 != null && uiComponent2 != null)
            {
                uiComponent1.SetActive(false);
                uiComponent2.SetActive(true);
            }
        }

        // Check if the player is holding down the space key and presses the A key
        if (isTimeFrozen && Input.GetKey(KeyCode.Space) && Input.GetKeyDown(KeyCode.A))
        {
            // Unfreeze time
            Time.timeScale = 1f;
            isTimeFrozen = false;

            // Deactivate the second UI component
            if (uiComponent2 != null)
            {
                uiComponent2.SetActive(false);
            }
        }
    }
}
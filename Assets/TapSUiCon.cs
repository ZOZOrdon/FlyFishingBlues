using UnityEngine;
using TMPro;

public class TapSUiCon : MonoBehaviour
{
    public GameObject uiElement; // Reference to the UI element to show or hide
    private FlyhookMassController flyhookMassController;
    private TextMeshProUGUI textMeshPro;
    public Animator characterAnimator;

    void Start()
    {
        // Find the FlyhookMassController in the scene
        flyhookMassController = FindObjectOfType<FlyhookMassController>();

        // Ensure uiElement is not null
        if (uiElement == null)
        {
            Debug.LogError("UI Element is not assigned to TapSUiCon");
        }
        else
        {
            // Get the TextMeshProUGUI component from the UI element
            textMeshPro = uiElement.GetComponentInChildren<TextMeshProUGUI>();
            if (textMeshPro == null)
            {
                Debug.LogError("TextMeshProUGUI component is not found in the UI element");
            }
        }

        // Find the Character animator in the scene
        characterAnimator = FindObjectOfType<Animator>();
        if (characterAnimator == null)
        {
            Debug.LogError("Character Animator is not found in the scene");
        }
    }

    void Update()
    {
        if (flyhookMassController != null && textMeshPro != null && characterAnimator != null)
        {
            // Check if the SetTheHook animation is playing
            if (characterAnimator.GetCurrentAnimatorStateInfo(0).IsName("SetTheHook"))
            {
                uiElement.SetActive(false);
                return;
            }

            // UI element stays active, toggle TextMeshProUGUI component based on IsInWater
            if (flyhookMassController.isInWater)
            {
                textMeshPro.gameObject.SetActive(true);
            }
            else
            {
                textMeshPro.gameObject.SetActive(false);
            }
        }
        else if (flyhookMassController == null)
        {
            Debug.LogError("FlyhookMassController script is not found in the scene");
        }
    }
}
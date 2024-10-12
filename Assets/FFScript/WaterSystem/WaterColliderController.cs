using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class WaterColliderController : MonoBehaviour
{
    // Reference to the BoxCollider component
    private BoxCollider boxCollider;

    // Update is called once per frame
    void Start()
    {
        // Get the BoxCollider component attached to this GameObject
        boxCollider = GetComponent<BoxCollider>();

        // Initially disable the BoxCollider
        boxCollider.enabled = false;
    }

    void Update()
    {
        // Check if the Space key is being held down
        if (Input.GetKey(KeyCode.Space))
        {
            // Enable the BoxCollider if Space is pressed
            if (!boxCollider.enabled)
            {
                boxCollider.enabled = true;
                Debug.Log("BoxCollider Enabled");
            }
        }
        else
        {
            // Disable the BoxCollider if Space is not pressed
            if (boxCollider.enabled)
            {
                boxCollider.enabled = false;
                Debug.Log("BoxCollider Disabled");
            }
        }
    }
}

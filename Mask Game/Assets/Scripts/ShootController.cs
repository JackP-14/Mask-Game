using UnityEngine;
using UnityEngine.InputSystem;
public class ShootController : MonoBehaviour
{
    [SerializeField] private string targetTag = "Target";
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameController gameController;
    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }
    void Update()
    {
        // Check for left mouse button click using new Input System
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (ZoomController.Zoom0 == true) return;
            HandleClick();
        }

    }
    void HandleClick()
    {
        // Get mouse position using new Input System
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag(targetTag))
            {
                OnCorrectTagClicked(hit.collider.gameObject);
            }
            else
            {
                OnWrongTagOrMissClicked();
            }
        }
        else
        {
            OnWrongTagOrMissClicked();
            gameController.lives = 0;
        }
    }
    void OnCorrectTagClicked(GameObject clickedObject)
    {
        Debug.Log("Clicked object with correct tag: " + clickedObject.name);

        // Add your condition here
    }

    void OnWrongTagOrMissClicked()
    {
        Debug.Log("Clicked wrong object or empty space");

        // Add your other condition here
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
public class SetCamToCursor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Camera cam;

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            // Get mouse position in world space BEFORE zoom
          Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, cam.nearClipPlane));
            mouseWorldPos.z = cam.transform.position.z; // Preserve camera's Z position
            
            // Change camera size
            cam.orthographicSize = 1f;
            
            // Move camera to where the cursor was
            cam.transform.position = mouseWorldPos;


        }
    }
}
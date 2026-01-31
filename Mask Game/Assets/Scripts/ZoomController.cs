using UnityEngine;
using UnityEngine.InputSystem;

public class ZoomController : MonoBehaviour
{
    public Camera MainCam;
    public Transform Crosshair;
    public GameObject Zoom0Wall;
    public GameObject Zoom2CameraController;
    public static bool Zoom0 = true;
    public static bool Zoom1 = false;
    public static bool Zoom2 = false;
    public GameObject hud;
    public GameObject cross;

    void Update()
    {
        //Vector3 mousePos = Mouse.current.position.ReadValue();
        //Vector3 worldPos = MainCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, MainCam.nearClipPlane));
        //worldPos.z = -10f;
        //Debug.Log($"Cursor en pantalla: {mousePos} | Cursor en mundo: {worldPos}");
        if (Keyboard.current != null &&
        Keyboard.current.wKey.wasPressedThisFrame)
        {
            if (Zoom0 == true && Zoom1 == false)
            {
                Zoom1 = true;
                Zoom0 = false;
                Debug.Log("Zoom0 Off");
                Debug.Log("Zoom1 On");
                Zoom0Wall.SetActive(false);
                hud.SetActive(false);
                cross.SetActive(true);

            }
            else if (Zoom0 == false && Zoom1 == true)
            {
                Zoom1 = false;
                Zoom2 = true;
                Debug.Log("Zoom1 Off");
                Debug.Log("Zoom2 On");
                hud.SetActive(false);
                cross.SetActive(true);

                Crosshair.localScale = Crosshair.localScale / 5;
                Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
                Vector3 mouseWorldPos = MainCam.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, MainCam.nearClipPlane));
                mouseWorldPos.z = MainCam.transform.position.z; // Preserve camera's Z position

                // Change camera size
                MainCam.orthographicSize = 1f;

                // Move camera to where the cursor was
                MainCam.transform.position = mouseWorldPos;
                Crosshair.position = mouseWorldPos; 
                Zoom2CameraController.SetActive(true);
            }

        }
        if (Keyboard.current != null &&
        Keyboard.current.sKey.wasPressedThisFrame)
        {
            if (Zoom1 == true && Zoom0 == false)
            {
                Zoom0 = true;
                Zoom1 = false;
                Debug.Log("Zoom1 Off");
                Debug.Log("Zoom0 On");
                Zoom0Wall.SetActive(true);
                hud.SetActive(true);
                cross.SetActive(false);

            }
            else if (Zoom1 == false && Zoom2 == true)
            {
                Zoom2 = false;
                Zoom1 = true;
                Debug.Log("Zoom2 Off");
                Debug.Log("Zoom1 On");
                Zoom2CameraController.SetActive(false);
                MainCam.orthographicSize = 5f;
                MainCam.transform.position = new Vector3(0, 0, -10);
                Crosshair.localScale = Crosshair.localScale * 5;

            }

        }
    }
    //public void TeleportarCamaraAlCursor()
    //{
    //    Vector3 mousePos = Mouse.current.position.ReadValue();
    //    Vector3 worldPos = MainCam.ScreenToWorldPoint(mousePos);
    //    worldPos.z = -10f;
    //    Cam.position = worldPos;
    //}
}


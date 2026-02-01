using UnityEngine;
using UnityEngine.InputSystem;

public class ZoomController : MonoBehaviour
{
    public Camera MainCam;
    public Transform Crosshair;
    public GameObject Zoom0Wall;
    public GameObject Zoom2CameraController;
    public GameObject Zoom0_Base;
    public static bool Zoom_Q = false;
    public static bool Zoom0 = true;
    public static bool Zoom1 = false;
    public static bool Zoom2 = false;
    
    [Header("Hud and Elements")]
    public GameObject hud;
    public class hud_elements
    {
        public GameObject background;
        public GameObject radio;
        public GameObject PC;
        public GameObject notebook;
        public GameObject textbox;
    }
    public GameObject cross;
    public float fadeDuration = 1.0f;
    [Header("Audio Clip")]
    public AudioClip soundToPlay;
    private AudioSource audioSource;
    void Start()
    {
        
        Zoom_Q = false;
        Zoom0 = true;
        Zoom1 = false;
        Zoom2 = false;
        Cursor.visible = false;
        Zoom0_Base.SetActive(true);
        hud.SetActive(false);
        Zoom0Wall.SetActive(true);
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (Keyboard.current != null &&
        Keyboard.current.wKey.wasPressedThisFrame)
        {
            if (Zoom2) return;
            if (!Zoom_Q)
            {
                audioSource.PlayOneShot(soundToPlay);
            }
            if (Zoom_Q)
            {
                Cursor.visible = false;
                Debug.Log("Zoom0 On");
                Zoom_Q = false;
                Zoom0 = true;
                hud.SetActive(false);
                Zoom0_Base.SetActive(true);
            }
            else if (Zoom0 && !Zoom1)
            {
                Zoom1 = true;
                Zoom0 = false;
                Debug.Log("Zoom1 On");
                Zoom0_Base.SetActive(false);
                Zoom0Wall.SetActive(false);
                cross.SetActive(true);
            }
            else if (Zoom0 == false && Zoom1 == true)
            {
                Zoom1 = false;
                Zoom2 = true;
                Debug.Log("Zoom2 On");
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
            if (Zoom_Q) return;
            if (!Zoom0)
            {
                audioSource.PlayOneShot(soundToPlay);
            }
            if (Zoom0 && !Zoom_Q)
            {
                Cursor.visible = true;
                Zoom0 = false;
                Zoom_Q = true;
                hud.SetActive(true);
                Zoom0_Base.SetActive(false);
            }
            else if (Zoom1 && !Zoom0)
            {
                Zoom0 = true;
                Zoom1 = false;
                Debug.Log("Zoom0 On");
                Zoom0Wall.SetActive(true);
                Zoom0_Base.SetActive(true);
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

}


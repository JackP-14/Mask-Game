using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ZoomController : MonoBehaviour
{
    public Camera MainCam;
    public Transform Crosshair;
    public GameObject Zoom0Wall;
    public static bool Zoom0 = true;
    public static bool Zoom1 = false;
    public static bool Zoom2 = false;
    void Update()
    {
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
               
            }
            else if(Zoom0 == false &&  Zoom1 == true)
            {
                Zoom1 = false;
                Zoom2 = true;
                Debug.Log("Zoom1 Off");
                Debug.Log("Zoom2 On");
                MainCam.orthographicSize = 1;
                Crosshair.localScale = Crosshair.localScale/5;
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

            }
            else if (Zoom1 == false && Zoom2 == true)
            {
                Zoom2 = false;
                Zoom1 = true;
                Debug.Log("Zoom2 Off");
                Debug.Log("Zoom1 On");
                MainCam.orthographicSize = 5;
                Crosshair.localScale = Crosshair.localScale * 5;
            }

        }
    }
    
}


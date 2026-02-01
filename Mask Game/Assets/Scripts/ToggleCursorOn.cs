using UnityEngine;
using UnityEngine.InputSystem;
public class ToggleCursorOn : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true;
        SaveData.Instance.lives = 5;
    }

}

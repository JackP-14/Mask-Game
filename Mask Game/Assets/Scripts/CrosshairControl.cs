using UnityEngine;
using UnityEngine.InputSystem;

public class CrosshairControl : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = 10f;

        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
    }
}

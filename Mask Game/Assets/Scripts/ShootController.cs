using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using System.Collections;
public class ShootController : MonoBehaviour
{
    [SerializeField] private string targetTag = "Target";
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameController gameController;
    [Header("Audio Clip")]
    public AudioClip soundToPlay;
    private AudioSource audioSource;
    [Header("Black Screen after shot")]
    public GameObject blackscreen;
    private SpriteRenderer black_renderer;
    void Start()
    {
        black_renderer = blackscreen.GetComponent<SpriteRenderer>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        // Check for left mouse button click using new Input System
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (ZoomController.Zoom0 == true) return;
            FadeIn(black_renderer, blackscreen, 0.5f);
            audioSource.PlayOneShot(soundToPlay);
            Invoke("HandleClick", 1f);
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
                return;
            }
        }
        OnWrongTagOrMissClicked();
        SaveData.Instance.lives = 0;
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
    public void FadeIn(SpriteRenderer spriteRenderer, GameObject targetObject, float fadeDuration = 0.5f)
    {
        StartCoroutine(FadeInCoroutine(spriteRenderer, targetObject, fadeDuration));
    }

    private IEnumerator FadeInCoroutine(SpriteRenderer spriteRenderer, GameObject targetObject, float fadeDuration)
    {
        float elapsed = 0f;
        Color color = spriteRenderer.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = 0f + (elapsed / fadeDuration);
            spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        spriteRenderer.color = new Color(color.r, color.g, color.b, 1f);
        //targetObject.SetActive(false);
    }
}

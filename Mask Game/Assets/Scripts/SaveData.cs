using UnityEngine;

public class SaveData : MonoBehaviour
{
    public static SaveData Instance;

   public int lives = 10;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}

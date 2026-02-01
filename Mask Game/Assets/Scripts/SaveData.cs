using UnityEngine;

public class SaveData : MonoBehaviour
{
    public static SaveData Instance;

   public int lives = 10;
   public int ShowedTutorial = 0;
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

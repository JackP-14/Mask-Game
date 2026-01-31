using UnityEngine;

public class CultistRandomSpawn : MonoBehaviour
{

    [Header("Object to be spawned at level start")]
    public GameObject Cultist;
    [Header("Maximum number of cultists")]
    public int MaxCultists;
    [Header("Minimum number of cultists")]
    public int MinCultists;

    void Start()
    {
        int NumberOfCultists = Random.Range(MinCultists, MaxCultists+1);
        for (int i = 0; i < NumberOfCultists; i++)
        {
            Spawn_cultist();
        }
    }

    public void Spawn_cultist()
    {
        float SpawnPointX = 0f;
        float SpawnPointY = 0f;
        int SpawnLvlY = Random.Range(0, 3);

        if (SpawnLvlY == 0)
        {
            SpawnPointY = -3f;
            SpawnPointX = Random.Range(-4.6f, 4.3f);
        }
        else if (SpawnLvlY == 1)
        {
            SpawnPointY = -1f;
            SpawnPointX = Random.Range(-4.6f, 5.3f);
        }
        else
        {
            SpawnPointY = 1f;
            SpawnPointX = Random.Range(-4.6f, 3.4f);
        }

        Vector2 SpawnPosition = new Vector2(SpawnPointX, SpawnPointY);

        Instantiate(Cultist, SpawnPosition, Quaternion.identity);
    }
}
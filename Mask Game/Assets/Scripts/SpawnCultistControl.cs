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
        int SpawnPointX = Random.Range(-4, 3);
        int SpawnLvlY = Random.Range(0, 3);
        int SpawnPointY = 0;

        if (SpawnLvlY == 0)
        {
            SpawnPointY = -3;
        }
        else if (SpawnLvlY == 1)
        {
            SpawnPointY = -1;
        }
        else
        {
            SpawnPointY = 1;
        }

        Vector2 SpawnPosition = new Vector2(SpawnPointX, SpawnPointY);

        Instantiate(Cultist, SpawnPosition, Quaternion.identity);
    }
}
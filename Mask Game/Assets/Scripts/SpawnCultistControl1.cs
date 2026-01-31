using UnityEngine;
using System.Collections.Generic;

public class CultistRandomSpawn1 : MonoBehaviour
{
    [Header("Object to be spawned at level start")]
    public GameObject Cultist;

    [Header("Maximum number of cultists")]
    public int MaxCultists;

    [Header("Minimum number of cultists")]
    public int MinCultists;

    private List<CultistRandomizer> spawnedCultists = new List<CultistRandomizer>();
    private CultistRandomizer targetCultist;

    void Start()
    {
        int NumberOfCultists = Random.Range(MinCultists, MaxCultists + 1);

        // Primero spawneamos todos los cultistas
        for (int i = 0; i < NumberOfCultists; i++)
        {
            Spawn_cultist(i);
        }

        // Después seleccionamos UNO como target
        if (spawnedCultists.Count > 0)
        {
            int targetIndex = Random.Range(0, spawnedCultists.Count);
            targetCultist = spawnedCultists[targetIndex];
            targetCultist.SetAsTarget();

            Debug.Log($"[TARGET] Cultista objetivo creado: Mask={targetCultist.maskTypeIndex}, Eyes={targetCultist.maskEyesIndex}, Accessory={targetCultist.maskAccessoryIndex}, Color={targetCultist.colorIndex}");
        }
    }

    public void Spawn_cultist(int spawnIndex)
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
        GameObject newCultist = Instantiate(Cultist, SpawnPosition, Quaternion.identity);

        SpriteRenderer[] renderers = newCultist.GetComponentsInChildren<SpriteRenderer>(true);
        int layerOffset = spawnIndex * 4;
        Debug.Log($"=== Cultista {spawnIndex} - Offset: {layerOffset} ===");
        foreach (var r in renderers)
        {
            int oldOrder = r.sortingOrder;
            r.sortingOrder += layerOffset;
            Debug.Log($"'{r.gameObject.name}': {oldOrder} -> {r.sortingOrder}");
        }

        CultistRandomizer randomizer = newCultist.GetComponent<CultistRandomizer>();
        if (randomizer != null)
        {
            // Generar apariencia aleatoria
            randomizer.RandomizeCultist();

            // Verificar que no sea idéntico al target (si ya existe)
            if (targetCultist != null)
            {
                int maxAttempts = 50;
                int attempts = 0;

                while (randomizer.IsSameAppearance(targetCultist.maskTypeIndex, targetCultist.maskEyesIndex,
                                                    targetCultist.maskAccessoryIndex, targetCultist.colorIndex)
                       && attempts < maxAttempts)
                {
                    randomizer.RandomizeCultist();
                    attempts++;
                }

                if (attempts >= maxAttempts)
                {
                    Debug.LogWarning("No se pudo generar un cultista único después de varios intentos");
                }
            }

            spawnedCultists.Add(randomizer);
        }
    }

    public CultistRandomizer GetTargetCultist()
    {
        return targetCultist;
    }
}
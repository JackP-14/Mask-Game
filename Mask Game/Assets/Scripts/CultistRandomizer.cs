using UnityEngine;

public class CultistRandomizer : MonoBehaviour
{
    [Header("Parent GameObjects")]
    public Transform maskTypeParent;
    public Transform maskEyesParent;
    public Transform maskAccessoryParent;

    [Header("Color Options")]
    public Color[] cultistColors;

    [Header("Accessory Options")]
    [Range(0f, 1f)]
    public float accessoryProbability = 1f;

    // Datos de la apariencia del cultista (públicos pero ocultos en inspector)
    [HideInInspector]
    public int maskTypeIndex = -1;
    [HideInInspector]
    public int maskEyesIndex = -1;
    [HideInInspector]
    public int maskAccessoryIndex = -1; // -1 = sin accesorio
    [HideInInspector]
    public int colorIndex = -1;

    private bool isTarget = false;

    void Awake()
    {
        // Si ya es el target, la apariencia se configuró desde CultistRandomSpawn
        if (!isTarget)
        {
            RandomizeCultist();
        }
    }

    public void SetAsTarget()
    {
        isTarget = true;
        gameObject.tag = "Target";
    }

    public void RandomizeCultist()
    {
        // Seleccionar índice de color aleatorio
        if (cultistColors.Length > 0)
        {
            colorIndex = Random.Range(0, cultistColors.Length);
        }
        else
        {
            colorIndex = 0;
        }

        Color selectedColor = (cultistColors.Length > 0) ? cultistColors[colorIndex] : Color.white;

        // Randomizar Mask Type
        if (maskTypeParent != null && maskTypeParent.childCount > 0)
        {
            foreach (Transform child in maskTypeParent)
            {
                child.gameObject.SetActive(false);
            }

            maskTypeIndex = Random.Range(0, maskTypeParent.childCount);
            Transform selectedMask = maskTypeParent.GetChild(maskTypeIndex);
            selectedMask.gameObject.SetActive(true);

            SpriteRenderer sr = selectedMask.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = selectedColor;
            }
        }

        // Randomizar Mask Eyes
        if (maskEyesParent != null && maskEyesParent.childCount > 0)
        {
            foreach (Transform child in maskEyesParent)
            {
                child.gameObject.SetActive(false);
            }

            maskEyesIndex = Random.Range(0, maskEyesParent.childCount);
            Transform selectedEyes = maskEyesParent.GetChild(maskEyesIndex);
            selectedEyes.gameObject.SetActive(true);

            SpriteRenderer sr = selectedEyes.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = selectedColor;
            }
        }

        // Randomizar Mask Accessory
        maskAccessoryIndex = -1; // Por defecto sin accesorio

        if (maskAccessoryParent != null && maskAccessoryParent.childCount > 0)
        {
            foreach (Transform child in maskAccessoryParent)
            {
                child.gameObject.SetActive(false);
            }

            if (Random.value <= accessoryProbability)
            {
                maskAccessoryIndex = Random.Range(0, maskAccessoryParent.childCount);
                Transform selectedAccessory = maskAccessoryParent.GetChild(maskAccessoryIndex);
                selectedAccessory.gameObject.SetActive(true);
            }
        }
    }

    public void SetAppearance(int maskType, int maskEyes, int maskAccessory, int color)
    {
        maskTypeIndex = maskType;
        maskEyesIndex = maskEyes;
        maskAccessoryIndex = maskAccessory;
        colorIndex = color;

        Color selectedColor = (cultistColors.Length > 0 && color >= 0) ? cultistColors[color] : Color.white;

        // Configurar Mask Type
        if (maskTypeParent != null && maskTypeParent.childCount > 0 && maskType >= 0)
        {
            foreach (Transform child in maskTypeParent)
            {
                child.gameObject.SetActive(false);
            }

            Transform selectedMask = maskTypeParent.GetChild(maskType);
            selectedMask.gameObject.SetActive(true);

            SpriteRenderer sr = selectedMask.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = selectedColor;
            }
        }

        // Configurar Mask Eyes
        if (maskEyesParent != null && maskEyesParent.childCount > 0 && maskEyes >= 0)
        {
            foreach (Transform child in maskEyesParent)
            {
                child.gameObject.SetActive(false);
            }

            Transform selectedEyes = maskEyesParent.GetChild(maskEyes);
            selectedEyes.gameObject.SetActive(true);

            SpriteRenderer sr = selectedEyes.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = selectedColor;
            }
        }

        // Configurar Mask Accessory
        if (maskAccessoryParent != null && maskAccessoryParent.childCount > 0)
        {
            foreach (Transform child in maskAccessoryParent)
            {
                child.gameObject.SetActive(false);
            }

            if (maskAccessory >= 0)
            {
                Transform selectedAccessory = maskAccessoryParent.GetChild(maskAccessory);
                selectedAccessory.gameObject.SetActive(true);
            }
        }
    }

    public bool IsSameAppearance(int maskType, int maskEyes, int maskAccessory, int color)
    {
        return maskTypeIndex == maskType &&
               maskEyesIndex == maskEyes &&
               maskAccessoryIndex == maskAccessory &&
               colorIndex == color;
    }
}
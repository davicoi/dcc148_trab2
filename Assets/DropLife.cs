using UnityEngine;

public class DropLife : MonoBehaviour
{
    public GameObject lowPotionPrefab, normalPotionPrefab, bigPotionPrefab;
    float[] potionPerf = { 0.55f, 0.85f, 1f };

    public static DropLife Instance { get; private set; }
    private void Awake() {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }


    void Start()
    {
        lowPotionPrefab = Resources.Load("items/RedVial") as GameObject;
        normalPotionPrefab = Resources.Load("items/RedPotion") as GameObject;
        bigPotionPrefab = Resources.Load("items/HeartGem") as GameObject;
    }

    public void dropPotion(Vector3 position, float extraLucky = 0f, Transform parent = null) {
        float perc = Random.Range(0f, 1f) * (1f + extraLucky);

        GameObject prefab;
        if (perc < potionPerf[0]) {
            prefab = lowPotionPrefab;
        } else if (perc < potionPerf[1]) {
            prefab = normalPotionPrefab;
        } else {
            prefab = bigPotionPrefab;
        }

        position.y += 1f;
        GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.Translate(position, Space.Self);

    }
}

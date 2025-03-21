using UnityEngine;

public class ActionAddLife : MonoBehaviour
{
    [SerializeField] int lifeAmount = 1;

    void Start()
    {
        Debug.Log("Life amount: " + lifeAmount);
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            playerController.addHealth(lifeAmount);
            Destroy(gameObject);
        }
    }
}

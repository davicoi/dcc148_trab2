using UnityEngine;

public class ActionWin : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            GameController.Instance.execEndGame(true);
        }
    }
}

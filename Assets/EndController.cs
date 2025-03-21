using UnityEngine;
using TMPro;

public class EndController : MonoBehaviour
{
    [SerializeField] TMP_Text txWinnner;
    [SerializeField] TMP_Text txLoser;
    [SerializeField] GameObject endGamePanel;
    // GameController gameController;


    void Start()
    {
        // gameController = GameObject.Find("GameManager").GetComponent<GameController>();
        getEndGamePanel();
    }

    GameObject getEndGamePanel() {
        if (endGamePanel != null)
            return endGamePanel;

        foreach (Transform child in GameObject.Find("Canvas").transform) {
            if (child.name == "EndGame") {
                endGamePanel = child.gameObject;
                break;
            }
        }

        foreach (Transform child in endGamePanel.transform) {
            if (child.name == "txWin") {
                txWinnner = child.GetComponent<TMP_Text>();
            } else if (child.name == "txLost") {
                txLoser = child.GetComponent<TMP_Text>();
            }
        }

        return endGamePanel;
    }

    public void endGame(bool winner) {
        getEndGamePanel();
        txWinnner.gameObject.SetActive(winner);
        txLoser.gameObject.SetActive(!winner);
        show(true);
    }

    public void show(bool status) {
        getEndGamePanel().gameObject.SetActive(status);
    }

    public void eventExit() {
        Debug.Log("EndGame Exit");
        Application.Quit();
    }
}

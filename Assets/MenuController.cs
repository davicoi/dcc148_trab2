using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject mainMenuPanel;
    GameController gameController;

    void Start()
    {
        getMainMenuPanel();
    }

    GameController getMainMenuPanel() {
        if (gameController != null)
            return gameController;

        getGameController();

        foreach (Transform child in GameObject.Find("Canvas").transform) {
            if (child.name == "MainMenu") {
                mainMenuPanel = child.gameObject;
                break;
            }
        }

        return gameController;
    }

    GameController getGameController() {
        if (!gameController)
            gameController = GameObject.Find("GameManager").GetComponent<GameController>();

        return gameController;
    }

    public void show(bool enable) {
        getMainMenuPanel();
        mainMenuPanel.gameObject.SetActive(enable);
    }

    public void eventMap10() {
        getGameController().execStartGame(10);
    }

    public void eventMap20() {
        getGameController().execStartGame(20);
    }

    public void eventMap30() {
        getGameController().execStartGame(30);
    }

    public void eventExit() {
        Debug.Log("Menu Exit");
        Application.Quit();
    }
}

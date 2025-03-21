using UnityEngine;
public class GameController : MonoBehaviour
{
    public EndController endController;
    public MenuController menuController;
    public HUDController hudController;
    public ToolController toolController;
    public GameObject player;
    public Rigidbody playerRB;
    int activeTool = -1;
    bool isRunning = false;
    bool firstRun = true;

    public static GameController Instance { get; private set; }
    private void Awake() {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }



    void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        menuController = canvas.GetComponent<MenuController>();
        endController = canvas.GetComponent<EndController>();
        hudController = canvas.GetComponent<HUDController>();
        toolController = GameObject.Find("ToolList").GetComponent<ToolController>();
        player = GameObject.Find("Player");
        playerRB = player.GetComponent<Rigidbody>();

        playerEnable(false);
        activeTool = 1;
    }

    void FirstRun() {
        playerEnable(false);

        //execStartGame(30);
        execMainMenu();
        //execEndGame(false);
        //execEndGame(true);
    }

    public void playerEnable(bool enable) {
        if (enable) {
            toolController.setTool(activeTool);
        } else {
            if (playerRB.isKinematic)
                activeTool = toolController.getToolId();
            toolController.disableAll();
        }
        playerRB.isKinematic = !enable;
    }

    public void execEndGame(bool win) {
        playerEnable(false);
        menuController.show(false);
        endController.endGame(win);
        isRunning = false;
        // unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void execMainMenu() {
        playerEnable(false);
        endController.show(false);
        menuController.show(true);
        isRunning = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void execStartGame(int roomsCount = 20) {
        endController.show(false);
        menuController.show(false);
        setMapSize(roomsCount);
        playerEnable(true);
        isRunning = true;
    }

    public bool isPlaying() {
        return isRunning;
    }

    void setMapSize(int size) {
        MapGenerator mapGenerator = GameObject.Find("MapManager").GetComponent<MapGenerator>();
        mapGenerator.roomSize = 10;
        mapGenerator.mapSize = 20;
        mapGenerator.roomCount = size;
        mapGenerator.generate();
    }
    


    void Update()
    {
        if (firstRun) {
            firstRun = false;
            FirstRun();
        }
    }
}

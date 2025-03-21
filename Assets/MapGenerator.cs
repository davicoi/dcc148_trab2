using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] public int mapSize = 5;
    [SerializeField] public int roomSize = 10;
    [SerializeField] public int roomCount = 10;
    [SerializeField] int roomDist = 5;
    [SerializeField] float doorSize = 4;
    [SerializeField] GameObject[] monsterHighProb;
    [SerializeField] GameObject[] monsterLowerProb;
    [SerializeField] float highProb = 0.9f;
    [SerializeField] GameObject monsterContainer;
    [SerializeField] float distOfActiveMonsters = 40;
    GameObject RoomPrefab, player;
    float distElapsed = 0;

    void Start()
    {
        // load resource
        player = GameObject.Find("Player");
        RoomPrefab = Resources.Load("custom/RoomGenerator") as GameObject;
        if (RoomPrefab == null) {
            Debug.Log("RoomPrefab is null");
        }
        //createRooms(mapSize, roomSize, roomCount, roomDist);
    }
    public void generate() {
        createRooms(mapSize, roomSize, roomCount, roomDist);
    }

    void createRooms(int mapSize, int roomSize, int roomCount, int roomDist = 5) {
        RoomGenerator randomRooms = new RoomGenerator();
        int startX = mapSize / 2;
        int startY = mapSize / 2;
        int [,] map = randomRooms.randomRooms(mapSize, roomCount, startX, startY);
        
        for (int i = 0 ; i < 30 && randomRooms.genCount != roomCount ; i++) {
            map = randomRooms.randomRooms(mapSize, roomCount, startX, startY);
        }
        randomRooms.printRoomList();

        int difX = startX*roomSize + startX*roomDist;
        int difZ = startY*roomSize + startY*roomDist;


        for (int y = 0; y < mapSize; y++) {
            for (int x = 0; x < mapSize; x++) {
                if (map[y, x] != 0) {
                    Vector3 pos = new Vector3(x * roomSize + x * roomDist - difX, 0, y * roomSize + y * roomDist - difZ);
                    GameObject room = Instantiate(RoomPrefab, Vector3.zero, Quaternion.identity, transform);
                    RoomController roomController = room.GetComponent<RoomController>();
                    room.transform.localPosition = Vector3.zero;
                    roomController.roomWidth = roomSize;
                    roomController.roomHeight = roomSize;
                    room.transform.Translate(pos, Space.Self);

                    roomController.doorSize = doorSize;
                    roomController.topDoor = (map[y, x] & 1) > 0 ? true : false;
                    roomController.rightDoor = (map[y, x] & 2) > 0 ? true : false;
                    roomController.bottomDoor = (map[y, x] & 4) > 0 ? true : false;
                    roomController.leftDoor = (map[y, x] & 8) > 0 ? true : false;

                    //Debug.Log("x=" + (x * roomSize + x * roomDist) + ", y=" + (y * roomSize + y * roomDist  ));
                    //room.transform.position = new Vector3(x * roomDist, 0, y * roomDist);
                    if (x != startX || y != startY) {
                        randomMonster(x * roomSize + x * roomDist - difX, 3, y * roomSize + y * roomDist - difZ, roomSize, roomSize);
                    }
                }
            }
        }

        //
        Vector3 trophyPos = randomRooms.getLastRoomPosition();
        trophyPos.x = trophyPos.x*roomSize + trophyPos.x*roomDist - difX;
        trophyPos.z = trophyPos.z*roomSize + trophyPos.z*roomDist - difZ;
        addTrophy(trophyPos.x, trophyPos.z);
    }

    void addTrophy(float x, float z) {
        GameObject trophyPrefab = Resources.Load("items/Trophy") as GameObject;
        GameObject trophy = Instantiate(trophyPrefab, Vector3.zero, Quaternion.identity, transform);
        trophy.transform.localPosition = Vector3.zero;
        trophy.transform.Translate(new Vector3(x, 2, z), Space.Self);
    }

    void randomMonster(float centerX, float y, float centerZ, float width, float height) {
        int count = Random.Range(3, 5);

        GameObject prefab, monster;
        for (int i = 0; i < count; i++) {
            Vector3 pos = new Vector3(Random.Range(centerX - width/2, centerX + width/2), y, Random.Range(centerZ - height/2, centerZ + height/2));
            if (Random.value < highProb) {
                prefab = monsterHighProb[Random.Range(0, monsterHighProb.Length)];
            } else {
                prefab = monsterLowerProb[Random.Range(0, monsterLowerProb.Length)];
            }

            if (!prefab) {
                Debug.Log("prefab is null");
                continue;
            }

            monster = Instantiate(prefab, Vector3.zero, Quaternion.identity, monsterContainer.transform);
            monster.transform.localPosition = Vector3.zero;
            monster.transform.Translate(pos, Space.Self);
        }
    }

    void disableDistantMonsters() {
        // childs in monsterContainer
        foreach (Transform child in monsterContainer.transform) {
            if (!GameController.Instance.isPlaying() || Vector3.Distance(child.position, player.transform.position) > distOfActiveMonsters) {
                child.gameObject.SetActive(false);
            } else {
                child.gameObject.SetActive(true);
            }
        }
    }

    void FixedUpdate()
    {
        distElapsed += Time.fixedDeltaTime;
        if (distElapsed >= 1) {
            disableDistantMonsters();
        }
    }
}

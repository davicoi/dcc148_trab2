using UnityEngine;

public class RoomController : MonoBehaviour
{
    GameObject ground, wall;
    public bool leftDoor = false;
    public bool rightDoor = false;
    public bool topDoor = false;
    public bool bottomDoor = false;

    [SerializeField] public float roomWidth = 10;
    [SerializeField] public float roomHeight = 10;
    [SerializeField] public float corridorSize = 5;
    [SerializeField] public float doorSize = 3f;
    [SerializeField] public float wallHeight = 5f;

    void Start()
    {
        ground = loadPrefab("base/base_ground");
        wall = loadPrefab("base/base_wall");

        createRoom(roomWidth, roomHeight, corridorSize);
    }

    GameObject loadPrefab(string name) {
        return Resources.Load(name) as GameObject;
    }

    void createRoom(float width, float height, float corridorSize, float x = 0, float z = 0) {
        GameObject ground = createGround(width, height, x, z);

        GameObject wallLeft = createWallWithDoor(leftDoor, height + 0.2f, true, -(width/2), 0, -(height/2));
        GameObject wallRight = createWallWithDoor(rightDoor, height, true, +(width/2), 0, -(height/2));

        GameObject wallTop = createWallWithDoor(topDoor, width, false, -(width/2), 0, +(height/2));
        GameObject wallBottom = createWallWithDoor(bottomDoor, width, false, -(width/2), 0, -(height/2));

        if (corridorSize > 0) {
            if (topDoor)
                createCorridor(0, width, height, doorSize, corridorSize);
            if (rightDoor)
                createCorridor(1, width, height, doorSize, corridorSize);
            if (bottomDoor)
                createCorridor(2, width, height, doorSize, corridorSize);
            if (leftDoor)
                createCorridor(3, width, height, doorSize, corridorSize);
        }
    }

    GameObject createGround(float width, float height, float x, float z) {
        GameObject ground1 = Instantiate(ground, Vector3.zero, Quaternion.identity, transform);
        ground1.transform.localPosition = Vector3.zero;
        ground1.transform.localScale = new Vector3(width + 0.5f, 1, height + 0.5f);
        ground1.transform.Translate(x, 0, z);
        return ground1;
    }

    GameObject createWall(float width, bool vertical, float x, float y, float z, float thickness = 0.2f) {
        GameObject wallx;
        y += wallHeight/2;
        if (vertical) {
            z += width/2;
            wallx = Instantiate(wall, Vector3.zero, Quaternion.identity, transform);
            wallx.transform.localPosition = Vector3.zero;
            wallx.transform.localScale = new Vector3(thickness, wallHeight, width);
            wallx.transform.Translate(x, y, z);
        } else {
            x += width/2;
            wallx = Instantiate(wall, Vector3.zero, Quaternion.identity, transform);
            wallx.transform.localPosition = Vector3.zero;
            wallx.transform.localScale = new Vector3(width, wallHeight, thickness);
            wallx.transform.Translate(x, y, z);
        }
        return wallx;
    }

    GameObject createWallWithDoor(bool door,float width, bool vertical, float x, float y, float z, float thickness = 0.2f) {
        GameObject wallx, wallx2;
        float newWidth = width/2 - doorSize/2;
        thickness *= 2;

        if (door == false)
            return createWall(width, vertical, x, y, z, thickness);

        if (vertical) {
            wallx = createWall(newWidth, vertical, x, y, z, thickness);
            wallx2 = createWall(newWidth, vertical, x, y, z + newWidth + doorSize, thickness);
        } else {
            wallx = createWall(newWidth, vertical, x, y, z, thickness);
            wallx2 = createWall(newWidth, vertical, x + newWidth + doorSize, y, z, thickness);
        }
        wallx2.transform.SetParent(wallx.transform);
        return wallx;
    }

    void createCorridor(int dir, float width, float height, float doorSize, float corridorSize, float thickness = 0.2f) {
        float x, z, w, h;
        doorSize += thickness;
        if (dir == 0 || dir == 2) {
            x = 0;
            z = +(width/2 + corridorSize/2);
            if (dir == 2)
                z -= width + corridorSize;
            w = doorSize;
            h = corridorSize;
            createGround(w, h, x, z);
            createWall(h, true, -doorSize/2, 0, z-corridorSize/2, thickness);
            createWall(h, true, +doorSize/2, 0, z-corridorSize/2, thickness);

        } else {
            x = -(height/2 + corridorSize/2);
            z = 0;
            if (dir == 1)
                x += height + corridorSize;
            w = corridorSize;
            h = doorSize;
            createGround(w, h, x, z);
            createWall(w, false, x-corridorSize/2, 0, -doorSize/2, thickness);
            createWall(w, false, x-corridorSize/2, 0, +doorSize/2, thickness);
        }
    }
}

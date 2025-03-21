using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float headHeightFactor = 3f/4f;
    public float mouseSensitivityX = 2.0f;
    public float mouseSensitivityY = 2.0f;

    private float currentRotationX = 0f;
    private float currentRotationY = 0f;
    GameController gameController;

    GameObject player;
    float addY = 1;
    void Start()
    {
        gameController = GameObject.Find("GameManager").GetComponent<GameController>();
        player = GameObject.Find("Player");
        Bounds bounds = player.GetComponent<Renderer>().bounds;
        addY = bounds.extents.y / headHeightFactor;

        Camera.main.transform.rotation = Quaternion.identity;
    }

    void FixedUpdate()
    {
        if (!gameController.isPlaying())
            return;

        if (Cursor.visible && Input.GetMouseButtonDown(0)) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY;

        currentRotationX += mouseX;
        currentRotationY -= mouseY;

        currentRotationY = Mathf.Clamp(currentRotationY, -90f, 90f);
        transform.rotation = Quaternion.Euler(currentRotationY, currentRotationX, 0f);
    }


    void LateUpdate()
    {
        if (player && player.activeSelf)
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + addY, player.transform.position.z);
    }
}

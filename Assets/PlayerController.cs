using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float speed = 5;
    [SerializeField] float jumpForce = 10;
    [SerializeField] float maxHealth = 100;
    [SerializeField] float healthRengen = 1;
    AudioSource audioSource;
    Rigidbody rb;

    float health;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
        meshRenderer.enabled = false;
        health = maxHealth;
    }

    void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;
        float dx = Input.GetAxis("Horizontal");
        float dz = Input.GetAxis("Vertical");
        Vector3 dir = (Camera.main.transform.right * dx) + (Camera.main.transform.forward * dz);
        dir.y = 0;
        transform.Translate(dir.normalized * speed * delta, Space.World);

        if (UnityEngine.Input.GetButton("Jump") || UnityEngine.Input.GetKey(KeyCode.Space)) {
            if (rb.linearVelocity.y == 0) {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        if (health < maxHealth)
            health += healthRengen * delta;
    }

    public void takeDamage(float damage) {
        health -= damage;
        if (health < 1) {
            // Destroy(gameObject);
            // TODO: DEATH SCREEN
            GameController.Instance.execEndGame(false);
        }
        // Debug.Log("Player Health: " + health);

        if (!audioSource.isPlaying) {
            audioSource.Play();
            Invoke("pauseSound", audioSource.clip.length/11);
        }
    }

    public void addHealth(float amount) {
        health += amount;
        if (health > maxHealth)
            health = maxHealth;
    }

    void pauseSound() {
        audioSource.Pause();
    }

    public float getHealth() {
        return health;
    }

    public float getMaxHealth() {
        return maxHealth;
    }
}

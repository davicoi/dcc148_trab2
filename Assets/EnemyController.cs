using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] string animIdle = "Idle";
    [SerializeField] string animWalk = "Walk";
    [SerializeField] string animRun = "Run";
    [SerializeField] string animAttack = "Attack";
    [SerializeField] string animDeath = "Death";
    [SerializeField] public float health = 60f;
    [SerializeField] public float damage = 10f;
    [SerializeField] LayerMask collisionLayer;
    [SerializeField] float raycastFrequency = 0.250f;
    [SerializeField] float extraSpeed = 0f;
    [SerializeField] float attackDuration = 2f;
    [SerializeField] bool enemyShoot = false;
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip hitSound;
    float lifeDropChance = 0.3f;
    float lifeDropLucky = 0f;

    GameObject player;
    PlayerController playerController;
    AudioSource audioSource;
    float elapsed, attackElapsed;
    bool playerVisible = false;
    Vector3 lastPlayerPosition;
    bool isAttacking = false, isDead = false;
    GameObject bulletPrefab;

    void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        collisionLayer = LayerMask.GetMask("Wall");
        audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        lastPlayerPosition = transform.position;
        bulletPrefab = Resources.Load("bullets/Bullet1") as GameObject;
        elapsed = 0;
    }

    bool playerIsVisible() {
        Vector3 playerPosition = player.transform.position;
        Vector3 dir = playerPosition - transform.position;
        RaycastHit hit;
        bool res = false;

        if (Physics.Raycast(transform.position, dir, out hit, dir.magnitude, collisionLayer)) {
            //Debug.Log("Raycast colidiu com: " + hit.collider.name);
        } else {
            //Debug.Log("Player visível");
            res = true;
        }

        Debug.DrawRay(transform.position, dir, Color.green);
        return res;
    }

    void FixedUpdate()
    {
        elapsed += Time.fixedDeltaTime;

        if (isDead) {
            return;
        }

        if (isAttacking) {
            attackElapsed += Time.fixedDeltaTime;
            if (attackElapsed >= attackDuration) {
                isAttacking = false;
                attackElapsed = 0;
            }
        }

        if (elapsed >= raycastFrequency) {
            elapsed = 0;
            if (playerIsVisible()) {
                if (!isAttacking) {
                    if (extraSpeed <= 0.5f)
                        GetComponent<Animator>().Play(animWalk);
                    else
                        GetComponent<Animator>().Play(animRun);
                }
                lastPlayerPosition = player.transform.position;
                playerVisible = true;

                // olha para a direção do movimento
                Vector3 dir = (lastPlayerPosition - transform.position).normalized;
                dir.y = 0;
                transform.rotation = Quaternion.LookRotation(dir);

            } else {
                if (!isAttacking)
                    GetComponent<Animator>().Play(animIdle);
                playerVisible = false;
            }
        }

        if (Vector3.Distance(transform.position, lastPlayerPosition) > 3f) {
            float speed = enemyShoot ? 1.0f : playerController.speed / 2 + extraSpeed;
            Vector3 dir = (lastPlayerPosition - transform.position).normalized;
            dir.y = 0;
            transform.Translate(dir * speed * Time.fixedDeltaTime, Space.World);
        } 
        
        if (!isAttacking && ((Vector3.Distance(transform.position, player.transform.position) <= 3f && !isAttacking) || (enemyShoot && playerVisible))) {
            audioSource.PlayOneShot(attackSound, 0.3f);
            isAttacking = true;
            attackElapsed = 0;
            GetComponent<Animator>().Play(animAttack);
            if (enemyShoot)
                shoot();
            else
                playerController.takeDamage(damage);
        }
    }

    public void takeDamage(float damage) {
        if (isDead)
            return;

        health -= damage;
        if (health <= 0) {
            isDead = true;
            GetComponent<Animator>().Play(animDeath);
            randomDropLife();
            Destroy(gameObject, 1.4f);
            return;
        }
        playHitSound();
    }

    void randomDropLife() {
        if (lifeDropLucky < -9999)
            return;

        if (Random.Range(0f, 1f) * (1f + lifeDropLucky) < lifeDropChance)
            DropLife.Instance.dropPotion(transform.position, lifeDropLucky, transform.parent);
        lifeDropLucky = -9999;
    }

    void shoot() {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.transform.rotation = transform.rotation;
        bullet.transform.Translate(Vector3.up * transform.localScale.y);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        bulletController.Fire(20f, damage, 30f, false);
    }

    void playHitSound() {
        if (hitSound != null)
            audioSource.PlayOneShot(hitSound, 1f);
    }
}

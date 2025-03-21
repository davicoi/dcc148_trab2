using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] ParticleSystem impactPrefab;

    float dist = 0, maxDist = 100f;
    float speed = 1;
    float damage = 0;
    bool isPlayer = true;
    
    void Start()
    {

    }

    public void Fire(float speed, float damage, float range, bool isPlayer = true) {
        dist = 0;
        maxDist = range;
        this.speed = speed;
        this.damage = damage;
        this.isPlayer = isPlayer;
    }

    void FixedUpdate() {
        dist += speed * Time.fixedDeltaTime;
        if (dist > maxDist)
            Destroy(gameObject);

        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player" && !isPlayer) {
            other.gameObject.GetComponent<PlayerController>().takeDamage(damage);
            Destroy(gameObject);

        } else if (other.tag == "Enemy" && isPlayer) {
            EnemyController enemyController = other.gameObject.GetComponent<EnemyController>();
            if (enemyController != null)
                enemyController.takeDamage(damage);
            Destroy(gameObject);
        
        } else if (other.tag != "Player" && other.tag != "Enemy") {
            Destroy(gameObject);
            // instanciate impact particle system
//             if (impactPrefab != null) {
//                 ParticleSystem p = Instantiate(impactPrefab, transform.position, transform.rotation);
// //                Destroy(impact.gameObject, 1f);
//             }

        }

        // ParticleSystem p2 = Instantiate(impactPrefab, transform.position, transform.rotation);
        // p2.Play();

    }
}

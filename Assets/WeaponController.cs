using System;
using System.Data;
using UnityEngine;

// gun script for fps
public class WeaponController : MonoBehaviour
{
    [SerializeField] float fireRate = 0.2f;
    [SerializeField] float reloadTime = 1f;
    [SerializeField] float bulletSpeed = 100f;
    [SerializeField] float damage = 10f;
    [SerializeField] float range = 50f;
    [SerializeField] int maxAmmo = 15;
    [SerializeField] int currentAmmo = 15;
    [SerializeField] AudioClip bulletSound;
    [SerializeField] AudioClip reloadSound;
    public HUDController hudController;
    GameObject bulletPoint;
    GameObject bulletPrefab;
    AudioSource audioSource;
    bool reloadPressed = false;
    
    public bool isFiring = false;
    public bool isReloading = false;
    float elapsed;

    void Start()
    {
        bulletPoint = GameObject.Find("BulletPoint");
        bulletPrefab = Resources.Load("bullets/Bullet1") as GameObject;
        audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        hudController = GameObject.Find("Canvas").GetComponent<HUDController>();
        hudController.setAmmo(currentAmmo, maxAmmo);        
    }

    void FixedUpdate()
    {
        elapsed += Time.fixedDeltaTime;
        if (isReloading) {
            elapsed += Time.fixedDeltaTime;
            if (elapsed >= reloadTime) {
                currentAmmo = maxAmmo;
                isReloading = false;
                elapsed = 0;
                hudController.setAmmo(currentAmmo, maxAmmo);
            }
            return;
        }

        if (isFiring && elapsed >= fireRate) {
            elapsed = 0;;
            newFire();
        }

        // FIXME: Remove
        isFiring = Input.GetButton("Fire1");

        if (!isReloading && !reloadPressed && Input.GetKey(KeyCode.R)) {
            reloadPressed = true;
            reload();
        } else
            reloadPressed = false;

    }
    public void reload() {
        isReloading = true;
        elapsed = 0;

        if (reloadSound != null)
            audioSource.PlayOneShot(reloadSound, 0.5f);
    }

    void newFire() {
        if (currentAmmo <= 0) {
            reload();
            return;
        }

        if (bulletSound != null)
            audioSource.PlayOneShot(bulletSound, 0.5f);

        GameObject bullet = Instantiate(bulletPrefab, bulletPoint.transform.position, bulletPoint.transform.rotation);

        Vector3 dir = raycastHitPoint();
        if (dir != Vector3.zero) {
            dir = (dir - bulletPoint.transform.position).normalized;
            bullet.transform.rotation = Quaternion.LookRotation(dir);
        } else {
            bullet.transform.rotation = Camera.main.transform.rotation;
        }

        BulletController bulletController = bullet.GetComponent<BulletController>();
        bulletController.Fire(bulletSpeed, damage, range);

        currentAmmo--;
        hudController.setAmmo(currentAmmo, maxAmmo);
        Destroy(bullet, 5f);
    }

    Vector3 raycastHitPoint()
    {
        Vector3 centerScreen = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(centerScreen);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            return hit.point;
        }

        return Vector3.zero;
    }

    public void changedToActive() {
        if (hudController)
            hudController.setAmmo(currentAmmo, maxAmmo);        
    }

}

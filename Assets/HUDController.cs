using UnityEngine;
using TMPro;
using System;

public class HUDController : MonoBehaviour
{
    TMP_Text txHealth;
    TMP_Text txAmmo;
    GameObject player;
    PlayerController playerController;
    int oldLife = -1;

    void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();

        txHealth = GameObject.Find("txLife").GetComponent<TMP_Text>();
        txAmmo = GameObject.Find("txAmmo").GetComponent<TMP_Text>();
    }

    void updateHealth() {
        if (player == null || !player.activeSelf)
            return;

        int health = (int)Math.Floor(Math.Clamp(playerController.getHealth(), 0, playerController.getMaxHealth()));
        int maxHealth = (int)playerController.getMaxHealth();
        txHealth.SetText(health + "/" + maxHealth);
    }

    void FixedUpdate()
    {
        if (player == null || !player.activeSelf)
            return;

        int health = (int)Math.Floor(Math.Clamp(playerController.getHealth(), 0, playerController.getMaxHealth()));
        if (oldLife != health) {
            updateHealth();
            oldLife = health;
        }
    }

    public void setAmmo(int ammo, int maxAmmo) {
        if (txAmmo == null)
            return;
        ammo = (int)Mathf.Clamp(ammo, 0, maxAmmo);
        txAmmo.SetText(ammo + "/" + maxAmmo);
    }
}

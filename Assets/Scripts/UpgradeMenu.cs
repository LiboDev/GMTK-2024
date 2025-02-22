using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    private EnemySpawner enemySpawner;
    [SerializeField] private PlayerController player;

    [SerializeField] private GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpgradePressed(string upgrade)
    {
        switch (upgrade)
        {
            case "damage":
                player.bulletDamage++;
                if (player.bulletDamage > player.maxDamage)
                {
                    player.bulletDamage = player.maxDamage;
                    return;
                }
                transform.GetChild(0).GetChild(1).gameObject.GetComponent<Slider>().value = (float)player.bulletDamage / player.maxDamage;
                break;

            case "fireRate":
                player.bulletsPerSecond++;
                if (player.bulletsPerSecond > player.maxBPS)
                {
                    player.bulletsPerSecond = player.maxBPS;
                    return;
                }
                transform.GetChild(1).GetChild(1).gameObject.GetComponent<Slider>().value = (float)player.bulletsPerSecond / player.maxBPS;
                break;

            case "range":
                player.range++;
                if (player.range > player.maxRange)
                {
                    player.range = player.maxRange;
                    return;
                }
                transform.GetChild(2).GetChild(1).gameObject.GetComponent<Slider>().value = (float)player.range / player.maxRange;
                break;

            case "damageReduction":
                player.damageReduction+=0.1f;
                if (player.damageReduction > player.maxDamageReduction)
                {
                    player.damageReduction = player.maxDamageReduction;
                    return;
                }
                print("Damage Reduction: " + player.damageReduction + "\nMax Damage Reduction: " + player.maxDamageReduction + "\nPercent of max the player has: " + (player.damageReduction / player.maxDamageReduction));
                transform.GetChild(3).GetChild(1).gameObject.GetComponent<Slider>().value = (float)player.damageReduction / player.maxDamageReduction;
                break;

            case "speed":
                player.speed++;
                if (player.speed > player.maxSpeed)
                {
                    player.speed = player.maxSpeed;
                    return;
                }
                player.returnTimeModifier += 0.1f;
                if (player.returnTimeModifier > player.maxReturnTimeModifier)
                {
                    player.returnTimeModifier = player.maxReturnTimeModifier;
                }
                transform.GetChild(4).GetChild(1).gameObject.GetComponent<Slider>().value = (float)player.speed / player.maxSpeed;
                break;

            case "knockback":
                player.bulletKnockback++;
                if (player.bulletKnockback > player.maxKnockback)
                {
                    player.bulletKnockback = player.maxKnockback;
                    return;
                }
                transform.GetChild(5).GetChild(1).gameObject.GetComponent<Slider>().value = (float)player.bulletKnockback / player.maxKnockback;
                break;

        }

        panel.SetActive(false);

        gameObject.SetActive(false);
        enemySpawner.waveStart = true;
    }

    public void SetEnemySpawner(GameObject spawner)
    {
        if (spawner.GetComponent<EnemySpawner>() != null)
        {
            enemySpawner = spawner.GetComponent<EnemySpawner>();
        }
        else
        {
            Debug.LogError("No EnemySpawner Script Found");
        }
    }
}

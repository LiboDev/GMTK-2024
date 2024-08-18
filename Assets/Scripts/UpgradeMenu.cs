using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    private EnemySpawner enemySpawner;
    [SerializeField] private PlayerController player;

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
                break;

            case "fireRate":
                player.bulletsPerSecond++;
                break;

            case "knockback":
                player.bulletKnockback++;
                break;

            case "range":
                player.range++;
                break;

            case "damageReduction":
                player.damageReduction++;
                break;

            case "speed":
                player.speed++;
                break;
        }

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

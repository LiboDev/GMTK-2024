using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //scene
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private Transform player;

    [SerializeField] private GameObject pauseMenu;

    //tracking
    private int wave = 1;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Wave());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleTime();
        }
    }

    public void ToggleTime()
    {
        if(Time.timeScale == 1)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }

    private IEnumerator Wave()
    {
        while (true)
        {
            for(int i = 0; i < wave * 10; i++)
            {
                int rand = Random.Range(0, enemies.Length);

                Vector2 circle = Random.insideUnitCircle;
                circle.Normalize();
                GameObject enemy = Instantiate(enemies[rand], player.position + new Vector3(circle.x,circle.y,0)*100, Quaternion.identity);
                enemy.transform.parent = this.transform;

                EnemyController enemyController = enemy.GetComponent<EnemyController>();

                if (rand == 0)
                {
                    enemyController.SetSize(Random.Range(1,wave));
                    enemyController.SetSpeed(5);
                }
                else if(rand == 1)
                {
                    enemyController.SetSize(Random.Range(1, wave));
                    enemyController.SetSpeed(5);
                }
                else if(rand == 2)
                {
                    enemyController.SetSize(5);
                    enemyController.SetBulletsPerSecond(wave);
                    enemyController.SetBulletDamage(wave);
                }

                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitUntil(() => transform.childCount <= 0);

            //play sfx loop again

            yield return null;
        }

    }
}

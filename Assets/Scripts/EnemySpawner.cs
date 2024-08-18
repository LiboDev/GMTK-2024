using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    //scene
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private Transform player;

    [SerializeField] private GameObject pauseMenu;

    //tracking
    private int wave = 1;
    public bool waveStart = true;

    //Events
    UnityEvent<GameObject> waveOver;


    // Start is called before the first frame update
    void Start()
    {
        if (waveOver == null)
        {
            waveOver = new UnityEvent<GameObject>();
        }

        waveOver.AddListener(player.gameObject.GetComponent<PlayerController>().StartUpgrade);

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

            waveOver.Invoke(gameObject);

            waveStart = false;

            yield return new WaitUntil(() => waveStart);

            //play sfx loop again

            yield return null;
        }

    }
}

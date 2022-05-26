using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    GameObject enemy;
    [SerializeField]
    int numOfEnemiesAtFirstWave, enemyIncrementByWave;
    [SerializeField]
    float height = 1.2f, timeDecrementAtKill = 2f;
    [SerializeField]
    float arenaHalfSize = 20f;
    [SerializeField]
    Transform arena;
    [SerializeField]
    Canvas canvas;

    Transform parent;
    int numOfEnemies;
    float seconds = 0;
    private void Awake()
    {
        parent = transform;
        numOfEnemies = numOfEnemiesAtFirstWave;
    }
    void Start()
    {
        StartCoroutine(Spawn(1f, numOfEnemiesAtFirstWave));
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            seconds++;
            canvas.GetComponent<UnityEngine.UI.Text>().text = "Time: " + seconds + " seconds";
        }
    }
    public void EnemyDestroyed()
    {
        seconds -= timeDecrementAtKill;
        canvas.GetComponent<UnityEngine.UI.Text>().text = "Time: " + seconds + " seconds";

        //if(seconds <= 0)

        if (transform.childCount == 1)
            NextWave();
    }
    public void NextWave()
    {
        numOfEnemies += enemyIncrementByWave;
        StartCoroutine(Spawn(1f, numOfEnemies));
    }
    private IEnumerator Spawn(float wait, int numOfEnemies)
    {
        yield return new WaitForSeconds(wait);

        for (; numOfEnemies > 0; numOfEnemies--)
        {
            //spawn enemies at a random point on arena
            GameObject obj = Instantiate(enemy, GetSpawnPoint(), Quaternion.identity, parent);

            //Debug.Log(obj.transform.position);
            yield return new WaitForSeconds(0.05f);
        }
    }
    //the spawn point vector must be within the arena size and rotated by the rotation of the arena
    private Vector3 GetSpawnPoint() => arena.rotation * new Vector3(Random.Range(-arenaHalfSize, arenaHalfSize), height, Random.Range(-arenaHalfSize, arenaHalfSize));
}

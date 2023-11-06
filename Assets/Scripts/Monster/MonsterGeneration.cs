using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGeneration : MonoBehaviour
{
    public float monsterDelay;
    public Transform[] spawnPoints;
    public GameObject littleMonster;
    public GameObject pursuer;
    public GameObject boss;
    public GameObject rangedBoss;
    private float respawnTimer;
    private int rangedSpawnThreshold; 

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("MonsterSpawn");
        StartCoroutine("RangedSpawn");
        respawnTimer = 0;
        rangedSpawnThreshold = 100;
    }

    // Update is called once per frame
    void Update()
    {

        if(GameObject.FindGameObjectsWithTag("Boss").Length != 0 && GameObject.FindGameObjectsWithTag("Pursuer").Length != 0) {
            respawnTimer = 0;
        }
        respawnTimer += Time.deltaTime;

        if(respawnTimer > 8f && GameObject.FindGameObjectsWithTag("Boss").Length == 0) {
            respawnTimer = 0;
            int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
            Transform randomSpawnPoint = spawnPoints[randomSpawnIndex];
            Instantiate(boss, randomSpawnPoint.position, Quaternion.identity);
        }

        if(respawnTimer > 4f && GameObject.FindGameObjectsWithTag("Pursuer").Length == 0) {
            respawnTimer = 0;
            int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
            Transform randomSpawnPoint = spawnPoints[randomSpawnIndex];
            Instantiate(pursuer, randomSpawnPoint.position, Quaternion.identity);
        }

    }

    void Spawn() {
        // Pick random spawn points and random asteroid prefabs
        int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
        Transform randomSpawnPoint = spawnPoints[randomSpawnIndex];

        // Spawn
        Instantiate(littleMonster, randomSpawnPoint.position, Quaternion.identity);
    }

    IEnumerator MonsterSpawn() {
        // Wait
        yield return new WaitForSeconds(monsterDelay);

        // Create monster
        Spawn();

        // Repeat
        StartCoroutine("MonsterSpawn");
    }

    IEnumerator RangedSpawn() {
        yield return new WaitForSeconds(30f);

        if(PlayerMovement.instance.score > rangedSpawnThreshold) {
            int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
            Transform randomSpawnPoint = spawnPoints[randomSpawnIndex];
            Instantiate(rangedBoss, randomSpawnPoint.position, Quaternion.identity);
        }

        StartCoroutine("RangedSpawn");
    }

    IEnumerator BossSpawn() {
        yield return new WaitForSeconds(5f);

        if(GameObject.FindGameObjectsWithTag("Boss").Length == 0) {
            int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
            Transform randomSpawnPoint = spawnPoints[randomSpawnIndex];
            Instantiate(boss, randomSpawnPoint.position, Quaternion.identity);
        }

        if(GameObject.FindGameObjectsWithTag("Pursuer").Length == 0) {
            int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
            Transform randomSpawnPoint = spawnPoints[randomSpawnIndex];
            Instantiate(pursuer, randomSpawnPoint.position, Quaternion.identity);
        }
    }
}

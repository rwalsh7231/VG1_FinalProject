using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGeneration : MonoBehaviour
{
    public float monsterDelay;
    public Transform[] spawnPoints;
    public GameObject littleMonster;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("MonsterSpawn");
    }

    // Update is called once per frame
    void Update()
    {
        
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
}

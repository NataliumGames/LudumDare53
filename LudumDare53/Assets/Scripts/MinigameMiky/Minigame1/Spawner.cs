using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] bonusPrefabs;
    public GameObject[] malusPrefabs;

    public float spawnRate = 1.0f;
    public float spawnY = 7.0f;

    public float leftBound = -8.0f;
    public float rightBound = 8.0f;

    public float maxGravity = -5.5f;
    public float minGravity = -4.0f;

    private float maxDistanceFromPrev;
    private float gravityRange;

    private int bonusCount = 0;

    private bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        maxDistanceFromPrev = (Mathf.Abs(leftBound) + Mathf.Abs(rightBound)) / 1.5f;
        gravityRange = maxGravity - minGravity;

        
        if (bonusPrefabs.Length == 0 && malusPrefabs.Length == 0)
        {
            Debug.Log("No prefabs loaded!");
            return;
        }
    }

    public void StartSpawner()
    {
        isRunning = true;
        StartCoroutine(SpawnObject());
    }

    public void StopSpawner()
    {
        isRunning = false;
    }

    IEnumerator SpawnObject()
    {
        float prevObjX = 0.0f;

        while (isRunning)
        {
            float spawnX, gravityY;
            GameObject go;

            gravityY = Random.Range(minGravity, maxGravity);
            //float tmp = (gravityY - minGravity) / gravityRange;
            spawnX = Random.Range(Mathf.Max(leftBound, prevObjX - maxDistanceFromPrev), Mathf.Min(rightBound, prevObjX + maxDistanceFromPrev));
            prevObjX = spawnX;
            Vector3 position = new Vector3(spawnX, spawnY, 0.0f);

            // spawn bonus or malus
            if(bonusCount < 3 || Random.Range(0, 2) == 1)
            {
                bonusCount++;
                go = Instantiate(bonusPrefabs[Random.Range(0, bonusPrefabs.Length)], transform);
            }
            else
            {
                bonusCount = 0;
                go = Instantiate(malusPrefabs[Random.Range(0, malusPrefabs.Length)], transform);
            }

            go.transform.position = position;
            go.transform.rotation = Quaternion.identity;
            go.transform.Rotate(new Vector3(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), 0.0f));
            go.GetComponent<FallingObject>().customGravity.y = gravityY;

            yield return new WaitForSeconds(1.0f / spawnRate);
        }
    }

    public int GetSpawnedObj()
    {
        return transform.childCount;
    }
}

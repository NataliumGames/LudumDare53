using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] prefabs;

    public float spawnRate = 1.0f;
    public float spawnY = 7.0f;

    public float leftBound = -8.0f;
    public float rightBound = 8.0f;

    public float maxGravity = -5.5f;
    public float minGravity = -4.0f;

    private float maxDistanceFromPrev;
    private float gravityRange;

    public bool game = true;

    // Start is called before the first frame update
    void Start()
    {
        maxDistanceFromPrev = (Mathf.Abs(leftBound) + Mathf.Abs(rightBound)) / 1.5f;
        gravityRange = maxGravity - minGravity;

        game = true;

        if (prefabs.Length == 0)
        {
            Debug.Log("No prefabs!");
            return;
        }
        StartCoroutine(SpawnObject());
    }

    IEnumerator SpawnObject()
    {
        float prevObjX = 0.0f;

        while (game)
        {
            float spawnX, gravityY;
            GameObject go;

            gravityY = Random.Range(minGravity, maxGravity);
            //float tmp = (gravityY - minGravity) / gravityRange;
            spawnX = Random.Range(Mathf.Max(leftBound, prevObjX - maxDistanceFromPrev), Mathf.Min(rightBound, prevObjX + maxDistanceFromPrev));
            prevObjX = spawnX;
            Vector3 position = new Vector3(spawnX, spawnY, 0.0f);

            go = Instantiate(prefabs[Random.Range(0, prefabs.Length)], position, Quaternion.identity);
            go.GetComponent<Obj>().customGravity.y = gravityY;

            yield return new WaitForSeconds(spawnRate);
        }
    }
}

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

    private float maxDiff;

    public bool game = true;

    // Start is called before the first frame update
    void Start()
    {
        maxDiff = (Mathf.Abs(leftBound) + Mathf.Abs(rightBound)) / 1.5f;

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
            float wanted = Random.Range(Mathf.Max(leftBound, prevObjX - maxDiff), Mathf.Min(rightBound, prevObjX + maxDiff));
            prevObjX = wanted;
            Vector3 position = new Vector3(wanted, spawnY, 0.0f);

            GameObject go = Instantiate(prefabs[Random.Range(0, prefabs.Length)], position, Quaternion.identity);
            go.GetComponent<Obj>().customGravity.y = Random.Range(minGravity, maxGravity);
            yield return new WaitForSeconds(spawnRate);
        }
    }
}

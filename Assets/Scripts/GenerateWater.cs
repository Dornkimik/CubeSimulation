using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateWater : MonoBehaviour
{
    public GameObject water;
    public List<Transform> waterSpawns = new List<Transform>();

    void Awake()
    {
        int waterIndex = Random.Range(0, waterSpawns.Count);

        Instantiate(water, waterSpawns[waterIndex].position, Quaternion.identity);
    }
}

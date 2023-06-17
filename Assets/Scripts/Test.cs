using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Baby Cube Test

    private Vector3 grownSize = new Vector3(0.708299994f, 0.708299994f, 0.708299994f);

    private float growSize = 0.05f;

    private void Start()
    {
        transform.localScale = new Vector3(0f, 0f, 0f);
    }
    private void Update()
    {
        if (transform.localScale.x < grownSize.x)
        {
            transform.localScale += new Vector3(growSize, growSize, growSize) * Time.deltaTime;
        }

    }
}
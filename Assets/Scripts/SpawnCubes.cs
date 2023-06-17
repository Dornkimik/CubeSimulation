using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCubes : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private GameObject preplacedPrefab;

    private bool isInBorderX;
    private bool isInBorderY;

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x > -10.04f && mousePos.x < 10f)
        {
            isInBorderX = true;
        } else
        {
            isInBorderX = false;
        }

        if (mousePos.y > -4.25f && mousePos.y < 4.3f)
        {
            isInBorderY = true;
        } else
        {
            isInBorderY = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            preplacedPrefab.SetActive(true);
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (isInBorderX && isInBorderY)
            {
                Instantiate(cubePrefab, mousePos, Quaternion.identity);
                preplacedPrefab.SetActive(false);
            }
            else
            {
                print("not inside border");
                preplacedPrefab.SetActive(false);
            }
        }

        preplacedPrefab.transform.position = mousePos;
    }
}

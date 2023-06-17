using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int speedTime;

    [SerializeField] Slider speedSlider;

    private void Start()
    {
        speedTime = 1;
    }

    private void Update()
    {
        Time.timeScale = speedTime;
        Controls();
    }

    public void ChangeSpeed()
    {
        speedTime = (int) speedSlider.value;
    }

    void Controls()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}

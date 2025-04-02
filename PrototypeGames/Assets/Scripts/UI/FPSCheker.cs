using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCheker : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;
    private float _fps;
    
    private void Start()
    {
        StartCoroutine(CheckFPS());
    }

    private IEnumerator CheckFPS()
    {
        while (true)
        {
            _fps = 1f / Time.deltaTime;
            DisplayFPS(_fps);

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void DisplayFPS(float fps)
    {
        fpsText.text = $"FPS: {(int)fps}";
    }
}

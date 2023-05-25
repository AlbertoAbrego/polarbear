using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderHUD : MonoBehaviour
{
    public Slider slider;

    public void StartOrder()
    {
        StartCoroutine(Preparing());
    }

    IEnumerator Preparing()
    {
        float progress = 1f;
        while (progress > 0)
        {
            slider.value = progress;
            progress -= Time.deltaTime / 10;
            yield return null;
        }
    }

    public void UpdateHUD(float time)
    {
        slider.value -= time;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testing : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        StartCoroutine(barra());
    }

    IEnumerator barra()
    {
        float valor = 1f;
        while (valor > 0)
        {
            slider.value = valor;
        Debug.Log(valor);
            valor -= Time.deltaTime/10;
            yield return null;
        }

    }
}

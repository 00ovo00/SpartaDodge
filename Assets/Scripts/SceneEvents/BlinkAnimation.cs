using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkAnimation : MonoBehaviour
{
    float time;
    private Image imageComponent;

    void Start()
    {
        imageComponent = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (time < 0.5f)
        {
            imageComponent.color = new Color(1, 1, 1, 1 - time);
        }
        else
        {
            imageComponent.color = new Color(1, 1, 1, time);
            if (time > 1f)
            {
                time = 0;
            }
        }

        time += Time.deltaTime;

    }
}

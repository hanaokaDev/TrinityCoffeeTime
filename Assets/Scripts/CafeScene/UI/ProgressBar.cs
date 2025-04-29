using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;

    public Gradient gradient;
    public Image fillImage;

    public void SetMaxValue(float maxValue)
    {
        slider.maxValue = maxValue;
        fillImage.color = gradient.Evaluate(1f); // Fill color set to max value color
    }

    public void SetValue(float value)
    {
        slider.value = value;
        fillImage.color = gradient.Evaluate(slider.normalizedValue); // Fill color based on current value
    }
    public float GetValue()
    {
        return slider.value;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSlider : MonoBehaviour
{
    [SerializeField] string sliderName;
    Slider slider;
    
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = LevelManager.instance.GetSliderValue(sliderName);
    }

    public void OnValueChanged()
    {
        LevelManager.instance.ChangeSliderValue(sliderName, slider.value);
    }
}

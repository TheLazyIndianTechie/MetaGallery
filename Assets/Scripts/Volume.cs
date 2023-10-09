using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Volume : MonoBehaviour
{
    public AudioSource source;
    public Slider slider;
    // Start is called before the first frame update
    void Awake()
    {
        slider.value = source.volume;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeVolume()
    {
        source.volume = slider.value;
    }
}

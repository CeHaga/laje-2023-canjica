using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GlobalController : MonoBehaviour
{

    public static string dificuldade;
    public float volume;

    public Slider mainSlider;
    public AudioMixer audioo;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        //Adds a listener to the main slider and invokes a method when the value changes.
        if(mainSlider != null)
            mainSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck()
    {
        volume = mainSlider.value;
        audioo.SetFloat("volume", mainSlider.value);
    }

}

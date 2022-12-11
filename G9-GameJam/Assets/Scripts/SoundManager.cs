using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    private static readonly string firstPlay = "FirstPlay";
    private static readonly string musicPref = "MusicPref";

    private int _firstPlayInt;
    private float _musicFloat;

    [SerializeField] private Slider musicSlider;    
    [SerializeField] private AudioSource musicAudio;
    [SerializeField] private AudioClip buttonSound;

    void Start()
    {
        _firstPlayInt = PlayerPrefs.GetInt(firstPlay);

        if (_firstPlayInt == 0)
        {
            _musicFloat = 0.5f;
            musicSlider.value = _musicFloat;
            PlayerPrefs.SetFloat(musicPref, _musicFloat);
            PlayerPrefs.SetInt(firstPlay, -1);
        }
        else
        {
            _musicFloat = PlayerPrefs.GetFloat(musicPref);
            musicSlider.value = _musicFloat;
        }
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(musicPref, musicSlider.value);
    }

    void OnApplicationFocus(bool inFocus)
    {
        if (!inFocus)
        {
            SaveSoundSettings();
        }
    }

    public void UpdateSound()
    {
        musicAudio.volume = musicSlider.value;
    }

    public void ButtonSound()
    {
        musicAudio.PlayOneShot(buttonSound);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    private static readonly string firstPlay = "FirstPlay";
    private static readonly string musicPref = "MusicPref";

    private int _firstPlayInt;
    private float _musicFloat;

    private Slider musicSlider;    
    [SerializeField] private AudioSource musicAudio;
    [SerializeField] private AudioClip buttonSound;

    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);


        musicSlider = FindObjectOfType<Slider>(true);
        if(musicSlider != null)
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
        else
        {
            _musicFloat = PlayerPrefs.GetFloat(musicPref);
            UpdateSound();
        }
    }

    public void SaveSoundSettings()
    {
        _musicFloat = musicSlider.value;
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
        if (musicSlider != null)
            _musicFloat = musicSlider.value;
        musicAudio.volume = _musicFloat;
    }

    public void ButtonSound()
    {
        musicAudio.PlayOneShot(buttonSound);
    }
}

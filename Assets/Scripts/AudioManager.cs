using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
/*    public static AudioManager Instance;
*/
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    private float sfxVolume;
    private float musicVolume;

    public Sound[] musicSounds;
    public AudioSource musicSource;

    private void Start()
    {
/*        Instance = this;*/

        Debug.Log("set volume");

        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1);

        //setting volume when new scene loaded
        SetSFXVolume(sfxVolume);
        SetMusicVolume(musicVolume);

        //set value on sliders
        sfxSlider.value = sfxVolume;
        musicSlider.value = musicVolume;

        Debug.Log(sfxVolume);
        Debug.Log(musicVolume);
    }

    //set audio mixer volume
    public void SetSFXVolume(float volume)
    {
        if (volume <= Mathf.Pow(10, -80))
        {
            audioMixer.SetFloat("SFX", -80f);
            PlayerPrefs.SetFloat("SFXVolume", -80f);
            return;
        }

        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);

        Debug.Log(volume);
    }

    public void SetMusicVolume(float volume)
    {
        if (volume <= Mathf.Pow(10, -80))
        {
            audioMixer.SetFloat("Music", -80f);
            PlayerPrefs.SetFloat("MusicVolume", -80f);
            return;
        }

        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);

        Debug.Log(volume);
    }
}
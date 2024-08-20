using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] private bool gameOverScreen;
    [SerializeField] private TextMeshProUGUI gameOverInfo;

    //navigation
    public void Play()
    {
        if (gameOverScreen)
        {
            PlayerPrefs.SetInt("score", 1);
            PlayerPrefs.SetInt("enemiesDefeated", 0);
            PlayerPrefs.SetFloat("sizeScore", 4);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Tutorial()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void Quit()
    {
        Application.Quit();
    }

    //graphics
    Resolution[] resolutions;
    [SerializeField] private TMPro.TMP_Dropdown resolutionDropdown;

    public void SetQuality (int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void ToggleFullscreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    void Start()
    {
        if (resolutionDropdown != null)
        {
            resolutions = Screen.resolutions;

            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            int currentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }
        else
        {
            Debug.LogWarning("No Resolution Dropdown Found");
        }

        if (gameOverScreen)
        {
            if (PlayerPrefs.GetFloat("score", 0) > PlayerPrefs.GetFloat("highScore", 0))
            {
                PlayerPrefs.SetFloat("highScore", PlayerPrefs.GetFloat("score", 0));
            }
            gameOverInfo.text = "Wave: " + PlayerPrefs.GetInt("score", 0) + "\nEnemies Defeated: " + PlayerPrefs.GetInt("enemiesDefeated", 0) + "\nTop Mass: " + PlayerPrefs.GetFloat("sizeScore", 0);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour {

    private GameObject[] allMenuPanels;

    public float musicVolume;

    public void MusicVolume(float myMusicVolume)
    {
        musicVolume = myMusicVolume;
    }

    public float soundFXVolume;

    public void SoundFXVolume(float mySoundFXVolume)
    {
        soundFXVolume = mySoundFXVolume;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ChangeToScene(int sceneToChangeTo)
    {
        SceneManager.LoadScene(sceneToChangeTo);
    }

    public void SwitchPanel(GameObject destinationPanel)
    {
        Debug.Log(destinationPanel.ToString());

        allMenuPanels = GameObject.FindGameObjectsWithTag("MenuPanel");
        foreach (GameObject panel in allMenuPanels)
        {
            panel.SetActive(false);
        }
        destinationPanel.SetActive(true);
    }

    public string VolumeConversionToString(float volumeFloat)
    {
        float volumeFloatToConvert = volumeFloat * 100;
        string volumeString = volumeFloatToConvert.ToString();
        return volumeString;
    }

    public float VolumeConversionToFloat(string volumeString)
    {
        float volumeFloat = float.Parse(volumeString);
        float volumeFloatConverted = volumeFloat / 100;
        return volumeFloatConverted;
    }
}

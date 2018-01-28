using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    private GameObject[] allMenuPanels;

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
}

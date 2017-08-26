using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    public GameObject MainMenuPanel;

    void Start ()
    {
        MainMenuPanel.GetComponentInChildren<ButtonController>().SwitchPanel(MainMenuPanel);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button[] buttons;

    private void Awake()
    {
        int unlockedStage = PlayerPrefs.GetInt("UnlockedStage", 1);
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for(int i = 0; i < unlockedStage; i++)
        {
            buttons[i].interactable = true;
        }
    }
    public void OpenStage(int Stageid)
    {
        string StageName = "Stage " + Stageid;
        SceneManager.LoadScene(StageName);
    }
}

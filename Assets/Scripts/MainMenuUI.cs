using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

    public GameObject MainButtons;
    public GameObject LimitButtons;
    public GameObject DeadlineButtons;
    public GameObject PurchasePanel;
    public GameObject Settings;
    public GameObject SoundButton;
    public List<Button> R_Buttons;
    public List<Button> H_Buttons;

    void Start()
    {
        if (GameManager.Instance.AdsDisabled)
        {
            PurchasePanel.GetComponent<CanvasGroup>().interactable = false;
        }

        if(GameManager.Instance.BG.mute == true)
        {
            SoundButton.GetComponent<Image>().color = GameManager.Instance.red;
        }
        for(int i = 0; i < R_Buttons.Count; i++)
        {
            if(i > GameManager.Instance.R_LevelsUnlocked)
            {
                R_Buttons[i].interactable = false;
            }
        }
        for (int i = 0; i < H_Buttons.Count; i++)
        {
            if (i > GameManager.Instance.H_LevelsUnlocked)
            {
                H_Buttons[i].interactable = false;
            }
        }
    }

    public void OpenLimit()
    {
        GameManager.Instance.MainMenuUI(LimitButtons, MainButtons, true);
    }

    public void OpenDeadline()
    {
        GameManager.Instance.MainMenuUI(DeadlineButtons, MainButtons, true);
    }

    public void Close(GameObject ObjectBeingClosed)
    {
        GameManager.Instance.MainMenuUI(MainButtons, ObjectBeingClosed, false);
    }

    public void SettingsOpen()
    {
        GameManager.Instance.MainMenuUI(Settings, MainButtons, true);
    }

    public void LoadLevel(string i)
    {
        GameManager.Instance.LoadLevel(i);
    }

    public void Sound()
    {
        GameManager.Instance.Sound(SoundButton);
    }

}

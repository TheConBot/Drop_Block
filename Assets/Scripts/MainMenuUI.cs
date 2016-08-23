using UnityEngine;
using System.Collections;

public class MainMenuUI : MonoBehaviour {

    public GameObject MainButtons;
    public GameObject LimitButtons;
    public GameObject DeadlineButtons;

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

    public void LoadLevel(string i)
    {
        GameManager.Instance.LoadLevel(i);
    }

}

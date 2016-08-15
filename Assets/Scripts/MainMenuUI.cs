using UnityEngine;
using System.Collections;

public class MainMenuUI : MonoBehaviour {

    public GameObject MasterButtons;

	public void OpenMMButtons(GameObject buttons)
    {
        GameManager.Instance.OpenMMButtons(MasterButtons, buttons);
    }

    public void CloseMMButtons(GameObject buttons)
    {
        GameManager.Instance.CloseMMButtons(MasterButtons, buttons);
    }

    public void LoadLevel(int i)
    {
        GameManager.Instance.LoadLevel(i);
    }

}

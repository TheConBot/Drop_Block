using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelChecker : MonoBehaviour
{

    public List<Button> buttons;

    // Use this for initialization
    void Start()
    {

        for (int i = 0; i < GameManager.Instance.levelsUnlocked; i++)
        {
            buttons[i].interactable = true;
        }
    }

    public void LoadLevel(int i)
    {
        GameManager.Instance.LoadLevel(i);
    }
}

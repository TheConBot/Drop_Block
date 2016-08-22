using UnityEngine;
using System.Collections;

public class MainMenuUI : MonoBehaviour {
    [Header("UI Panels")]
    public GameObject FiniteButtons;
    public GameObject LifeButtons;
    public GameObject Life;
    public GameObject Death;

    void Awake()
    {
        FiniteButtons.SetActive(true);
        LifeButtons.SetActive(false);
        Life.SetActive(false);
        Death.SetActive(false);
    }

	public void OpenLifeButtons()
    {
        GameManager.Instance.MainMenuUI(LifeButtons, FiniteButtons, true);
    }

    public void CloseLifeButtons()
    {
        GameManager.Instance.MainMenuUI(FiniteButtons, LifeButtons, false);
    }

    public void OpenLife()
    {
        GameManager.Instance.MainMenuUI(Life, LifeButtons, true);
    }

    public void CloseLife()
    {
        GameManager.Instance.MainMenuUI(LifeButtons, Life, false);
    }

    public void OpenDeath()
    {
        GameManager.Instance.MainMenuUI(Death, LifeButtons, true);
    }

    public void CloseDeath()
    {
        GameManager.Instance.MainMenuUI(LifeButtons, Death, false);
    }


    public void LoadLevel(string i)
    {
        GameManager.Instance.LoadLevel(i);
    }

}

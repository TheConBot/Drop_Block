using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour {

    public GameObject PausePanel;


	public void PauseGame()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
    }

    public void MainMenu()
    {
        GameManager.Instance.LoadLevel("_MainMenu");
    }

    public void Restart()
    {
        GameManager.Instance.LoadLevel(-1);
    }

    public void NextLevel()
    {
        GameManager.Instance.NextLevel();
    }
}

using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour {

    public GameObject PausePanel;
    public Text Count;

    private string countSave;

	public void PauseGame()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
        countSave = Count.text;
        Count.text = "PAUSED";
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        Count.text = countSave;
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

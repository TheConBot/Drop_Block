using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class StartLevel : MonoBehaviour {

    public enum GameMode {Regular, Hard, Endless };
    public GameMode gameMode;
    public Vector2 dropBlockSpawn;
    public GameObject[] startBlocks;
    public List<GameObject> regBlocks;
    public Text UI_count;
    public GameObject gm;
    public GameObject gameOverPanel;

    void Awake()
    {
        //Spawns a gamemanager if there is none. This is editor only for level testing.
        if (Application.isEditor && GameObject.Find("GameManager") == null)
        {
            GameObject GameManager = Instantiate(gm);
            //Only doing this to get rid of the stupid "value is assigned but not used" warning
            GameManager.name = "GameManager";
        }
    }

    void Start () {
        if (gameMode == GameMode.Regular) { }
        if (gameMode == GameMode.Hard) { GameManager.Instance.StartLevelHard(startBlocks, regBlocks, dropBlockSpawn, UI_count, gameOverPanel); }
        if (gameMode == GameMode.Endless) { GameManager.Instance.StartLevelEndless(regBlocks[0], dropBlockSpawn, UI_count, gameOverPanel); }
	}
}

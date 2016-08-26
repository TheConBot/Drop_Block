using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class StartLevel : MonoBehaviour {
    public enum GameMode {Regular, Hard, Endless };
    [Header("Game Mode")]
    public GameMode gameMode;
    [Header("Block Spawn Position")]
    public Vector2 dropBlockSpawn;
    [Header("Block Lists")]
    public GameObject[] startBlocks;
    public List<GameObject> regBlocks;
    [Header("UI")]
    public Text UI_count;
    public Text blockCount;
    public GameObject gameOverPanel;
    [Header("Speed Increase Per Block")]
    public float speedMod = 0.15f;
    [Header("GameManager(Editor Only)")]
    public GameObject gm;
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
        if (gameMode == GameMode.Regular) { GameManager.Instance.StartLevelRegular(startBlocks, regBlocks, dropBlockSpawn, UI_count, gameOverPanel, blockCount, speedMod); }
        if (gameMode == GameMode.Hard) { GameManager.Instance.StartLevelHard(startBlocks, regBlocks, dropBlockSpawn, UI_count, gameOverPanel, speedMod); }
        if (gameMode == GameMode.Endless) { GameManager.Instance.StartLevelEndless(regBlocks[0], dropBlockSpawn, UI_count, gameOverPanel, speedMod); }
	}
}

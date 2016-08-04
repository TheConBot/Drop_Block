 using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class StartLevel : MonoBehaviour {

    public Vector2 dropBlockSpawn;
    public GameObject[] startBlocks;
    public List<GameObject> regBlocks;
    public Text UI_count;
    public GameObject gm;

    void Awake()
    {
        if (Application.isEditor && GameObject.Find("GameManager") == null)
        {
            GameObject GameManager = Instantiate(gm);
        }
    }

    void Start () {
        GameManager.Instance.StartLevel(startBlocks, regBlocks, dropBlockSpawn, UI_count);
	}
}

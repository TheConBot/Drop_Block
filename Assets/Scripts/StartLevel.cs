 using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartLevel : MonoBehaviour {

    public Vector2 dropBlockSpawn;
    public GameObject[] startBlocks;
    public Text UI_count;

	void Start () {
        GameManager.Instance.StartLevel(startBlocks, dropBlockSpawn, UI_count);
	}
}

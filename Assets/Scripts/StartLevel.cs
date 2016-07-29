using UnityEngine;
using System.Collections;

public class StartLevel : MonoBehaviour {

    public Vector2 dropBlockSpawn;
    public GameObject[] startBlocks;

	void Start () {
        GameManager.Instance.StartLevel(startBlocks, dropBlockSpawn);
	}
}

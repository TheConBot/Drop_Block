using UnityEngine;
using System.Collections;

public class MainMenuSpawner : MonoBehaviour {

    public GameObject blockNG;
	// Use this for initialization
	void Start () {
        StartCoroutine(SpawnBlocks());
	}

    IEnumerator SpawnBlocks()
    {
        while (true)
        {
            GameObject Block = (GameObject)Instantiate(blockNG, new Vector3(Random.Range(-7.25f, 7.25f), 6), Quaternion.identity);
            float newScale = Random.Range(0.3f, 1.0f);
            Block.transform.localScale = new Vector3(newScale, newScale);
            Block.GetComponent<Rigidbody2D>().gravityScale = newScale;
            yield return new WaitForSeconds(0.25f);
        }
    }
}

using UnityEngine;
using System.Collections;

public class TimedDestroy : MonoBehaviour {

    public float waitTime;
	// Use this for initialization
	void Start () {
        GameManager.Instance.SpawnMainMenuBlock(gameObject);
        StartCoroutine(DestroyBlock());
	}
	
	// Update is called once per frame
	IEnumerator DestroyBlock()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            GameManager.Instance.SpawnMainMenuBlock(gameObject);
        }
    }
}

using UnityEngine;
using System.Collections;

public class TimedDestroy : MonoBehaviour {

    public float waitTime;
	// Use this for initialization
	void Start () {
        StartCoroutine(DestroyBlock());
	}
	
	// Update is called once per frame
	IEnumerator DestroyBlock()
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}

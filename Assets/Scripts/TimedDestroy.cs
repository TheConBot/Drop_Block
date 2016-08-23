using UnityEngine;
using System.Collections;

public class TimedDestroy : MonoBehaviour {

	void Start () {
        GameManager.Instance.SpawnMainMenuBlock(gameObject);
	}
	
	void Update()
    {
        if(transform.position.y <= -5.5f)
        {
            GameManager.Instance.SpawnMainMenuBlock(gameObject);
        }
    }
}

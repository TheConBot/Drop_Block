using UnityEngine;
using System.Collections;

public class CameraLerp : MonoBehaviour {

    public float moveSpeed;
	// Update is called once per frame
	void Update () {
        if (GameManager.Instance.moveCam)
        {
            gameObject.transform.position = Vector3.Lerp(transform.position, GameManager.Instance.newCamPos, Time.deltaTime * moveSpeed);
        }

        if((transform.position.y + 0.05f) >= GameManager.Instance.newCamPos.y)
        {
            GameManager.Instance.moveCam = false;
        }
	}
}

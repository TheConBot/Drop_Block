using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DropBlockInput : MonoBehaviour {

    private bool getInputOnce = true;
	
	void Update () {
        if (Application.isMobilePlatform)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && getInputOnce && !EventSystem.current.IsPointerOverGameObject((Input.GetTouch(0).fingerId)))
            {
                gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                gameObject.GetComponent<MoveBlock>().move = false;
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                getInputOnce = false;
                GameManager.Instance.isActiveMovingBlock = false;
                enabled = false;
            }
        }
        else {
            if (Input.anyKeyDown && getInputOnce && !EventSystem.current.IsPointerOverGameObject())
            {
                gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                gameObject.GetComponent<MoveBlock>().move = false;
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                getInputOnce = false;
                GameManager.Instance.isActiveMovingBlock = false;
                enabled = false;
            }
        }

    }
}

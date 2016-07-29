using UnityEngine;
using System.Collections;

public class DropBlock : MonoBehaviour {
    
    private bool getInputOnce;
    private bool spawnBlockOnce;
    private bool inGoal;

    void Start()
    {
        getInputOnce = true;
        spawnBlockOnce = true;
    }

    void Update()
    {
        if (Input.anyKeyDown && getInputOnce)
        {
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            gameObject.GetComponent<MoveBlock>().move = false;
            getInputOnce = false;
        }
    }

	void FixedUpdate () {
        if (gameObject.GetComponent<Rigidbody2D>().velocity.magnitude <= 0.01 && transform.position.y < gameObject.GetComponent<MoveBlock>().startPos.y && spawnBlockOnce)
        {
            if (inGoal)
            {
                Debug.Log("You win!");
                GameManager.Instance.blockColorChange(2, 1);
                spawnBlockOnce = false;
            }
            else
            {
                GameManager.Instance.spawnDropBlock();
                GameManager.Instance.blockColorChange(3, 2);
                spawnBlockOnce = false;
            }
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Goal_Block") inGoal = true;
        else if(other.name == "Death_Block")
        {
            GameManager.Instance.LoadLevel(-1);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Goal_Block") inGoal = false;
    }
}

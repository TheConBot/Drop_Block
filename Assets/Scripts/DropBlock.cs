using UnityEngine;
using System.Collections;

public class DropBlock : MonoBehaviour
{

    private bool getInputOnce;
    private bool spawnBlockOnce;
    private bool inGoal;
    private bool touchingBlock;
    private GameObject objectTouched;

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

    void FixedUpdate()
    {
        if (gameObject.GetComponent<Rigidbody2D>().velocity.magnitude <= 0.01 && transform.position.y < gameObject.GetComponent<MoveBlock>().startPos.y && spawnBlockOnce && touchingBlock)
        {
            if (GameManager.Instance.isRowComplete(objectTouched))
            {
                GameManager.Instance.SpawnDropBlock();
                spawnBlockOnce = false;
            }
            else {
                gameObject.tag = objectTouched.tag;
                GameManager.Instance.BlockColorChange(objectTouched, gameObject);
                GameManager.Instance.CheckWin(inGoal, gameObject);
                spawnBlockOnce = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Goal_Block") inGoal = true;
        else if (other.name == "Death_Block")
        {
            GameManager.Instance.LoadLevel(-1);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Goal_Block") inGoal = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        touchingBlock = true;
        objectTouched = other.gameObject;
    }

    void OnCollisionExit2D(Collision2D other)
    {
        touchingBlock = false;
        objectTouched = null;
    }
}

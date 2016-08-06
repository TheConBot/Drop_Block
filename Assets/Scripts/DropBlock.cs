using UnityEngine;
using System.Collections;

public class DropBlock : MonoBehaviour
{
    public enum TypeOfBlock {Drop_Block, Start_Block};
    public TypeOfBlock blockType;
    private bool getInputOnce;
    private bool spawnBlockOnce;
    private bool inGoal;
    private bool touchingBlock;
    private GameObject objectTouched;
    public bool currentlyGold = false;

    void Start()
    {
        if (blockType == TypeOfBlock.Drop_Block)
        {
            getInputOnce = true;
            spawnBlockOnce = true;
        }
    }

    void Update()
    {
        if (blockType == TypeOfBlock.Drop_Block)
        {
            if (Input.anyKeyDown && getInputOnce)
            {
                gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                gameObject.GetComponent<MoveBlock>().move = false;
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                getInputOnce = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (blockType == TypeOfBlock.Drop_Block)
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
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (blockType == TypeOfBlock.Drop_Block)
        {
            if (other.name == "Goal_Block") inGoal = true;
            else if (other.name == "Death_Block") GameManager.Instance.LoadLevel(-1);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (blockType == TypeOfBlock.Drop_Block)
        {
            if (other.name == "Goal_Block") inGoal = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (blockType == TypeOfBlock.Drop_Block)
        {
            if (other.gameObject.tag.StartsWith("S") && blockType == TypeOfBlock.Drop_Block && other.gameObject.GetComponent<DropBlock>().currentlyGold)
            {
                touchingBlock = true;
                objectTouched = other.gameObject;
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (blockType == TypeOfBlock.Drop_Block)
        {
            if (other.gameObject.tag.StartsWith("S") && blockType == TypeOfBlock.Drop_Block && other.gameObject.GetComponent<DropBlock>().currentlyGold)
            {
                touchingBlock = false;
                objectTouched = null;
            }
        }
    }
}

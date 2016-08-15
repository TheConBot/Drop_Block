using UnityEngine;
using UnityEngine.EventSystems;

public class DropBlock : MonoBehaviour
{
    public enum TypeOfBlock { Drop_Block, Start_Block };
    public TypeOfBlock      blockType;
    private bool            getInputOnce;
    private bool            spawnBlockOnce;
    private bool            inGoal;
    private bool            touchingBlock;
    private GameObject      objectTouched;
    public bool             currentlyGold;

    void Start()
    {
        //Only once bools
        if (blockType == TypeOfBlock.Drop_Block)
        {
            getInputOnce = true;
            spawnBlockOnce = true;
        }
    }

    void Update()
    {
        //Getting the input and dropping the block
        if (blockType == TypeOfBlock.Drop_Block)
        {
            if (Application.isMobilePlatform)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began && getInputOnce && !EventSystem.current.IsPointerOverGameObject((Input.GetTouch(0).fingerId)))
                {
                    gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                    gameObject.GetComponent<MoveBlock>().move = false;
                    gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    getInputOnce = false;
                }
            }
            else {
                if (Input.anyKeyDown && getInputOnce && !EventSystem.current.IsPointerOverGameObject())
                {
                    gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                    gameObject.GetComponent<MoveBlock>().move = false;
                    gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    getInputOnce = false;
                }
            }
        }
    }

    void FixedUpdate()
    {
        //If the block is not moving, is below its spawn point, and is touching a gold block it adds that block to the appropriate collumn, changes the color, and checks the game state.
        if (blockType == TypeOfBlock.Drop_Block)
        {
            if (gameObject.GetComponent<Rigidbody2D>().velocity.magnitude <= 0.01 && transform.position.y < gameObject.GetComponent<MoveBlock>().startPos.y && spawnBlockOnce && touchingBlock)
            {
                gameObject.tag = objectTouched.tag;
                GameManager.Instance.BlockColorChange(objectTouched, gameObject);
                GameManager.Instance.CheckWin(inGoal, gameObject);
                spawnBlockOnce = false;
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
}

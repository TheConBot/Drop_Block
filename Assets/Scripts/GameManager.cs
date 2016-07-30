using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject dropBlock;
    public List<GameObject> blocks;
    public List<string> completedCollums;
    public Color gold;
    public static GameManager Instance { get; private set; }

    private Vector2 dropBlockSpawn;
    private int currentLevel = 0;
    private Text goalCount;
    private int winCondition;
    private int goalsGot;

    // Use this for initialization
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }
        else {

            // Here we save our singleton instance
            Instance = this;

            // Furthermore we make sure that we don't destroy between scenes (this is optional)
            DontDestroyOnLoad(gameObject);
        }
    }

    public void BlockColorChange(GameObject toWhite, GameObject toGold)
    {
        SpriteRenderer[] square = toWhite.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in square)
        {
            sprite.color = Color.white;
        }
        //Set newly dropped block to gold
        square = toGold.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in square)
        {
            sprite.color = gold;
        }
    }

    public void SpawnDropBlock()
    {
        GameObject Block = (GameObject)Instantiate(dropBlock, dropBlockSpawn, Quaternion.identity);
        Block.name = "Drop_Blocks_" + (blocks.Count - 1);
        blocks.Add(Block);
    }

    public void StartLevel(GameObject[] startBlocks, Vector2 spawn, Text b)
    {
        dropBlockSpawn = spawn;
        foreach (GameObject block in startBlocks)
        {
            blocks.Add(block);
            winCondition++;
        }
        SpawnDropBlock();
        goalCount = b;
        goalCount.text = "0/" + winCondition;
    }

    public void LoadLevel(int i)
    {
        if (i == -1) SceneManager.LoadScene(currentLevel);
        else {
            SceneManager.LoadScene(i);
            currentLevel = i;
        }
        ClearVariables();
    }

    public void CheckWin(bool inGoal, GameObject block)
    {
        if (inGoal)
        {
            goalsGot++;
            goalCount.text = goalsGot + "/" + winCondition;
            completedCollums.Add(block.tag);
            foreach(GameObject i in blocks)
            {
                if (i.tag == block.tag)
                {
                    i.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                    i.GetComponent <Collider2D>().enabled = false;
                    SpriteRenderer[] sprites = i.GetComponentsInChildren<SpriteRenderer>();
                    foreach(SpriteRenderer square in sprites)
                    {
                        square.color = Color.green;
                    }
                }
            }
        }
         
        if (goalsGot == winCondition)
        {
            Debug.Log("You Win!");
        }
        else
        {
            SpawnDropBlock();
        }
    }

    public bool isRowComplete(GameObject block)
    {
        foreach(string tag in completedCollums)
        {
            if (block.tag == tag) return true;
        }
        return false;
    }

    private void ClearVariables()
    {
        winCondition = 0;
        goalsGot = 0;
        blocks.Clear();
        completedCollums.Clear();
        dropBlockSpawn = Vector2.zero;
    }
}

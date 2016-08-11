using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Lists of Data")]
    public List<GameObject> blocks;
    public List<string> completedCollums;
    [Header("Block Colors")]
    public Color gold;
    public Color green;
    public Color white;
    [Header("Variable Fields")]
    public float speedMod;
    public int levelsUnlocked;
    [Header("Sounds")]
    public AudioSource Confirm;
    public AudioSource Back;
    public AudioSource Ding;

    public static GameManager Instance { get; private set; }

    private Vector2 dropBlockSpawn;
    private int currentLevel = 0;
    private Text goalCount;
    private int winCondition;
    private int goalsGot;
    private int currentBlock;
    private bool wonLevel;

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
            if (Application.isEditor) { currentLevel = SceneManager.GetActiveScene().buildIndex; }
            // Furthermore we make sure that we don't destroy between scenes (this is optional)
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update()
    {
        if(wonLevel && Input.anyKeyDown)
        {
            levelsUnlocked++;
            //REMOVE THIS LATER!!!!!
            if (levelsUnlocked > 5) levelsUnlocked = 5;
            LoadLevel(0);
        }
    }

    public void BlockColorChange(GameObject toWhite, GameObject toGold)
    {
        Ding.Play();
        toWhite.GetComponent<DropBlock>().currentlyGold = false;
        toGold.GetComponent<DropBlock>().currentlyGold = true;
        SpriteRenderer[] square = toWhite.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in square)
        {
            sprite.color = white;
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
        blocks[currentBlock].SetActive(true);
        float dist = blocks[currentBlock].GetComponent<MoveBlock>().distance;
        dist = Random.Range((dropBlockSpawn.x - dist), (dropBlockSpawn.x + dist));
        Vector2 finalSpawn = new Vector2(dist, dropBlockSpawn.y);
        blocks[currentBlock].GetComponent<MoveBlock>().SetPosition(dropBlockSpawn);
        blocks[currentBlock].transform.position = finalSpawn;
        currentBlock++;
    }

    public void SpawnMainMenuBlock(GameObject blockNG)
    {
        float newScale = Random.Range(0.3f, 1.0f);
        blockNG.transform.localScale = new Vector3(newScale, newScale);
        blockNG.GetComponent<Rigidbody2D>().gravityScale = newScale;
        blockNG.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        blockNG.transform.position = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(5.25f, 9.25f));
        blockNG.GetComponent<TimedDestroy>().waitTime = (blockNG.transform.position.y / 3f) + (1 - newScale);
    }

    public void StartLevel(GameObject[] startBlocks, List<GameObject> regBlocks, Vector2 spawn, Text b)
    {
        dropBlockSpawn = spawn;
        foreach (GameObject block in startBlocks)
        {
            blocks.Add(block);
            winCondition++;
        }
        currentBlock = winCondition;
        float currentSpeedMod = 0;
        foreach(GameObject block in regBlocks)
        {
            float blockSpeed = block.GetComponent<MoveBlock>().speed;
            blockSpeed = blockSpeed + currentSpeedMod;
            currentSpeedMod = speedMod + currentSpeedMod;
            block.GetComponent<MoveBlock>().speed = blockSpeed;
            blocks.Add(block);
            block.SetActive(false);
        }
        SpawnDropBlock();
        goalCount = b;
        goalCount.text = "0/" + winCondition;
    }

    public void LoadLevel(int i)
    {
        if (i == -1)
        {
            Back.Play();
            SceneManager.LoadScene(currentLevel);
        }
        else {
            Confirm.Play();
            SceneManager.LoadScene(i);
            currentLevel = i;
        }
        ClearVariables();
        Time.timeScale = 1;
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
                        square.color = green;
                    }
                }
            }
        }
         
        if (goalsGot == winCondition)
        {
            wonLevel = true;
            if (Application.isMobilePlatform)
            {
                goalCount.text = "You Win! Tap anywhere to continue!";
            }
            else {
                goalCount.text = "You Win! Press any button to continue!";
            }
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
        currentBlock = 0;
        wonLevel = false;
    }
}

using UnityEngine;
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
    private int currentGameMode;
    private GameObject gameOverScreen;
    private int endlessHighScore;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // Destroy if another Gamemanager already exists
            Destroy(gameObject);
        }
        else {

            // Here we save our singleton instance
            Instance = this;
            //In editor we make sure the game knows what scene it is in for level loading
            if (Application.isEditor) { currentLevel = SceneManager.GetActiveScene().buildIndex; }
            // Furthermore we make sure that we don't destroy between scenes
            DontDestroyOnLoad(gameObject);
            endlessHighScore = PlayerPrefs.GetInt("endlessHighScore");
        }
    }

    void Update()
    {
        //After you win a level it takes you back to the main menu after touching/clicking
        if (wonLevel && Input.anyKeyDown)
        {
            LoadLevel(0);
        }
    }

    //Provides the climbing gold block effect and plays a sound
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

    //Checks the state of the level after each block is dropped.
    public void CheckWin(bool inGoal, GameObject block)
    {
        if (currentGameMode == 2)
        {
            SpawnDropBlock(blocks[0]);
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 0.46f, Camera.main.transform.position.z);
        }
        else {
            //Checks to see if you made it into the goal with a stack. If so, turns all the blocks green and makes them immovable
            if (inGoal)
            {
                goalsGot++;
                goalCount.text = goalsGot + "/" + winCondition;
                completedCollums.Add(block.tag);
                foreach (GameObject i in blocks)
                {
                    if (i.tag == block.tag)
                    {
                        i.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                        i.GetComponent<Collider2D>().enabled = false;
                        SpriteRenderer[] sprites = i.GetComponentsInChildren<SpriteRenderer>();
                        foreach (SpriteRenderer square in sprites)
                        {
                            square.color = green;
                        }
                    }
                }
            }
            //if you've won changes the game state to the end game
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
            //If you didnt win keeps on playing
            else
            {
                SpawnDropBlock();
            }
        }
    }

    //Clears variables that the GameManager uses to track game state
    private void ClearVariables()
    {
        winCondition = 0;
        goalsGot = 0;
        blocks.Clear();
        completedCollums.Clear();
        dropBlockSpawn = Vector2.zero;
        currentBlock = 0;
        wonLevel = false;
        gameOverScreen = null;
    }

    public void CloseMMButtons(GameObject MasterButtons, GameObject buttons)
    {
        Back.Play();
        MasterButtons.SetActive(true);
        buttons.SetActive(false);
    }

    public void GameOver()
    {
        Back.Play();
        gameOverScreen.SetActive(true);
        goalCount.text = "Try Again!";
        if(currentGameMode == 2)
        {
            if(currentBlock > endlessHighScore)
            {
                PlayerPrefs.SetInt("endlessHighScore", currentBlock);
                endlessHighScore = currentBlock;
                gameOverScreen.GetComponentInChildren<Text>().text = string.Format("Score: {0}    Best: <color=#56D963FF>{0}</color>", currentBlock);
            }
            else
            {
                gameOverScreen.GetComponentInChildren<Text>().text = string.Format("Score: {0}    Best: {1}", currentBlock, endlessHighScore);
            }
        }
        else
        {
            gameOverScreen.GetComponentInChildren<Text>().text = string.Format("Goals: {0}", goalsGot);
        }

    }

    //Level switching system, if -1 is passed it reloads the level.
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

    public void OpenMMButtons(GameObject MasterButtons, GameObject buttons)
    {
        Confirm.Play();
        MasterButtons.SetActive(false);
        buttons.SetActive(true);
    }

    //Method for starting hard levels.
    public void StartLevelHard(GameObject[] startBlocks, List<GameObject> regBlocks, Vector2 spawn, Text b, GameObject gameOverPanel)
    {
        gameOverScreen = gameOverPanel;
        currentGameMode = 1;
        dropBlockSpawn = spawn;
        foreach (GameObject block in startBlocks)
        {
            blocks.Add(block);
            winCondition++;
        }
        currentBlock = winCondition;
        float currentSpeedMod = 0;
        foreach (GameObject block in regBlocks)
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

    public void StartLevelEndless(GameObject dropBlock, Vector2 spawn, Text b, GameObject gameOverPanel)
    {
        gameOverScreen = gameOverPanel;
        currentGameMode = 2;
        dropBlockSpawn = spawn;
        blocks.Add(dropBlock);
        goalCount = b;
        SpawnDropBlock(blocks[0]);
    }

    //The system for "spawing" the blocks.
    private void SpawnDropBlock()
    {
        blocks[currentBlock].SetActive(true);
        float dist = blocks[currentBlock].GetComponent<MoveBlock>().distance;
        dist = Random.Range((dropBlockSpawn.x - dist), (dropBlockSpawn.x + dist));
        Vector2 finalSpawn = new Vector2(dist, dropBlockSpawn.y);
        blocks[currentBlock].GetComponent<MoveBlock>().SetPosition(dropBlockSpawn);
        blocks[currentBlock].transform.position = finalSpawn;
        currentBlock++;
    }

    private void SpawnDropBlock(GameObject block)
    {
        float dist = block.GetComponent<MoveBlock>().distance;
        dist = Random.Range((dropBlockSpawn.x - dist), (dropBlockSpawn.x + dist));
        Vector2 finalSpawn = new Vector2(dist, dropBlockSpawn.y + 0.46f);
        dropBlockSpawn = new Vector2(dropBlockSpawn.x, finalSpawn.y);
        GameObject newBlock = (GameObject)Instantiate(block, finalSpawn, Quaternion.identity);
        newBlock.GetComponent<MoveBlock>().SetPosition(dropBlockSpawn);
        currentBlock++;
        goalCount.text = (currentBlock - 1).ToString();
    }

    //The system for "spawning" the main menu decrotive blocks.
    public void SpawnMainMenuBlock(GameObject blockNG)
    {
        float newScale = Random.Range(0.3f, 1.0f);
        blockNG.transform.localScale = new Vector3(newScale, newScale);
        blockNG.GetComponent<Rigidbody2D>().gravityScale = newScale;
        blockNG.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        blockNG.transform.position = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(5.25f, 9.25f));
        blockNG.GetComponent<TimedDestroy>().waitTime = (blockNG.transform.position.y / 3f) + (1 - newScale);
    }
}
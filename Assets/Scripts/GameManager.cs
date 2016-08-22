﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isActiveMovingBlock;
    [Header("Block Colors")]
    public Color gold;
    public Color green;
    public Color white;
    [Header("Variable Fields")]
    public float camSpeed = 3;
    public int levelsUnlocked;
    [Header("Sounds")]
    public AudioSource Confirm;
    public AudioSource Back;
    public AudioSource Ding;

    public static GameManager Instance { get; private set; }

    //Lists
    [HideInInspector]
    public List<GameObject> blocks;
    [HideInInspector]
    public List<string> completedCollums;
    //Vars
    private Vector2 dropBlockSpawn;
    private Text goalCount;
    private int winCondition;
    private int goalsGot;
    private int currentBlock;
    private bool wonLevel;
    private int currentGameMode;
    private GameObject endGameScreen;
    private int endlessHighScore;
    private int blocksRemaining;
    private Text blocksRemainingText;

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
            // Furthermore we make sure that we don't destroy between scenes
            DontDestroyOnLoad(gameObject);
            endlessHighScore = PlayerPrefs.GetInt("endlessHighScore");
            Debug.Log(endlessHighScore);
        }
    }

    //Provides the climbing gold block effect and plays a sound
    public void BlockColorChange(GameObject toWhite, GameObject toGold)
    {
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
            SpawnDropBlock(blocks[0], block);
        }
        else {
            SetBlocksRText();
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
                Win();
            }
            //If you didnt win keeps on playing
            else
            {
                if (currentGameMode == 0 && (blocks.Count - currentBlock <= 0))
                {
                    GameOver();
                }
                else {
                    Ding.Play();
                    SpawnDropBlock();
                }
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
        endGameScreen = null;
        blocksRemaining = 0;
        blocksRemainingText = null;
        goalCount = null;
        isActiveMovingBlock = false;
    }

    public bool CloseToSpawn(Vector3 objPos)
    {
        if (objPos.y >= dropBlockSpawn.y - 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DeadBlock()
    {
        if (wonLevel)
        {
            return;
        }
        if (currentGameMode == 0)
        {
            SetBlocksRText();
            if (blocks.Count - currentBlock <= 0)
            {
                GameOver();
            }
            else {
                Back.Play();
                SpawnDropBlock();
            }
        }
        else
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        if (!endGameScreen.activeSelf)
        {
            goalCount.text = "Try Again!";
            if (currentGameMode == 2)
            {
                if (currentBlock - 1 > endlessHighScore)
                {
                    Debug.Log("Bloop currentblock: " + (currentBlock - 1) + " and high score: " + endlessHighScore);
                    PlayerPrefs.SetInt("endlessHighScore", currentBlock - 1);
                    endlessHighScore = currentBlock;
                    endGameScreen.GetComponentInChildren<Text>().text = string.Format("Score: {0}    Best: <color=#56D963FF>{0}</color>", currentBlock - 1);
                }
                else
                {
                    endGameScreen.GetComponentInChildren<Text>().text = string.Format("Score: {0}    Best: {1}", currentBlock - 1, endlessHighScore);
                }
            }
            else
            {
                endGameScreen.GetComponentInChildren<Text>().text = string.Format("Goals: {0}", goalsGot);
            }
            endGameScreen.transform.Find("Restart").gameObject.SetActive(true);
            Back.Play();
            endGameScreen.SetActive(true);
        }
    }

    //Level switching system, if -1 is passed it reloads the level.
    public void LoadLevel(int i)
    {
        if (i == -1)
        {
            Back.Play();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            ClearVariables();
            Time.timeScale = 1;
        }
    }

    public void LoadLevel(string i) {
        if (i == "_MainMenu")
        {
            Back.Play();
        }
        else {
            Confirm.Play();
        }
        SceneManager.LoadScene(i);
        ClearVariables();
        Time.timeScale = 1;
    }

    //Manages the opening and closing of menus on the opening screen
    public void MainMenuUI(GameObject Open, GameObject Close, bool ConfirmSound)
    {
        if (ConfirmSound) Confirm.Play();
        else Back.Play();
        Close.SetActive(false);
        Open.SetActive(true);
    }

    public void SetBlocksRText()
    {
        if (currentGameMode == 0)
        {
            blocksRemaining--;
            blocksRemainingText.text = string.Format("[ {0} ]", (blocksRemaining));
        }
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().name.StartsWith("H_Level"))
        {
            string c = SceneManager.GetActiveScene().name[SceneManager.GetActiveScene().name.Length - 1].ToString();
            int i = int.Parse(c);
            if (i == 0)
            {
                LoadLevel("_MainMenu");
            }
            else {
                LoadLevel("H_Level" + (i + 1));
            }
        }

        else if (SceneManager.GetActiveScene().name.StartsWith("R_Level"))
        {
            string c = SceneManager.GetActiveScene().name[SceneManager.GetActiveScene().name.Length - 1].ToString();
            int i = int.Parse(c);
            if (i == 7)
            {
                LoadLevel("_MainMenu");
            }
            else {
                LoadLevel("R_Level" + (i + 1));
            }
        }
    }

    //Endless: Setting new cam position in a smart way
    private void SetCameraPosition(GameObject oldBlock, GameObject newBlock)
    {
        float newY = (newBlock.transform.position.y + oldBlock.transform.position.y) / 2;
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, newY, Camera.main.transform.position.z);
    }

    public void SetNewestGold(GameObject that)
    {
        if (currentGameMode != 0)
        {
            DeadBlock();
        }
        else {
            GameObject toGold = null;
            foreach (GameObject block in blocks)
            {
                if (block.activeSelf && that.tag == block.tag && block != that && block.GetComponent<Rigidbody2D>().velocity.magnitude <= 0.02)
                {
                    toGold = block;
                }
            }
            toGold.GetComponent<DropBlock>().currentlyGold = true;
            that.GetComponent<DropBlock>().currentlyGold = false;
            SpriteRenderer[] square = toGold.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sprite in square)
            {
                sprite.color = gold;
            }
        }
    }

    public void StartLevelEndless(GameObject dropBlock, Vector2 spawn, Text b, GameObject gameOverPanel)
    {
        endGameScreen = gameOverPanel;
        currentGameMode = 2;
        dropBlockSpawn = spawn;
        blocks.Add(dropBlock);
        goalCount = b;
        SpawnDropBlock(blocks[0]);
    }

    //Method for starting hard levels.
    public void StartLevelHard(GameObject[] startBlocks, List<GameObject> regBlocks, Vector2 spawn, Text b, GameObject gameOverPanel, float speedMod)
    {
        endGameScreen = gameOverPanel;
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

    public void StartLevelRegular(GameObject[] startBlocks, List<GameObject> regBlocks, Vector2 spawn, Text b, GameObject gameOverPanel, Text blocksLeft, float speedMod)
    {
        blocksRemainingText = blocksLeft;
        blocksRemaining = regBlocks.Count + 1;
        endGameScreen = gameOverPanel;
        currentGameMode = 0;
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
        SetBlocksRText();
    }

    //The system for spawning the blocks in a pool.
    private void SpawnDropBlock()
    {
        if (!isActiveMovingBlock)
        {
            blocks[currentBlock].SetActive(true);
            float dist = blocks[currentBlock].GetComponent<MoveBlock>().distance;
            dist = Random.Range((dropBlockSpawn.x - dist), (dropBlockSpawn.x + dist));
            Vector2 finalSpawn = new Vector2(dist, dropBlockSpawn.y);
            blocks[currentBlock].GetComponent<MoveBlock>().SetPosition(dropBlockSpawn);
            blocks[currentBlock].transform.position = finalSpawn;
            currentBlock++;
            isActiveMovingBlock = true;
        }
    }
    //System for spawning the blocks in Endless mode
    private void SpawnDropBlock(GameObject block)
    {
        if (!isActiveMovingBlock)
        {
            float dist = block.GetComponent<MoveBlock>().distance;
            dist = Random.Range((dropBlockSpawn.x - dist), (dropBlockSpawn.x + dist));
            Vector2 finalSpawn = new Vector2(dist, dropBlockSpawn.y);
            dropBlockSpawn = new Vector2(dropBlockSpawn.x, finalSpawn.y);
            GameObject newBlock = (GameObject)Instantiate(block, finalSpawn, Quaternion.identity);
            newBlock.GetComponent<MoveBlock>().SetPosition(dropBlockSpawn);
            currentBlock++;
            goalCount.text = (currentBlock - 1).ToString();
            isActiveMovingBlock = true;
        }
    }

    private void SpawnDropBlock(GameObject block, GameObject oldBlock)
    {
        if (!isActiveMovingBlock)
        {
            float dist = block.GetComponent<MoveBlock>().distance;
            dist = Random.Range((dropBlockSpawn.x - dist), (dropBlockSpawn.x + dist));
            Vector2 finalSpawn = new Vector2(dist, oldBlock.transform.position.y + 4.5f);
            dropBlockSpawn = new Vector2(dropBlockSpawn.x, finalSpawn.y);
            GameObject newBlock = (GameObject)Instantiate(block, finalSpawn, Quaternion.identity);
            newBlock.GetComponent<MoveBlock>().SetPosition(dropBlockSpawn);
            SetCameraPosition(oldBlock, newBlock);
            currentBlock++;
            goalCount.text = (currentBlock - 1).ToString();
            isActiveMovingBlock = true;
        }
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

    public void Win()
    {
        endGameScreen.SetActive(true);
        goalCount.text = "You Win!";
        endGameScreen.GetComponentInChildren<Text>().text = string.Format("Goals: {0}", goalsGot);
        endGameScreen.transform.Find("Next").gameObject.SetActive(true);
    }
}
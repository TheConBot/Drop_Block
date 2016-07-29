using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject dropBlock;
    public List<GameObject> blocks;
    public Color gold;
    public static GameManager Instance { get; private set; }

    private Vector2 dropBlockSpawn;
    private int currentLevel = 0;

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

    public void blockColorChange(int toWhite, int toGold)
    {
        SpriteRenderer[] square = blocks[blocks.Count - toWhite].GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in square)
        {
            sprite.color = Color.white;
        }
        //Set newly dropped block to gold
        square = blocks[blocks.Count - toGold].GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in square)
        {
            sprite.color = gold;
        }
    }

    public void spawnDropBlock()
    {
        GameObject Block = (GameObject)Instantiate(dropBlock, dropBlockSpawn, Quaternion.identity);
        Block.name = "Drop_Blocks_" + (blocks.Count - 1);
        blocks.Add(Block);
    }

    public void StartLevel(GameObject[] startBlocks, Vector2 spawn)
    {
        dropBlockSpawn = spawn;
        foreach (GameObject block in startBlocks)
        {
            blocks.Add(block);
        }
        spawnDropBlock();
    }

    public void LoadLevel(int i)
    {
        if (i == -1) SceneManager.LoadScene(currentLevel);
        else {
            SceneManager.LoadScene(i);
            currentLevel = i;
        }
    }
}

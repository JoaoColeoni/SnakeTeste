using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjSpawnerManager : MonoBehaviour
{
    public GameObject looseBlockPrefab, snakeControllerPrefab, snakeBlockPrefab;
    [SerializeField]
    private Vector2 minSize;
    [SerializeField]
    private Vector2 maxSize;
    [SerializeField]
    private Sprite[] blockSprites;
    [SerializeField]
    private Sprite[] LooseBlockSprites;

    public void SpawnIASnake(Color snakeColor, int index)
    {
        SnakeController snakeController = Instantiate(snakeControllerPrefab, Vector3.zero, Quaternion.identity).GetComponent<SnakeController>();
        snakeController.snakeIndex = index;
        IAInputs iaInputs = snakeController.gameObject.AddComponent<IAInputs>();
        iaInputs.iaSnake = snakeController;
        Vector2 startPosition = EmptyPositionInMap();
        for (var i = 0; i < 3; i++)
        {
            SnakeBlock snakeBlock = Instantiate(snakeBlockPrefab, startPosition, Quaternion.identity, snakeController.transform).GetComponent<SnakeBlock>();
            snakeBlock.GetComponent<SpriteRenderer>().color = snakeColor;
            snakeBlock.GetComponent<SpriteRenderer>().sprite = SpriteSelector(snakeBlock.blockType, i);
            if(i > 0)
            {
                snakeBlock.transform.Rotate(0, 90, 0);
            }
            snakeController.snakeBlocks.Add(snakeBlock);
        }
        GameManager.Instance.enemyList.Add(snakeController.gameObject);
    }

    public void SpanwPlayerSnake(SnakeType snakeType, string leftKey, string rightKey, Color snakeColor, int index)
    {
        SnakeController snakeController = Instantiate(snakeControllerPrefab, Vector3.zero, Quaternion.identity).GetComponent<SnakeController>();
        snakeController.snakeIndex = index;
        PlayerInputs playerInputs = snakeController.gameObject.AddComponent<PlayerInputs>();
        playerInputs.SetKeys(leftKey, rightKey);
        playerInputs.playerSnake = snakeController;
        Vector2 startPosition = EmptyPositionInMap();
        //SpanwSnakeBlocks
        int RngBlockType = UnityEngine.Random.Range(0,Enum.GetNames(typeof(BlockType)).Length);
        for (var i = 0; i < 3; i++)
        {
            if(i == 2 && snakeType == SnakeType.Diferent)
            {
                RngBlockType = (RngBlockType + UnityEngine.Random.Range(1,Enum.GetNames(typeof(BlockType)).Length-1)) % Enum.GetNames(typeof(BlockType)).Length;
            }
            SnakeBlock snakeBlock = Instantiate(snakeBlockPrefab, startPosition, Quaternion.identity, snakeController.transform).GetComponent<SnakeBlock>();
            snakeBlock.blockType = (BlockType)RngBlockType;
            snakeBlock.GetComponent<SpriteRenderer>().color = snakeColor;
            snakeBlock.GetComponent<SpriteRenderer>().sprite = SpriteSelector(snakeBlock.blockType, i);
            if(i > 0)
            {
                snakeBlock.transform.Rotate(0, 90, 0);
            }
            if(snakeBlock.blockType == BlockType.Power)
            {
                snakeController.speed += 10;
            }
            snakeController.snakeBlocks.Add(snakeBlock);
        }
        GameManager.Instance.playerList.Add(snakeController.gameObject);
    }

    public Sprite SpriteSelector(BlockType type, int index)
    {
        if(type == BlockType.Power)
            return blockSprites[4];
        else if(type == BlockType.Ram)
            return blockSprites[3];
        else if(type == BlockType.Regular && index == 0)
            return blockSprites[0];
        else if(type == BlockType.Regular && index == 2)
            return blockSprites[2];
        else
            return blockSprites[1];
    }

    public void SpawnNewBlock(Color color, int index)
    {
        SnakeBlock looseBlock = Instantiate(looseBlockPrefab, EmptyPositionInMap(), Quaternion.identity).GetComponent<SnakeBlock>();
        GameManager.Instance.enemyList[index].GetComponent<IAInputs>().targetBlock = looseBlock.gameObject;
        int RngBlockType = UnityEngine.Random.Range(0,Enum.GetNames(typeof(BlockType)).Length);
        looseBlock.index = index;
        looseBlock.blockType = (BlockType)RngBlockType;
        looseBlock.GetComponent<SpriteRenderer>().sprite = LooseBlockSprites[RngBlockType];
        looseBlock.GetComponent<SpriteRenderer>().color = color;
    }

    public Vector2 EmptyPositionInMap()
    {
        List<Vector2> EmptySpaces = new List<Vector2>();
        for (var x = minSize.x; x < maxSize.x; x++)
        {
            for (var y = minSize.y; y < maxSize.y; y++)
            {
                if(Physics2D.OverlapCircle(new Vector2(x,y),0.25f) == null)
                {
                    EmptySpaces.Add(new Vector2(x,y));
                }
            }
        }
        int RngPosition = UnityEngine.Random.Range(0,EmptySpaces.Count);
        return EmptySpaces[RngPosition];
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public int snakeIndex;
    public List<SnakeBlock> snakeBlocks;
    public float speed;
    public int turnCooldown = 2;

    [SerializeField]
    private float regularActionTime, nextActionTime;
    private bool alive;

    public void StartGame()
    {
        alive = true;
        turnCooldown = 0;
        CalculateNextActionTime();
    }

    private void Update()
    {
        if(!alive)
            return;

        if(nextActionTime < Time.time)
        {
            CheckAction();
        }
    }

    private void CalculateNextActionTime()
    {
        if(this.GetComponent<IAInputs>())
            this.GetComponent<IAInputs>().Think();

        nextActionTime = Time.time + regularActionTime/speed;
    }

    public void TurnDirection(int direction)
    {
        turnCooldown = 2;
        snakeBlocks[0].transform.eulerAngles += new Vector3(0, 0, direction*90);   
    }

    private void CheckAction()
    {
        if(turnCooldown > 0)
            turnCooldown--;

        Vector2 movePosition = (Vector2)snakeBlocks[0].transform.position + (Vector2)snakeBlocks[0].transform.up;
        Collider2D checkColision = Physics2D.OverlapCircle(movePosition,0.25f);
        if(checkColision != null && checkColision.tag == "XWall")
        {
            movePosition = new Vector2(snakeBlocks[0].transform.position.x * -1, snakeBlocks[0].transform.position.y);
            checkColision = Physics2D.OverlapCircle(movePosition,0.25f);
        }
        else if(checkColision != null && checkColision.tag == "YWall")
        {
            movePosition = new Vector2(snakeBlocks[0].transform.position.x, snakeBlocks[0].transform.position.y * -1);
            checkColision = Physics2D.OverlapCircle(movePosition,0.25f);
        }

        if(checkColision == null)
        {
            Move(movePosition);
        }
        else if(checkColision.tag == "Snake")
        {
            bool Saved = false;
            if(checkColision.transform.parent != this.transform)
            {
                //Verify if has RAM power
                foreach (SnakeBlock block in snakeBlocks)
                {
                    if(block.blockType == BlockType.Ram)
                    {
                        block.blockType = BlockType.Regular;
                        block.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.SpriteSelector(BlockType.Regular,snakeBlocks.IndexOf(block));
                        Saved = true;
                        continue;
                    }
                }
            }
            
            if(Saved)
                Move(movePosition);
            else
                GameLose();
        }
        else if(checkColision.tag == "LooseBlock")
        {
            if(checkColision.GetComponent<SnakeBlock>().index == snakeIndex)
                ConsumeBlock(checkColision.gameObject);
            else
                Move(movePosition);
        }

        CalculateNextActionTime();
    }

    private void GameLose()
    {
        if(GetComponent<PlayerInputs>())
        {
            alive = false;
            gameObject.SetActive(false);
            GameManager.Instance.PlayerDeath(snakeIndex);
        }
        else if(GetComponent<IAInputs>())
        {
            Vector2 respawnPosition = GameManager.Instance.EmptyPositionInMap();
            for (var i = 0; i < snakeBlocks.Count; i++)
            {
                snakeBlocks[i].transform.position = respawnPosition;
                if(i > 0)
                {
                    snakeBlocks[i].transform.Rotate(0, 90, 0);
                }
            }
        }
    }

    private void Move(Vector2 movePosition)
    {
        for (var i = snakeBlocks.Count-1; i > 0; i--)
        {
            snakeBlocks[i].transform.position = snakeBlocks[i-1].transform.position;
            snakeBlocks[i].transform.rotation = snakeBlocks[i-1].transform.rotation;
        }
        snakeBlocks[0].transform.position = movePosition;
    }

    private void ConsumeBlock(GameObject block)
    {
        block.transform.parent = this.transform;
        block.tag = "Snake";
        snakeBlocks.Insert(0, block.GetComponent<SnakeBlock>());
        snakeBlocks[0].transform.rotation = snakeBlocks[1].transform.rotation;

        //UpdateSprites
        snakeBlocks[0].GetComponent<SpriteRenderer>().sprite = GameManager.Instance.SpriteSelector(snakeBlocks[0].blockType,0);
        snakeBlocks[1].GetComponent<SpriteRenderer>().sprite = GameManager.Instance.SpriteSelector(snakeBlocks[1].blockType,1);
        snakeBlocks[0].GetComponent<SpriteRenderer>().color = snakeBlocks[1].GetComponent<SpriteRenderer>().color;

        //ModifySpeed
        speed -= 1;
        if(snakeBlocks[0].blockType == BlockType.Power)
            speed += 10;

        Color auxColor = snakeBlocks[0].GetComponent<SpriteRenderer>().color;
        auxColor.a = 1f;
        GameManager.Instance.SpawnNewBlock(auxColor, snakeIndex);
    }
}

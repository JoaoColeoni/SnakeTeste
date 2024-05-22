using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAInputs : MonoBehaviour
{
    public SnakeController iaSnake;
    public GameObject targetBlock;

    public void Think()
    {
        if(iaSnake.turnCooldown > 0)
            return;
        
        Vector2 distance = targetBlock.transform.position - iaSnake.snakeBlocks[0].transform.position;
        if(
            (distance.x < 0 && Mathf.Round(iaSnake.snakeBlocks[0].transform.up.x) == -1f) ||
            (distance.x > 0 && Mathf.Round(iaSnake.snakeBlocks[0].transform.up.x) == 1f ) ||
            (distance.y < 0 && Mathf.Round(iaSnake.snakeBlocks[0].transform.up.y) == -1f) ||
            (distance.y > 0 && Mathf.Round(iaSnake.snakeBlocks[0].transform.up.y) == 1f ) 
        )
        {
            return;
        }
        else if(distance.x != 0f && Mathf.Round(iaSnake.snakeBlocks[0].transform.up.x) == 0)
        {
            iaSnake.TurnDirection((int)(Mathf.Sign((int)distance.x)*Mathf.Round(iaSnake.snakeBlocks[0].transform.up.y))*-1);
        }
        else if(distance.y != 0f && Mathf.Round(iaSnake.snakeBlocks[0].transform.up.y) == 0f)
        {
            iaSnake.TurnDirection((int)(Mathf.Sign((int)distance.y)*Mathf.Round(iaSnake.snakeBlocks[0].transform.up.x)));
        }
    }

    public void DisableIA()
    {
        iaSnake.enabled = false;
        iaSnake.gameObject.SetActive(false);
        Destroy(targetBlock);
    }
}

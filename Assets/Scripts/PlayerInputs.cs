using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    public SnakeController playerSnake;
    [SerializeField]
    private string leftKey, rightKey;

    private void Update()
    {
        if (Input.GetKeyDown(leftKey) && playerSnake.turnCooldown == 0)
        {
            playerSnake.TurnDirection(1);
        }
        else if (Input.GetKeyDown(rightKey) && playerSnake.turnCooldown == 0)
        {
            playerSnake.TurnDirection(-1);
        }
    }

    public void SetKeys(string left, string right)
    {
        leftKey = left;
        rightKey = right;
    }
}

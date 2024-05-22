using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public List<GameObject> playerList, enemyList;
    private List<int> playerLeaderBoard;

    [SerializeField]
    private TMP_Text countdownTxt;
    [SerializeField]
    private GameObject backgroundImg;
    [SerializeField]
    public ObjSpawnerManager objSpawnerManager;
    [SerializeField]
    private SetupPlayers setupPlayersController;
    [SerializeField]
    private LeaderBoardController leaderBoardController;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartGame()
    {
        setupPlayersController.enabled = false;
        setupPlayersController.gameObject.SetActive(false);
        playerList = new List<GameObject>();
        enemyList = new List<GameObject>();
        playerLeaderBoard = new List<int>();
        SpawnAllSnakes();
        StartCoroutine("StartCountdown");
    }

    private void SpawnAllSnakes()
    {
        int count = 0;
        foreach (PlayerTag tag in setupPlayersController.playerTags)
        {
            objSpawnerManager.SpanwPlayerSnake(tag.snakeType, tag.key1, tag.key2, tag.playerColor, count);
            Color auxColor = tag.playerColor;
            auxColor.a = 0.25f;
            objSpawnerManager.SpawnIASnake(auxColor, count);
            objSpawnerManager.SpawnNewBlock(tag.playerColor, count);
            count++;
        }
    }

    IEnumerator StartCountdown()
    {
        countdownTxt.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        countdownTxt.text = "2";
        yield return new WaitForSeconds(1);
        countdownTxt.text = "1";
        yield return new WaitForSeconds(1);
        countdownTxt.text = "GO!";
        yield return new WaitForSeconds(1);
        countdownTxt.gameObject.SetActive(false);
        backgroundImg.SetActive(false);
        foreach (GameObject playerGo in playerList)
        {
            playerGo.GetComponent<SnakeController>().StartGame();
        }
        foreach (GameObject iaGo in enemyList)
        {
            iaGo.GetComponent<SnakeController>().StartGame();
        }
    }

    public void PlayerDeath(int index)
    {
        playerLeaderBoard.Insert(0,index);
        enemyList[index].GetComponent<IAInputs>().DisableIA();
        foreach (GameObject playerGO in playerList)
        {
            if(playerGO.activeSelf)
                return;
        }
        CallLeaderBoard();
    }

    private void CallLeaderBoard()
    {
        leaderBoardController.StartLeaderBoard(playerLeaderBoard);
    }

    public string GetPlayerName(int index)
    {
        return setupPlayersController.playerTags[index].nameTxt.text;
    }

    public Color GetPlayerColor(int index)
    {
        return setupPlayersController.playerTags[index].playerColor;
    }
}

public enum BlockType {Regular = 0, Power = 1, Ram = 2};

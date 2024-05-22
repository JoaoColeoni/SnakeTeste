using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SetupPlayers : MonoBehaviour
{
    public List<PlayerTag> playerTags;
    [SerializeField]
    private GameObject playerList, playerTag, startTxt;
    private List<string> usedKeys;
    private string key1, key2;
    private float timeValidKeyPressed;

    void Awake()
    {
        playerTags = new List<PlayerTag>();
        usedKeys = new List<string>();
        key1 = "";
        key2 = "";
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && playerList.transform.childCount > 0)
        {
            foreach (PlayerTag tag in playerTags)
            {
                tag.enabled = false;
            }
            GameManager.Instance.StartGame();
            return;
        }

        if (Input.anyKeyDown)
        {
            if(Input.inputString.Trim().Length == 1 && !usedKeys.Contains(Input.inputString))
            {
                if(key1 == "")
                {
                    key1 = Input.inputString;
                    timeValidKeyPressed = Time.time;
                }
                else if(key2 == "")
                {
                    key2 = Input.inputString;
                    timeValidKeyPressed = Time.time;
                }
            }
        }

        if(key1 != "" && Input.GetKeyUp(key1))
        {
            key1 = "";
        }
        if(key2 != "" && Input.GetKeyUp(key2))
        {
            key2 = "";
        }

        if(timeValidKeyPressed + 1f < Time.time && key1 != "" && key2 != "")
        {
            AddPlayer();
            startTxt.SetActive(true);
        }
    }

    void AddPlayer()
    {
        PlayerTag tag = Instantiate(playerTag,playerList.transform).GetComponent<PlayerTag>();
        tag.InitTag("P"+playerList.transform.childCount.ToString(),key1,key2);
        playerTags.Add(tag);
        usedKeys.Add(key1);
        usedKeys.Add(key2);
        key1 = "";
        key2 = "";
    }
}

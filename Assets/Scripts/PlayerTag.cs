using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerTag : MonoBehaviour
{
    public SnakeType snakeType;
    public string key1,key2;
    public Color playerColor;
    public Image colorImg;
    public TMP_Text nameTxt, typeTxt, key1Txt, key2Txt;

    public void InitTag(string name, string outKey1, string outKey2)
    {
        snakeType = SnakeType.Diferent;
        key1 = outKey1;
        key2 = outKey2;
        nameTxt.text = name;
        key1Txt.text = key1.ToUpper() + " -";
        key2Txt.text = key2.ToUpper() + " -";
        ChangeColor();
    }

    public void InitLeaderboardTag(int position, string name, Color color)
    {
        typeTxt.gameObject.SetActive(false);
        key1Txt.gameObject.SetActive(false);
        key2Txt.gameObject.SetActive(false);
        nameTxt.text = position.ToString() + " - " + name;
        colorImg.color = color;
    }

    private void Update()
    {
        if (Input.GetKeyDown(key1))
        {
            ChangeType();
        }
        if (Input.GetKeyDown(key2))
        {
            ChangeColor();
        }
    }

    void ChangeColor()
    {
        playerColor = new Color(
                                (float)UnityEngine.Random.Range(0,256)/255f,
                                (float)UnityEngine.Random.Range(0,256)/255f,
                                (float)UnityEngine.Random.Range(0,256)/255f,
                                1);
        colorImg.color = playerColor;
    }

    void ChangeType()
    {
        snakeType = (SnakeType)(((int)snakeType + 1) % Enum.GetNames(typeof(SnakeType)).Length);

        if(snakeType == SnakeType.Diferent)
        {
            typeTxt.text = "2 Blocos Iguais e 1 diferente";
        }
        else if(snakeType == SnakeType.Equal)
        {
            typeTxt.text = "3 Blocos Iguais";
        }
    }
}

public enum SnakeType {Diferent, Equal};

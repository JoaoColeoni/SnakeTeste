using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderBoardController : MonoBehaviour
{
    public GameObject playerTagPrefab, leaderboardPannel;
    // Start is called before the first frame update
    public void StartLeaderBoard(List<int> playerIndex)
    {
        transform.parent.gameObject.SetActive(true);
        gameObject.SetActive(true);
        int count = 1;
        foreach (int index in playerIndex)
        {
            PlayerTag tag = Instantiate(playerTagPrefab,leaderboardPannel.transform).GetComponent<PlayerTag>();
            tag.enabled = false;
            tag.InitLeaderboardTag(count,GameManager.Instance.GetPlayerName(index),GameManager.Instance.GetPlayerColor(index));
            count++;
        }
    }

    void Update()
    {
        if(gameObject.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

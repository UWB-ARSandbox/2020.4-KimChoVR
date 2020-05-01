using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CupPongScoreBoard : MonoBehaviour
{
    public List<GameObject> p1Cups;
    public List<GameObject> p2Cups;

    public TextMeshProUGUI p1ScoreText;
    public TextMeshProUGUI p2ScoreText;

    public GameObject scorePanel;
    public GameObject p1WinsPanel;
    public GameObject p2WinsPanel;

    public int p1Score;
    public int p2Score;

    private bool hasWon;

    // Start is called before the first frame update
    void Start()
    {
        p1Score = p1Cups.Count;
        p2Score = p2Cups.Count;
        hasWon = false;
    }

    // Update is called once per frame
    void Update()
    {
        p1Score = CountCups(p1Cups);
        p2Score = CountCups(p2Cups);

        p1ScoreText.text = "" + p1Score;
        p2ScoreText.text = "" + p2Score;

        if (p2Score <= 0 && !hasWon)
        {
            scorePanel.SetActive(false);
            p1WinsPanel.SetActive(true);
            hasWon = true;
        }

        if (p1Score <= 0 && !hasWon)
        {
            scorePanel.SetActive(false);
            p2WinsPanel.SetActive(true);
            hasWon = true;
        }

    }

    private int CountCups(List<GameObject> playerCupList)
    {
        int count = 0;

        for (int i = 0; i < playerCupList.Count; i++)
        {
            if (playerCupList[i] != null)
            {
                count++;
            }
        }

        return count;
    }
}

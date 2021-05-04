using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    int score_Count = 0;

    public Text score_CountText;

    void Start()
    {
        score_Count = 0;
        score_CountText.text = score_Count.ToString();
    }
    public void Score(int score)
    {

        score_Count += score;

        Changescore_Count();
    }

    void Changescore_Count()
    {

        if (score_CountText)
        {
            score_CountText.text = score_Count.ToString();
        }
    }
}

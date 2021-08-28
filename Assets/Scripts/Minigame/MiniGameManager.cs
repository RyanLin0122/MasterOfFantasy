using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MiniGameManager : MonoBehaviour
{

    #region Ranking
    public Text[] Names = new Text[10];
    public Text[] Scores = new Text[10];
    public Dictionary<string, int> ranking;
    public void InitRanking()
    {
        ranking = GameRoot.Instance.gameObject.GetComponent<GotoMiniGame>().ranking;
        int[] ScoreArray = new int[10];
        int index = 0;
        foreach (var value in ranking.Values)
        {
            ScoreArray[index] = value;
            index++;
        }

        int i, j, temp;
        for (i = ScoreArray.Length - 1; i >= 0; i--)
        {
            for (j = 0; j < i; j++)
            {
                if (ScoreArray[j] <= ScoreArray[i])
                {
                    temp = ScoreArray[i];
                    ScoreArray[i] = ScoreArray[j];
                    ScoreArray[j] = temp;
                }
            }
        }
        string[] NameArray = new string[] { "", "", "", "", "", "", "", "", "", "" };
        foreach (var name in ranking.Keys)
        {
            for (int k = 0; k < 10; k++)
            {

                if (ranking[name] == ScoreArray[k])
                {
                    if (NameArray[k] == "")
                    {
                        NameArray[k] = name;
                    }
                }
            }
        }

        for (int r = 0; r < 10; r++)
        {
            print(NameArray[r]);
            print(ScoreArray[r]);

        }

        for (int m = 0; m < 10; m++)
        {
            Names[m].text = NameArray[m];
            Scores[m].text = ScoreArray[m].ToString() + "¤À";
        }
    }
    #endregion
}

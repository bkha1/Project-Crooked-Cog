using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour {
    //private GUIStyle myStyle;

    void Awake()
    {
        //myStyle.normal.textColor = Color.black;
        //myStyle.fontSize = 10;
        Screen.lockCursor = false;
    }

    void Update()
    {
        Screen.lockCursor = false;
    }

    void OnGUI()
    {
        GUI.contentColor = Color.black;
 
        if (StageStatsScript.Instance.goalsLeft <= 0)
        {
            string tempOffenseRank;
            string tempDefenseRank;
            string tempSpeedRank;
            //offense
            if (StageStatsScript.Instance.offenseRank == 6)
            {
                tempOffenseRank = "S";
            }
            else if (StageStatsScript.Instance.offenseRank == 5)
            {
                tempOffenseRank = "A";
            }
            else if (StageStatsScript.Instance.offenseRank == 4)
            {
                tempOffenseRank = "B";
            }
            else if (StageStatsScript.Instance.offenseRank == 3)
            {
                tempOffenseRank = "C";
            }
            else if (StageStatsScript.Instance.offenseRank == 2)
            {
                tempOffenseRank = "D";
            }
            else
            {
                tempOffenseRank = "F";
            }
            //defense
            if (StageStatsScript.Instance.defenseRank == 6)
            {
                tempDefenseRank = "S";
            }
            else if (StageStatsScript.Instance.defenseRank == 5)
            {
                tempDefenseRank = "A";
            }
            else if (StageStatsScript.Instance.defenseRank == 4)
            {
                tempDefenseRank = "B";
            }
            else if (StageStatsScript.Instance.defenseRank == 3)
            {
                tempDefenseRank = "C";
            }
            else if (StageStatsScript.Instance.defenseRank == 2)
            {
                tempDefenseRank = "D";
            }
            else
            {
                tempDefenseRank = "F";
            }
            //speed
            if (StageStatsScript.Instance.speedRank == 6)
            {
                tempSpeedRank = "S";
            }
            else if (StageStatsScript.Instance.speedRank == 5)
            {
                tempSpeedRank = "A";
            }
            else if (StageStatsScript.Instance.speedRank == 4)
            {
                tempSpeedRank = "B";
            }
            else if (StageStatsScript.Instance.speedRank == 3)
            {
                tempSpeedRank = "C";
            }
            else if (StageStatsScript.Instance.speedRank == 2)
            {
                tempSpeedRank = "D";
            }
            else
            {
                tempSpeedRank = "F";
            }

            GUI.Label(new Rect(10, 10, 100, 20), "Offense: "+ tempOffenseRank);
            GUI.Label(new Rect(10, 25, 100, 20), "Defense: " + tempDefenseRank);
            GUI.Label(new Rect(10, 40, 100, 20), "Speed: " + tempSpeedRank);
            GUI.Label(new Rect(10, 55, 300, 20), "Score: " + StageStatsScript.Instance.totalScore * StageStatsScript.Instance.offenseRank * StageStatsScript.Instance.defenseRank * StageStatsScript.Instance.speedRank);//Combo Bonus = cumulative combo points * offenserank * defenserank * speedrank
        }

        const int buttonWidth = 120;
        const int buttonHeight = 60;

        if (
          GUI.Button(
            // Center in X, 1/3 of the height in Y
            new Rect(
              Screen.width / 2 - (buttonWidth / 2),
              (1 * Screen.height / 3) - (buttonHeight / 2),
              buttonWidth,
              buttonHeight
            ),
            "Retry!"
          )
        )
        {
            // Reload the level
            Application.LoadLevel("DemoScene");
        }

        if (
          GUI.Button(
            // Center in X, 2/3 of the height in Y
            new Rect(
              Screen.width / 2 - (buttonWidth / 2),
              (2 * Screen.height / 3) - (buttonHeight / 2),
              buttonWidth,
              buttonHeight
            ),
            "Back to menu"
          )
        )
        {
            // Reload the level
            Application.LoadLevel("Menu");
        }
    }
}

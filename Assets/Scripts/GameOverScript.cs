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

    void OnGUI()
    {
        GUI.contentColor = Color.black;
 
        if (StageStatsScript.Instance.goalsLeft <= 0)
        {
            GUI.Label(new Rect(10, 10, 100, 20), "Offense: "+ StageStatsScript.Instance.offenseRank);
            GUI.Label(new Rect(10, 25, 100, 20), "Defense: "+ StageStatsScript.Instance.defenseRank);
            GUI.Label(new Rect(10, 40, 100, 20), "Speed: "+ StageStatsScript.Instance.speedRank);
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

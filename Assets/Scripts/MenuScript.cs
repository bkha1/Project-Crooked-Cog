using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour
{

    void OnGUI()
    {
        const int buttonWidth = 84;
        const int buttonHeight = 60;

        GUI.contentColor = Color.black;
        string titleString = "PROJECT CROOKED COG: PROOF OF CONCEPT";
        GUI.Label(new Rect(Screen.width / 3, Screen.height / 3, Screen.width, Screen.height), titleString);

        // Draw a button to start the game
        if (
          GUI.Button(
            // Center in X, 2/3 of the height in Y
            new Rect(
              Screen.width / 2 - (buttonWidth / 2),
              (2 * Screen.height / 3) - (buttonHeight / 2),
              buttonWidth,
              buttonHeight
            ),
            "Start Demo"
          )
        )
        {
            // On Click, load the first level.
            // "Stage1" is the name of the first scene we created.
            Application.LoadLevel("DemoScene");
        }

        if(GUI.Button(new Rect(Screen.width/2 - (buttonWidth/2),(2 * Screen.height/3) + (buttonHeight),buttonWidth,buttonHeight),"Quit Game"))
        {
            Application.Quit();
        }
    }
}

using UnityEngine;
using System.Collections;

public class StageStatsScript: MonoBehaviour {

    public static StageStatsScript Instance;

    public int comboReq;
    public float timeReq;
    public int deathReq;

    public int highestCombo;
    public float stageTime;
    public int numOfDeaths;

    public int playerXP;
    public int XPdecreaseRate = 2;
    private float perSecond = 0;

    //public TextMesh timeValueText;
    public GUIText timeValueText;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of StageStatsScript!");
        }

        Instance = this;
    }

	// Use this for initialization
	void Start () {
        highestCombo = 0;
        stageTime = 0;
        numOfDeaths = 0;

        playerXP = 0;
        perSecond = 0;
	}
	
	// Update is called once per frame
	void Update () {

        stageTime += Time.deltaTime;
        perSecond += Time.deltaTime;

        if (perSecond >= 1)
        {
            playerXP -= XPdecreaseRate;
            perSecond = 0;
        }

        if (playerXP < 0)
        {
            playerXP = 0;
        }

        timeValueText.text = stageTime.ToString("0.##");
	}

    public void increaseXP(int i)
    {
        playerXP+=i;
    }

    public void wipeXP()
    {
        playerXP = 0;
    }

    public void levelUp()
    {
        highestCombo++;
    }

    public Texture barBackTexture;
    public Texture barFrontTexture;
    void OnGUI()
    {
        //GUI.Box(new Rect(Screen.width * .15f, Screen.height * .96f, Screen.width / 4 , 20), "100"+ "/" +"100");
        //GUI.DrawTexture(new Rect(Screen.width * .15f, Screen.height * .96f, Screen.width / 4, 20), true, 1);// "100" + "/" + "100");
        //GUI.DrawTexture(new Rect(10, 10, 60, 60), aTexture, ScaleMode.ScaleToFit, true, 10.0F);
        //GUI.DrawTexture(new Rect(Screen.width * .15f, Screen.height * .96f, Screen.width / 4, 20), barBackTexture); //ScaleMode.ScaleToFit, true, 10.0F);
        //GUI.DrawTexture(new Rect(Screen.width * .15f, Screen.height * .96f, (Screen.width / 4) , 5), barFrontTexture); //ScaleMode.ScaleToFit, true, 10.0F);
    }
}

using UnityEngine;
using System.Collections;

public class StageStatsScript: MonoBehaviour {

    public static StageStatsScript Instance;

    public int parCombo;//points gained after a full combo
    public float parTime;
    public int parDeath;

    public int highestCombo;
    public float stageTime;
    public int numOfDeaths;

    public int currentCombo;

    public int playerXP;
    public int XPdecreaseRate = 5;
    private float perSecond = 0;
    private int nextLevel = 80;
    public int levelValue = 1;

    public int playerLives = 3;

    public GUIText timeValueText;
    public GUIText livesValueText;
    public GUIText levelValueText;

    public GUITexture upgradeMeter;
    private int meterMaxWidth = 120;

    private bool pauseTimer;

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

        currentCombo = 0;
        playerXP = 0;
        perSecond = 0;

        playerLives = 3;
        levelValue = 1;

        pauseTimer = false;

        respawnCooldown = respawnTime;
	}
	
	// Update is called once per frame
	void Update () {

        Rect tempMeter = upgradeMeter.pixelInset;
        tempMeter.width = ((float)playerXP / (float)nextLevel) * meterMaxWidth; ;
        upgradeMeter.pixelInset = tempMeter;

        if (!pauseTimer)
        {
            stageTime += Time.deltaTime;
        }
        perSecond += Time.deltaTime;

        if (perSecond >= .25f)//player will lose 5 percent of bar every second
        {
            playerXP -= (int)((float)nextLevel * .05f * .25f);
            perSecond = 0;
        }

        if (playerXP < 0)
        {
            playerXP = 0;
            /*if (levelValue > 1)
            {
                levelValue--;
                nextLevel = (int)(nextLevel / 1.8);
                playerXP = nextLevel;
            }
            else
            {
                playerXP = 0;
            }*/
        }

        if (levelValue < 1)
        {
            levelValue = 1;
        }

        if (playerXP > nextLevel)
        {
            if (levelValue < 3)
            {
                SoundEffectsScript.Instance.playPowerUpSound1(1);
                levelValue++;
                playerXP = 0;
                nextLevel = (int)(nextLevel * 1.8);
            }
            else
            {
                int tempDiff = playerXP - nextLevel;
                currentCombo += tempDiff;
                if (highestCombo < currentCombo)
                {
                    highestCombo = currentCombo;
                }
                playerXP = nextLevel;
                //Debug.Log("highest combo " + highestCombo);
                //Debug.Log("current combo " + currentCombo);
            }
        }

        timeValueText.text = "Time:" + stageTime.ToString("0.##");
        livesValueText.text = "x" + playerLives.ToString();
        levelValueText.text = "Lvl:" + levelValue.ToString();

        if (respawning)
        {
            levelValue = 1;
            nextLevel = 80;
            playerXP = 0;
            currentCombo = 0;
            respawnCooldown-=Time.deltaTime;
            if (respawnCooldown <= 0)
            {
                respawnCooldown = respawnTime;
                respawning = false;
                StartCoroutine(respawnPlayer(Instantiate(player) as Transform));
            }
        }
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

    public Transform player;
    private float respawnCooldown;
    public float respawnTime = 1;
    private bool respawning = false;
    IEnumerator respawnPlayer(Transform playerObject)
    {
        playerObject.position = new Vector3(0, -3, 0);

        foreach (Behaviour childComponent in playerObject.GetComponentsInChildren<Behaviour>())
        {
            childComponent.enabled = false;
        }

        foreach (Animator rend in playerObject.GetComponentsInChildren<Animator>())
        {
            rend.enabled = true;
        }

        foreach (SpriteRenderer spr in playerObject.GetComponentsInChildren<SpriteRenderer>())
        {
            spr.enabled = true;
            Color tempCol = spr.color;
            tempCol.a = .5f;
            spr.color = tempCol;
        }

        PlayerMovementScript mov = playerObject.GetComponent<PlayerMovementScript>();
        mov.enabled = true;

        yield return new WaitForSeconds(2f);

        foreach (Behaviour childCompnent in playerObject.GetComponentsInChildren<Behaviour>())
        {
            childCompnent.enabled = true;
        }

        foreach (SpriteRenderer spr in playerObject.GetComponentsInChildren<SpriteRenderer>())
        {
            Color tempCol = spr.color;
            tempCol.a = 1f;
            spr.color = tempCol;
        }
    }

    public void respawn()
    {
        //playerLives--;

        if (playerLives > 0)
        {
            playerLives--;
            respawning = true;
        }
        else
        {
            pauseTimer = true;
            playerLives = 0;
            //GAME OVER
        }
    }

    
    void OnGUI()
    {
        //GUI.Box(new Rect(Screen.width * .15f, Screen.height * .96f, Screen.width / 4 , 20), "100"+ "/" +"100");
        //GUI.DrawTexture(new Rect(Screen.width * .15f, Screen.height * .96f, Screen.width / 4, 20), true, 1);// "100" + "/" + "100");
        //GUI.DrawTexture(new Rect(10, 10, 60, 60), aTexture, ScaleMode.ScaleToFit, true, 10.0F);
        //GUI.DrawTexture(new Rect(Screen.width * .15f, Screen.height * .96f, Screen.width / 4, 20), barBackTexture); //ScaleMode.ScaleToFit, true, 10.0F);
        //GUI.DrawTexture(new Rect(Screen.width * .15f, Screen.height * .96f, (Screen.width / 4) , 5), barFrontTexture); //ScaleMode.ScaleToFit, true, 10.0F);
    }
}

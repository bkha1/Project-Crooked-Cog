using UnityEngine;
using System.Collections;

public class StageStatsScript: MonoBehaviour {

    public static StageStatsScript Instance;

    public int parCombo = 1000;//points gained after a full combo
    public float parTime = 60;//seconds to beat to get S rank
    public int parDeath = 3;//deaths to avoid to get S rank

    public int highestCombo;
    public float stageTime;
    public int numOfDeaths;

    public int currentCombo;

    public float playerXP;
    //public int XPdecreaseRate = 5;
    private float perSecond = 0;
    public int nextLevel;
    public int baseXPReq = 50;
    public int levelValue = 1;

    public int playerLives = 3;

    public GUIText timeValueText;
    public GUIText livesValueText;
    public GUIText levelValueText;

    public GUITexture upgradeMeter;
    private int meterMaxWidth = 120;

    private bool pauseTimer;

    //public int parGoals = 3;
    //public int goalsAchieved = 0;
    private bool goalCheck = true;
    public int goalsLeft = 0;

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

        nextLevel = baseXPReq;

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

        if (perSecond >= .2f)//player will lose 5 percent of bar every second
        {
            playerXP -= ((float)nextLevel * .05f * .2f);
            perSecond = 0;
        }

        if (playerXP < 0)
        {
            playerXP = 0;
            currentCombo = 0;
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
                nextLevel = (int)(nextLevel * 1.8);//xp requirement increases by 1.8 times
            }
            else
            {
                int tempDiff = (int)playerXP - nextLevel;
                currentCombo += tempDiff;
                if (highestCombo < currentCombo)
                {
                    highestCombo = currentCombo;
                }
                playerXP = nextLevel;
            }
        }

        timeValueText.text = "Time:" + stageTime.ToString("0.##");
        livesValueText.text = "x" + playerLives.ToString();
        levelValueText.text = "Lvl:" + levelValue.ToString();

        if (respawning)
        {
            levelValue = 1;
            nextLevel = baseXPReq;
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
        /*
        if (goalsAchieved < 0)//if by some freak accident this happens; fix it
        {
            goalsAchieved = 0;
        }

        if(parGoals<=goalsAchieved)//PAR GOALS ACHIEVED
        {

        }*/

        if (goalsLeft <= 0 && goalCheck)
        {
            
            goalCheck = false;
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
            //transform.parent.gameObject.AddComponent<GameOverScript>();
            transform.gameObject.AddComponent<GameOverScript>();
            Screen.lockCursor = false;
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

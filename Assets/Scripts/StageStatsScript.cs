using UnityEngine;
using System.Collections;

public class StageStatsScript : MonoBehaviour
{

    public static StageStatsScript Instance;

    public int parCombo = 1000;//points gained after a full combo
    public float parTime = 60;//seconds to beat to get S rank
    public int parDeath = 3;//deaths to avoid to get S rank

    public int highestCombo;
    public float stageTime;
    public int numOfDeaths;
    public int totalScore;

    public int offenseRank;
    public int defenseRank;
    public int speedRank;

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
    void Start()
    {
        highestCombo = 0;
        stageTime = 0;
        numOfDeaths = 0;

        currentCombo = 0;
        playerXP = 0;
        perSecond = 0;
        totalScore = 0;

        nextLevel = baseXPReq;

        playerLives = 3;
        levelValue = 1;

        pauseTimer = false;

        respawnCooldown = respawnTime;
    }

    // Update is called once per frame
    void Update()
    {

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
                //destroy all enemy bullets and convert to points
                ShotScript[] shots = GameObject.FindObjectsOfType<ShotScript>();
                foreach (ShotScript enemyshot in shots)
                {
                   if (enemyshot.isEnemyShot)
                   {
                       totalScore += 100;
                       SpecialEffectsScript.Instance.playPointsGainText(enemyshot.transform.position, new Vector2(1, 1), 100);
                       Destroy(enemyshot.gameObject);
                   }
                }
                //end enemy shot purge

                SoundEffectsScript.Instance.playPowerUpSound1(1);
                levelValue++;
                playerXP = 0;
                nextLevel = (int)(nextLevel * 1.8);//xp requirement increases by 1.8 times
            }
            else
            {
                int tempDiff = (int)playerXP - nextLevel;
                totalScore += tempDiff;
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

        //respawning check
        if (respawning)
        {
            levelValue = 1;
            nextLevel = baseXPReq;
            playerXP = 0;
            currentCombo = 0;
            respawnCooldown -= Time.deltaTime;
            if (respawnCooldown <= 0)
            {
                respawnCooldown = respawnTime;
                respawning = false;
                StartCoroutine(respawnPlayer(Instantiate(player) as Transform));
            }
        }

        if (goalsLeft <= 0 && goalCheck)
        {
            //destroy all enemy bullets and convert to points
            ShotScript[] shots = GameObject.FindObjectsOfType<ShotScript>();
            foreach (ShotScript enemyshot in shots)
            {
                if (enemyshot.isEnemyShot)
                {
                    totalScore += 100;
                    SpecialEffectsScript.Instance.playPointsGainText(enemyshot.transform.position, new Vector2(1, 1), 100);
                    Destroy(enemyshot.gameObject);
                }
            }

            //calculate ranks
            //calculate combo ratio
            float tempRank = (float)highestCombo / (float)parCombo;
            if (tempRank >= 1)
            {
                offenseRank = 6;
            }
            else if (tempRank >= .9)
            {
                offenseRank = 5;
            }
            else if (tempRank >= .8)
            {
                offenseRank = 4;
            }
            else if (tempRank >= .7)
            {
                offenseRank = 3;
            }
            else if (tempRank >= .6)
            {
                offenseRank = 2;
            }
            else
            {
                offenseRank = 1;
            }

            //todo:should change this to livesused/playerlives ratio
            //calculate death ratio
            if (numOfDeaths <= 0)
            {
                defenseRank = 6;
            }
            else if (numOfDeaths <= 1)
            {
                defenseRank = 4;
            }
            else if (numOfDeaths <= 2)
            {
                defenseRank = 2;
            }
            else
            {
                defenseRank = 1;
            }

            //calculate time ratio
            tempRank = (float)stageTime / (float)parTime;
            if (tempRank <= 1)
            {
                speedRank = 6;
            }
            else if (tempRank <= 1.2)
            {
                speedRank = 5;
            }
            else if (tempRank <= 1.4)
            {
                speedRank = 4;
            }
            else if (tempRank <= 1.6)
            {
                speedRank = 3;
            }
            else if (tempRank <= 1.8)
            {
                speedRank = 2;
            }
            else
            {
                speedRank = 1;
            }

            transform.gameObject.AddComponent<GameOverScript>();
            pauseTimer = true;

            goalCheck = false;
        }
    }

    public Transform player;
    private float respawnCooldown;
    private float respawnTime = 1;
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
        //todo: get rid of all bullets in the scene upon respawn
        if (playerLives > 0)
        {
            numOfDeaths++;
            playerLives--;
            respawning = true;

            //destroy all enemy bullets and convert to points
            ShotScript[] shots = GameObject.FindObjectsOfType<ShotScript>();
            foreach (ShotScript enemyshot in shots)
            {
                if (enemyshot.isEnemyShot)
                {
                    SpecialEffectsScript.Instance.playHitEffectPrefab(enemyshot.transform.position, new Vector2(1, 1));
                    Destroy(enemyshot.gameObject);
                }
            }
            //end enemy shot purge
        }
        else
        {
            pauseTimer = true;
            playerLives = 0;
            //GAME OVER
            //transform.parent.gameObject.AddComponent<GameOverScript>();

            transform.gameObject.AddComponent<GameOverScript>();
            //Screen.lockCursor = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;




public struct PlayerStats{
    public float life;
    public float regLife;
    public float mana;
    public float regMana;
    public float velocity;
    public float weaponCost;
    public float rangeWeapon;
    public float dmgWeapon;
    public float rangeExplosion;
    public float strExplosion;
    public float cdExplosion;
    public float expMultiplier;
}

public struct CardData{
    int type;
    string title;
    string description;
}
public class GameController : MonoBehaviour
{

    public AudioSource SoundGlobalSource;
    public AudioClip bossFightSong;

    //PLayer Stats;
    public PlayerStats playerStats;
    PlayerStats playerStatsLvl;
    
    PlayerStats[] lvlStats = new PlayerStats[5];
    
    public MovePlayer playerMove;

    public static GameController gameController;

    public float bossLife=40*4;

    public bool isBossFight = false;

    public GameObject bossPart1;
    public GameObject bossPart2;
    public GameObject bossPart3;
    public GameObject bossPart4;

    public GameObject BossText;
    public GameObject BossBar;
    Image BossBarReal;
    public GameObject BossBarParent;


    #region UI
    public Text timerText;
    public Text lvlText;

    public Text debugText;

    public Image cdExplosionUI;

    public GameObject pauseUI;

    public GameObject gameOverUI;
    public Text gameOverUIText;

    public GameObject lvlUpUI;

    #endregion

    AudioSource audioSource;
    public AudioClip lvlUpSound;

    float time=0;

    // Enemy Prefabs
    public GameObject enemy1Prefab; 
    public GameObject enemy2Prefab; 
    public GameObject enemy3Prefab; 
    public GameObject enemy4Prefab; 
    public GameObject gigantEnemy1Prefab; 


    //Weapons
    public GameObject fov;
    Vector3 initialFovScale;
    private float mana = 10.0f; //This is the mana
    private float maxMana = 10.0f;
    public GameObject manaBar;
    private Image manaBarReal;

    private float expansionWeaponCD = 0.0f;
    public Animator expansionAnimator;
    public GameObject expansionWeapon;
    
    public float experience=0;
    public float experienceNextLevel = 2;

    public GameObject expBar;
    private Image expBarReal;
    [SerializeField] private float nextLevelFactor=1.2f;

    public List<SpawnAreaController> spawnAreaControllers;
    public int currentLevel=1;

    //Cards
    int Card1Type=0;
    int Card2Type=0;

    public Text Card1Title;
    public Text Card1Description;

    public Text Card2Title;
    public Text Card2Description;


    void StartBossFight(){
        isBossFight=true;
        bossPart1.SetActive(true);
        bossPart2.SetActive(true);
        bossPart3.SetActive(true);
        bossPart4.SetActive(true);

        BossBar.SetActive(true);
        BossText.SetActive(true);
        BossBarParent.SetActive(true);

        SoundGlobalSource.clip=bossFightSong;
        SoundGlobalSource.Play();

    }

       void Start()
    {

        gameController=this.GetComponent<GameController>();

        InvokeRepeating("CreateEnemy", 1, 2);
        Invoke("StartBossFight", 300);
        // InvokeRepeating("CreateEnemy", 1, 1);

        audioSource= GetComponent<AudioSource>();

        manaBarReal=manaBar.GetComponent<Image>();
        
        expBarReal= expBar.GetComponent<Image>();

        BossBarReal=BossBar.GetComponent<Image>();

        initialFovScale=fov.transform.localScale;

        //Initial Lvls
        playerStatsLvl.life = 0;
        playerStatsLvl.regLife = 0;
        playerStatsLvl.mana = 0;
        playerStatsLvl.regMana = 0;
        playerStatsLvl.velocity = 0;
        playerStatsLvl.rangeWeapon = 0;
        playerStatsLvl.dmgWeapon = 0;
        playerStatsLvl.rangeExplosion = 0;
        playerStatsLvl.strExplosion = 0;
        playerStatsLvl.cdExplosion = 0;
        playerStatsLvl.expMultiplier = 0;
        playerStatsLvl.weaponCost = 0;

        initializeLvlPlayerStats();

        updatePlayerStats();

    }

    // Update is called once per 55
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape)) PauseGame();

        debugText.text="Life:" + playerStats.life + "\nRegLife: " + playerStats.regLife + "\nMana: " + playerStats.mana + 
        "\nRegMana: " + playerStats.regMana + "\nVel: " + playerStats.velocity + 
        "\nRangeExp: " + playerStats.rangeExplosion + "\nRangeCone: " + playerStats.rangeWeapon + "\nDmg: " + playerStats.dmgWeapon + 
        "\nStrExplosion: " + playerStats.strExplosion + "\nCdExplosion: " + playerStats.cdExplosion + 
        "\nExpMultiplier: " + playerStats.expMultiplier + "\n";

        timer();

        if (fov.activeSelf && mana<=0) stopUsingFov();

        manaBarReal.fillAmount=mana/maxMana;
        expBarReal.fillAmount=experience/experienceNextLevel;

        BossBarReal.fillAmount=bossLife/(40*4);

        lvlText.text="Lvl. " + currentLevel;

        if (bossLife<=0){
            gameOverUI.SetActive(true);
            gameOverUIText.text = "Thanks for playing!"
        }
        
    }


    void PauseGame(){
        if (Time.timeScale==0){
            Time.timeScale=1;
            pauseUI.SetActive(false);
        }else{
            Time.timeScale=0;
            pauseUI.SetActive(true);
        }
    }

    //Se ejecuta cada instante de tiempo para hacer distintas cosas
    void timer(){
        if (expansionWeaponCD>0) {
            expansionWeaponCD-=Time.deltaTime;
            cdExplosionUI.fillAmount=(playerStats.cdExplosion-expansionWeaponCD)/playerStats.cdExplosion;
        }
        if (mana<maxMana && !fov.activeSelf) mana+=playerStats.regMana*Time.deltaTime;
        else if (fov.activeSelf) mana-=playerStats.weaponCost*Time.deltaTime;

        time+=Time.deltaTime;
        timerText.text= "" + ((int)time/60).ToString("00") + ":" + ((int)time%60).ToString("00");

    }

    void updatePlayerStats(){

        playerStats.life = lvlStats[(int)playerStatsLvl.life].life;
        playerStats.regLife = lvlStats[(int)playerStatsLvl.regLife].regLife;
        playerStats.mana = lvlStats[(int)playerStatsLvl.mana].mana;
        playerStats.regMana = lvlStats[(int)playerStatsLvl.regMana].regMana;
        playerStats.velocity = lvlStats[(int)playerStatsLvl.velocity ].velocity;
        playerStats.rangeWeapon = lvlStats[(int)playerStatsLvl.rangeWeapon].rangeWeapon;
        playerStats.dmgWeapon = lvlStats[(int)playerStatsLvl.dmgWeapon].dmgWeapon;
        playerStats.rangeExplosion = lvlStats[(int)playerStatsLvl.rangeExplosion].rangeExplosion;
        playerStats.strExplosion = lvlStats[(int)playerStatsLvl.strExplosion].strExplosion;
        playerStats.cdExplosion = lvlStats[(int)playerStatsLvl.cdExplosion].cdExplosion;
        playerStats.expMultiplier = lvlStats[(int)playerStatsLvl.expMultiplier ].expMultiplier;
        playerStats.weaponCost = lvlStats[(int)playerStatsLvl.weaponCost].weaponCost;

        fov.transform.localScale=initialFovScale*lvlStats[(int)playerStatsLvl.rangeWeapon].rangeWeapon;



        playerMove.life=playerStats.life;
        playerMove.maxLife=playerStats.life;
        playerMove.velocidad=playerStats.velocity;
        playerMove.regLife=playerStats.regLife;

        mana=playerStats.mana;
        maxMana=playerStats.mana;

        expansionWeaponCD=0;
        expansionWeapon.transform.localScale=new Vector3(playerStats.rangeExplosion, playerStats.rangeExplosion, playerStats.rangeExplosion);



    }

    void CreateEnemy(){
        if (isBossFight) return;


        int spanwSel=Random.Range(0, spawnAreaControllers.Count);
        int enemyType = ((int)time / 45) % 5;

        GameObject enemyPrefab=null;


        switch(enemyType){
            case 0: 
                enemyPrefab=enemy1Prefab;
                break;
            case 1:
                enemyPrefab = enemy2Prefab;
                break;
            case 2:
                enemyPrefab = enemy3Prefab;
                break;
            case 3:
                enemyPrefab = enemy4Prefab;
                break;
            case 4:
                enemyPrefab = gigantEnemy1Prefab;
                break;
        }
        // enemyPrefab=enemy4Prefab;

        if (spanwSel>=0)
            spawnAreaControllers[spanwSel].SpawnEnemy(enemyPrefab);

        if (time>225){

             switch(enemyType){
            case 0: 
                enemyPrefab=enemy1Prefab;
                break;
            case 1:
                enemyPrefab = enemy2Prefab;
                break;
            case 2:
                enemyPrefab = enemy3Prefab;
                break;
            case 3:
                enemyPrefab = enemy4Prefab;
                break;
            case 4:
                enemyPrefab = gigantEnemy1Prefab;
                break;
        }
            spanwSel=Random.Range(0, spawnAreaControllers.Count);
            enemyType = Random.Range(0, 5);
            spawnAreaControllers[spanwSel].SpawnEnemy(enemyPrefab);

             switch(enemyType){
            case 0: 
                enemyPrefab=enemy1Prefab;
                break;
            case 1:
                enemyPrefab = enemy2Prefab;
                break;
            case 2:
                enemyPrefab = enemy3Prefab;
                break;
            case 3:
                enemyPrefab = enemy4Prefab;
                break;
            case 4:
                enemyPrefab = gigantEnemy1Prefab;
                break;
        }
            spanwSel=Random.Range(0, spawnAreaControllers.Count);
            enemyType = Random.Range(0, 5);
            spawnAreaControllers[spanwSel].SpawnEnemy(enemyPrefab);
        }
    }

    void deactiveExpansionWeapon(){
        expansionWeapon.SetActive(false);
    }
    public void useStick(ref bool result){
        
        result=expansionWeaponCD<=0 && fov != null;

        if (expansionWeaponCD<=0 && fov != null){
            expansionWeapon.SetActive(true);
            expansionWeaponCD=playerStats.cdExplosion;
            Invoke("deactiveExpansionWeapon", 0.4f);
            expansionAnimator.SetTrigger("cast_expansion");
        }
    }

    public void useFov(){
        if (mana>0.1 && fov != null){
            fov.SetActive(true);
            
        }
    }

    public void stopUsingFov(){
        if (fov != null)
            fov.SetActive(false);        
    }

    public void addExperience(float value){

        float sumLvl = playerStatsLvl.life +
        playerStatsLvl.regLife +
        playerStatsLvl.mana +
        playerStatsLvl.regMana +
        playerStatsLvl.velocity +
        playerStatsLvl.rangeWeapon +
        playerStatsLvl.dmgWeapon +
        playerStatsLvl.rangeExplosion +
        playerStatsLvl.strExplosion +
        playerStatsLvl.cdExplosion +
        playerStatsLvl.expMultiplier +
        playerStatsLvl.weaponCost;

        if (sumLvl/4==12) {//Guarda para evitar el freeze
            experience=experienceNextLevel;
            return;
        }

        experience+=value*playerStats.expMultiplier;
        if (experienceNextLevel<=experience){
            //TODO: LevelUp Animation

            levelUp();

            currentLevel++;
            experience=0;
            experienceNextLevel = experienceNextLevel*nextLevelFactor;

            audioSource.pitch=0.7f;
            audioSource.PlayOneShot(lvlUpSound);

            lvlUpUI.SetActive(true);
            Time.timeScale=0;

        }
    }

    public void levelUp(){

        do{
            Card1Type = Random.Range(0,11);
        }while(getStatLvlByType(Card1Type)==4);

        int attempt=0;
        do{
            Card2Type = Random.Range(0,11);
            attempt++;
        }while((getStatLvlByType(Card2Type)==4 || Card2Type==Card1Type) && attempt<=10);

        Card1Title.text=getCardTitle(Card1Type);
        Card1Description.text=getCardDescription(Card1Type);

        Card2Title.text=getCardTitle(Card2Type);
        Card2Description.text=getCardDescription(Card2Type);
    
    }

    void initializeLvlPlayerStats(){
        //Initial Values - Lvl1
        lvlStats[0].life = 5;
        lvlStats[0].regLife = 0.02f;
        lvlStats[0].mana = 3;
        lvlStats[0].regMana = 0.4f;
        lvlStats[0].velocity = 3;
        lvlStats[0].rangeWeapon = 1;
        lvlStats[0].dmgWeapon = 2;
        lvlStats[0].rangeExplosion = 200;
        lvlStats[0].strExplosion = 8;
        lvlStats[0].cdExplosion = 10;
        lvlStats[0].expMultiplier = 1;
        lvlStats[0].weaponCost = 0.5f;

        //Lvl 2
        lvlStats[1].life = 7;
        lvlStats[1].regLife = 0.04f;
        lvlStats[1].mana = 4;
        lvlStats[1].regMana = 0.5f;
        lvlStats[1].velocity = 3.75f;
        lvlStats[1].rangeWeapon = 1.1f;//TODO: Arreglar esto
        lvlStats[1].dmgWeapon = 2.5f;
        lvlStats[1].rangeExplosion = 220;
        lvlStats[1].strExplosion = 8.5f;
        lvlStats[1].cdExplosion = 7;
        lvlStats[1].expMultiplier = 1.2f;
        lvlStats[1].weaponCost = 0.4f;

        //Lvl 3
        lvlStats[2].life = 9;
        lvlStats[2].regLife = 0.06f;
        lvlStats[2].mana = 5;
        lvlStats[2].regMana = 0.7f;
        lvlStats[2].velocity = 4.5f;
        lvlStats[2].rangeWeapon = 1.2f; //TODO: Arreglar esto
        lvlStats[2].dmgWeapon = 3f;
        lvlStats[2].rangeExplosion = 240;
        lvlStats[2].strExplosion = 9f;
        lvlStats[2].cdExplosion = 6;
        lvlStats[2].expMultiplier = 1.4f;
        lvlStats[2].weaponCost = 0.3f;

        //Lvl 4
        lvlStats[3].life = 12;
        lvlStats[3].regLife = 0.08f;
        lvlStats[3].mana = 5.5f;
        lvlStats[3].regMana = 0.8f;
        lvlStats[3].velocity = 5.25f;
        lvlStats[3].rangeWeapon = 1.4f; //TODO: Arreglar esto
        lvlStats[3].dmgWeapon = 3.5f;
        lvlStats[3].rangeExplosion = 260;
        lvlStats[3].strExplosion = 9.3f;
        lvlStats[3].cdExplosion = 5;
        lvlStats[3].expMultiplier = 1.6f;
        lvlStats[3].weaponCost = 0.2f;

        //Lvl 5 - Max LVL
        lvlStats[4].life = 15;
        lvlStats[4].regLife = 0.1f;
        lvlStats[4].mana = 6f;
        lvlStats[4].regMana = 0.9f;
        lvlStats[4].velocity = 6f;
        lvlStats[4].rangeWeapon = 1.6f; //TODO: Arreglar esto
        lvlStats[4].dmgWeapon = 4f;
        lvlStats[4].rangeExplosion = 280;
        lvlStats[4].strExplosion = 10f;
        lvlStats[4].cdExplosion = 2;
        lvlStats[4].expMultiplier = 2f;
        lvlStats[4].weaponCost = 0.1f;
    }

    public void GameOver(){
        //TODO: GAME OVER LOGIC

        gameOverUI.SetActive(true);
    }

    string getCardTitle(int type){ 
        
        switch (type){
            case 0:
                return "Life++";
            
           case 1:
                return "Reg.Life++";
            
            case 2:
                return "Mana++";
            
            case 3:
                return "Reg.Mana++";
            
            case 4:
                return "Vel++";
            
            case 5:
                return "Range Magic++";
            
            case 6:
                return "Magic Power++"; //dmgWeapon
            
            case 7:
                return "Range Explosion++";
            
            case 8:
                return "CD Explosion--";
            
            case 9:
                return "Str Explosion++";
            
            case 10:
                return "Experience Multiplier";
            
            case 11:
                return "Magic Cost--";
            
        }


        return "";
    }

    //The playerStatsLvl should be less than 5 the maximum.
    string getCardDescription(int type){ 
        
        switch (type){
            case 0:
                // return "Life: \n+" + (lvlStats[(int)playerStatsLvl.life+1].life - playerStats.life);
                return "Life: \n+" + (int)(lvlStats[(int)playerStatsLvl.life+1].life*100/playerStats.life-100) + "%";
            
           case 1:
                // return "Reg.Life: \n+" + (lvlStats[(int)playerStatsLvl.regLife+1].regLife - playerStats.regLife);
                return "Reg.Life: \n+" + (int)(lvlStats[(int)playerStatsLvl.regLife+1].regLife*100/playerStats.regLife-100) + "%";
            
            case 2:
                // return "Mana: \n+" + (lvlStats[(int)playerStatsLvl.mana+1].mana - playerStats.mana);
                return "Mana: \n+" + (int)(lvlStats[(int)playerStatsLvl.mana+1].mana*100/playerStats.mana-100) + "%";
            
            case 3:
                // return "Reg.Mana: \n+" + (lvlStats[(int)playerStatsLvl.regMana+1].regMana - playerStats.regMana);;
                return "Reg.Mana: \n+" + (int)(lvlStats[(int)playerStatsLvl.regMana+1].regMana*100/playerStats.regMana-100) + "%";
            
            case 4:
                // return "Velocity: \n+" + (lvlStats[(int)playerStatsLvl.velocity+1].velocity - playerStats.velocity);
                return "Velocity: \n+" + (int)(lvlStats[(int)playerStatsLvl.velocity+1].velocity*100/playerStats.velocity-100) + "%";
            
            case 5:
                // return "Range Magic: \n+" + (lvlStats[(int)playerStatsLvl.rangeWeapon+1].rangeWeapon - playerStats.rangeWeapon);
                return "Range Magic: \n+" + (int)(lvlStats[(int)playerStatsLvl.rangeWeapon+1].rangeWeapon*100/playerStats.rangeWeapon-100) + "%";
            
            case 6:
                // return "Magic Power: \n+" + (lvlStats[(int)playerStatsLvl.dmgWeapon+1].dmgWeapon - playerStats.dmgWeapon);
                return "Magic Power: \n+" + (int)(lvlStats[(int)playerStatsLvl.dmgWeapon+1].dmgWeapon*100/playerStats.dmgWeapon-100) + "%";
            
            case 7:
                // return "Range Explosion: \n+" + (lvlStats[(int)playerStatsLvl.rangeExplosion+1].rangeExplosion - playerStats.rangeExplosion);
                return "Range Explosion: \n+" + (int)(lvlStats[(int)playerStatsLvl.rangeExplosion+1].rangeExplosion*100/playerStats.rangeExplosion-100) + "%";
            
            case 8:
                // return "CD Explosion: \n-" + (playerStats.cdExplosion-lvlStats[(int)playerStatsLvl.cdExplosion+1].cdExplosion);
                return "CD Explosion: \n-" + (int)(playerStats.cdExplosion*100/lvlStats[(int)playerStatsLvl.cdExplosion+1].cdExplosion-100) + "%";
            
            case 9:
                // return "Str Explosion: \n+" + (lvlStats[(int)playerStatsLvl.velocity+1].velocity - playerStats.velocity);
                return "Str Explosion: \n+" + (int)(lvlStats[(int)playerStatsLvl.velocity+1].velocity*100/playerStats.velocity-100) + "%";
            
            case 10:
                // return "Experience Multiplier: \n+" + (lvlStats[(int)playerStatsLvl.expMultiplier+1].expMultiplier - playerStats.expMultiplier);
                return "Experience Multiplier: \n+" + (int)(lvlStats[(int)playerStatsLvl.expMultiplier+1].expMultiplier*100/playerStats.expMultiplier-100) + "%";
            
            
            case 11:
                // return "Magic Cost: \n-" + (playerStats.weaponCost-lvlStats[(int)playerStatsLvl.weaponCost+1].weaponCost);
                return "Magic Cost: \n-" + (int)(playerStats.weaponCost*100/lvlStats[(int)playerStatsLvl.weaponCost+1].weaponCost-100) + "%";
            
        }


        return "";
    }

    float getStatLvlByType(int type){ 
        
        switch (type){
            case 0:
                return playerStatsLvl.life;
            
           case 1:
                return playerStatsLvl.regLife;
            
            case 2:
                return playerStatsLvl.mana;
            
            case 3:
                return playerStatsLvl.regMana;
            
            case 4:
                return playerStatsLvl.velocity;
            
            case 5:
                return playerStatsLvl.rangeWeapon;
            
            case 6:
                return playerStatsLvl.dmgWeapon;
            
            case 7:
                return playerStatsLvl.rangeExplosion;
            
            case 8:
                return playerStatsLvl.cdExplosion;
            
            case 9:
                return playerStatsLvl.strExplosion;
            
            case 10:
                return playerStatsLvl.expMultiplier;
            
            case 11:
                return playerStatsLvl.weaponCost;
            
        }


        return -1;
    }

    public void selectCard(int cardNum){
        if (cardNum==1)
            statUpByType(Card1Type);
        else
            statUpByType(Card2Type);
    }
    void statUpByType(int type){
        switch (type){
            case 0:
                playerStatsLvl.life++;
            break;
           case 1:
                playerStatsLvl.regLife++;
            break;
            case 2:
                playerStatsLvl.mana++;
            break;
            case 3:
                playerStatsLvl.regMana++;
            break;
            case 4:
                playerStatsLvl.velocity++;
            break;
            case 5:
                playerStatsLvl.rangeWeapon++;
            break;
            case 6:
                playerStatsLvl.dmgWeapon++;
            break;
            case 7:
                playerStatsLvl.rangeExplosion++;
            break;
            case 8:
                playerStatsLvl.cdExplosion++;
            break;
            case 9:
                playerStatsLvl.strExplosion++;
            break;
            case 10:
                playerStatsLvl.expMultiplier++;
            break;
            case 11:
                playerStatsLvl.weaponCost++;
            break;
        }
    
        updatePlayerStats();

        lvlUpUI.SetActive(false);
        Time.timeScale=1;
    
    }


}

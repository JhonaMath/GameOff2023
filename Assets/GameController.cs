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
    public float costExplosion;
    public float expMultiplier;
}





public class GameController : MonoBehaviour
{

    //PLayer Stats;
    public PlayerStats playerStats;
    PlayerStats playerStatsLvl;
    
    PlayerStats[] lvlStats = new PlayerStats[10];
    
    public MovePlayer playerMove;

    public static GameController gameController;

    //UI
    public Text timerText;
    public Text lvlText;

    public Text debugText;

    public Image cdExplosionUI;

    public GameObject pauseUI;


    float time=0;

    // Enemy Prefabs
    public GameObject enemy1Prefab; 


    //Weapons
    public GameObject fov;
    private float mana = 10.0f; //This is the mana
    private float maxMana = 10.0f;
    public GameObject manaBar;
    private Image manaBarReal;

    private float expansionWeaponCD = 0.0f;
    public Animator expansionAnimator;
    public GameObject expansionWeapon;
    
    public float experience=0;
    public float experienceNextLevel = 10;

    public GameObject expBar;
    private Image expBarReal;
    [SerializeField] private float nextLevelFactor=1.2f;

    public List<SpawnAreaController> spawnAreaControllers;
    public int currentLevel=1;


       void Start()
    {
        gameController=this.GetComponent<GameController>();

        InvokeRepeating("CreateEnemy", 1, 2);
        // InvokeRepeating("CreateEnemy", 1, 1);

        manaBarReal=manaBar.GetComponent<Image>();
        
        expBarReal= expBar.GetComponent<Image>();

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
        playerStatsLvl.costExplosion = 0;
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
        "\nStrExplosion: " + playerStats.strExplosion + "\nCdExplosion: " + playerStats.cdExplosion + "\nCostExplosion: " + playerStats.costExplosion + 
        "\nExpMultiplier: " + playerStats.expMultiplier + "\n";

        timer();

        if (fov.activeSelf && mana<=0) stopUsingFov();

        manaBarReal.fillAmount=mana/maxMana;
        expBarReal.fillAmount=experience/experienceNextLevel;
        lvlText.text="Lvl. " + currentLevel;
        
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
        playerStats.regLife = lvlStats[(int)playerStats.regLife].regLife;
        playerStats.mana = lvlStats[(int)playerStats.mana].mana;
        playerStats.regMana = lvlStats[(int)playerStats.regMana].regMana;
        playerStats.velocity = lvlStats[(int)playerStats.velocity ].velocity;
        playerStats.rangeWeapon = lvlStats[(int)playerStats.rangeWeapon].rangeWeapon;
        playerStats.dmgWeapon = lvlStats[(int)playerStats.dmgWeapon].dmgWeapon;
        playerStats.rangeExplosion = lvlStats[(int)playerStats.rangeExplosion].rangeExplosion;
        playerStats.strExplosion = lvlStats[(int)playerStats.strExplosion].strExplosion;
        playerStats.cdExplosion = lvlStats[(int)playerStats.cdExplosion].cdExplosion;
        playerStats.costExplosion = lvlStats[(int)playerStats.costExplosion].costExplosion;
        playerStats.expMultiplier = lvlStats[(int)playerStats.expMultiplier ].expMultiplier;
        playerStats.weaponCost = lvlStats[(int)playerStats.weaponCost].weaponCost;



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

        int spanwSel=Random.Range(0, spawnAreaControllers.Count);

        if (spanwSel>=0)
            spawnAreaControllers[spanwSel].SpawnEnemy(enemy1Prefab);
    }

    void deactiveExpansionWeapon(){
        expansionWeapon.SetActive(false);
    }
    public void useStick(){
        if (expansionWeaponCD<=0 && mana>=playerStats.costExplosion && fov != null){
            expansionWeapon.SetActive(true);
            expansionWeaponCD=playerStats.cdExplosion;
            mana-=playerStats.costExplosion;
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
        experience+=value*playerStats.expMultiplier;
        if (experienceNextLevel<=experience){
            //TODO: LevelUp Animation

            Debug.Log("LEVEL UP!");

            currentLevel++;
            experience=0;
            experienceNextLevel = experienceNextLevel*nextLevelFactor;
        }
    }

    void initializeLvlPlayerStats(){
        //Initial Values - Lvl1
        lvlStats[0].life = 5;
        lvlStats[0].regLife = 0.01f;
        lvlStats[0].mana = 3;
        lvlStats[0].regMana = 0.1f;
        lvlStats[0].velocity = 3;
        lvlStats[0].rangeWeapon = 1;
        lvlStats[0].dmgWeapon = 2;
        lvlStats[0].rangeExplosion = 200;
        lvlStats[0].strExplosion = 8;
        lvlStats[0].cdExplosion = 10;
        lvlStats[0].costExplosion = 2;
        lvlStats[0].expMultiplier = 1;
        lvlStats[0].weaponCost = 0.5f;

        //Lvl 2
        lvlStats[1].life = 7;
        lvlStats[1].regLife = 0.02f;
        lvlStats[1].mana = 4;
        lvlStats[1].regMana = 0.2f;
        lvlStats[1].velocity = 3.5f;
        lvlStats[1].rangeWeapon = 1;//TODO: Arreglar esto
        lvlStats[1].dmgWeapon = 2.2f;
        lvlStats[1].rangeExplosion = 220;
        lvlStats[1].strExplosion = 8.5f;
        lvlStats[1].cdExplosion = 7;
        lvlStats[1].costExplosion = 1.6f;
        lvlStats[1].expMultiplier = 1.2f;
        lvlStats[1].weaponCost = 0.4f;

        //Lvl 3
        lvlStats[2].life = 9;
        lvlStats[2].regLife = 0.03f;
        lvlStats[2].mana = 5;
        lvlStats[2].regMana = 0.3f;
        lvlStats[2].velocity = 4;
        lvlStats[2].rangeWeapon = 1; //TODO: Arreglar esto
        lvlStats[2].dmgWeapon = 2.5f;
        lvlStats[2].rangeExplosion = 240;
        lvlStats[2].strExplosion = 9f;
        lvlStats[2].cdExplosion = 6;
        lvlStats[2].costExplosion = 1.1f;
        lvlStats[2].expMultiplier = 1.4f;
        lvlStats[2].weaponCost = 0.3f;

        //Lvl 4
        lvlStats[3].life = 12;
        lvlStats[3].regLife = 0.04f;
        lvlStats[3].mana = 5.5f;
        lvlStats[3].regMana = 0.35f;
        lvlStats[3].velocity = 4.5f;
        lvlStats[3].rangeWeapon = 1; //TODO: Arreglar esto
        lvlStats[3].dmgWeapon = 2.7f;
        lvlStats[3].rangeExplosion = 260;
        lvlStats[3].strExplosion = 9.3f;
        lvlStats[3].cdExplosion = 5;
        lvlStats[3].costExplosion = 0.9f;
        lvlStats[3].expMultiplier = 1.6f;
        lvlStats[3].weaponCost = 0.2f;

        //Lvl 5 - Max LVL
        lvlStats[4].life = 15;
        lvlStats[4].regLife = 0.06f;
        lvlStats[4].mana = 6f;
        lvlStats[4].regMana = 0.5f;
        lvlStats[4].velocity = 5f;
        lvlStats[4].rangeWeapon = 1; //TODO: Arreglar esto
        lvlStats[4].dmgWeapon = 3f;
        lvlStats[4].rangeExplosion = 280;
        lvlStats[4].strExplosion = 10f;
        lvlStats[4].cdExplosion = 3;
        lvlStats[4].costExplosion = 0.5f;
        lvlStats[4].expMultiplier = 2f;
        lvlStats[4].weaponCost = 0.1f;

        
    }
}

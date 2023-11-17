using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class GameController : MonoBehaviour
{

    public static GameController gameController;

    //UI
    public Text timerText;
    public Text lvlText;

    float time=0;

    // Enemy Prefabs
    public GameObject enemy1Prefab; 


    //Weapons
    public GameObject fov;
    private float fovRemainTime = 10.0f;
    private float maxRemaininTime = 10.0f;
    public GameObject manaBar;
    private Image manaBarReal;



    public GameObject stick;
    private float stickCD = 0.0f;


    public float weaponDamage=2;
    
    public float experience=0;
    public float experienceNextLevel = 10;

    public GameObject expBar;
    private Image expBarReal;
    [SerializeField] private float nextLevelFactor=1.2f;

    public List<SpawnAreaController> spawnAreaControllers;
    public int currentLevel=1;


    //Se ejecuta cada instante de tiempo para hacer distintas cosas
    void timer(){
        if (stickCD>0) stickCD-=Time.deltaTime;
        if (fovRemainTime<maxRemaininTime && !fov.activeSelf) fovRemainTime+=Time.deltaTime;
        else if (fov.activeSelf) fovRemainTime-=Time.deltaTime;

        time+=Time.deltaTime;
        timerText.text= "" + ((int)time/60).ToString("00") + ":" + ((int)time%60).ToString("00");

    }

    void CreateEnemy(){

        int spanwSel=Random.Range(0, spawnAreaControllers.Count);

        if (spanwSel>=0)
            spawnAreaControllers[spanwSel].SpawnEnemy(enemy1Prefab);
    }

    void deactiveStick(){
        stick.SetActive(false);
    }
    public void useStick(){
        if (stickCD<=0 && fov != null){
            stick.SetActive(true);
            stickCD=3;
            Invoke("deactiveStick", 0.2f);
        }
    }



    public void useFov(){
        if (fovRemainTime>0.2 && fov != null){
            fov.SetActive(true);
            
        }
    }

    public void stopUsingFov(){
        if (fov != null)
            fov.SetActive(false);        
    }

    public void addExperience(float value){
        experience+=value;
        if (experienceNextLevel<=experience){
            //TODO: LevelUp Animation

            Debug.Log("LEVEL UP!");

            currentLevel++;
            experience=0;
            experienceNextLevel = experienceNextLevel*nextLevelFactor;
        }
    }

    void Start()
    {
        gameController=this.GetComponent<GameController>();

        InvokeRepeating("CreateEnemy", 1, 2);
        // InvokeRepeating("CreateEnemy", 1, 1);

        manaBarReal=manaBar.GetComponent<Image>();
        
        expBarReal= expBar.GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        timer();

        if (fov.activeSelf && fovRemainTime<=0) stopUsingFov();

        manaBarReal.fillAmount=fovRemainTime/maxRemaininTime;
        expBarReal.fillAmount=experience/experienceNextLevel;
        lvlText.text="Lvl. " + currentLevel;
        
    }
}

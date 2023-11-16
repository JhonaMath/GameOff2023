using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class GameController : MonoBehaviour
{

    public static GameController gameController;
    // Enemy Prefabs
    public GameObject enemy1Prefab;


    //Weapons
    public GameObject fov;
    private float fovRemainTime = 10.0f;

    public GameObject stick;
    private float stickCD = 0.0f;


    public float weaponDamage=2;
    
    public float experience=0;
    public float experienceNextLevel = 10;
    [SerializeField] private float nextLevelFactor=1.2f;

    public List<SpawnAreaController> spawnAreaControllers;
    public int currentLevel=1;


    //Se ejecuta cada instante de tiempo para hacer distintas cosas
    void timer(){
        if (stickCD>0) stickCD-=Time.deltaTime;
        if (fovRemainTime<10 && !fov.activeSelf) fovRemainTime+=Time.deltaTime;
        else if (fov.activeSelf) fovRemainTime-=Time.deltaTime;
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

    }

    // Update is called once per frame
    void Update()
    {
        timer();

        if (fov.activeSelf && fovRemainTime<=0) stopUsingFov();
        Debug.Log(nextLevelFactor);
    }
}

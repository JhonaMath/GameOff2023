using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class GameController : MonoBehaviour
{

    public static GameController gameController = new GameController();
    public GameObject enemy1Prefab;
    public float weaponDamage=2;
    
    public float experience=0;
    public float experienceNextLevel = 10;
    [SerializeField] private float nextLevelFactor=1.2f;

    public List<SpawnAreaController> spawnAreaControllers;
    public int currentLevel=1;

    void CreateEnemy(){

        int spanwSel=Random.Range(0, spawnAreaControllers.Count);

        if (spanwSel>=0)
            spawnAreaControllers[spanwSel].SpawnEnemy(enemy1Prefab);
    }

    public void addExperience(float value){
        experience+=value;
        if (experienceNextLevel<=experience){
            //TODO: LevelUp Animation

            Debug.Log("LEVEL UP!");

            currentLevel++;
            experience=0;
            experienceNextLevel=experienceNextLevel*nextLevelFactor;
        }
    }

    void Start()
    {
        InvokeRepeating("CreateEnemy", 1, 1);
        InvokeRepeating("CreateEnemy", 1, 1);

    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}

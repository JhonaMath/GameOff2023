using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject enemy1Prefab;
    
    void CreateEnemy(){
        GameObject newObj = Instantiate(enemy1Prefab);
        newObj.transform.position = new Vector3(5,5);
        Debug.Log("Crear Enemy");
    }

    void Start()
    {
        InvokeRepeating("CreateEnemy", 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}

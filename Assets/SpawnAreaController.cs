using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnAreaController : MonoBehaviour
{

    public float width=1;
    public float height=1;

    public int fromLevel = 0;
    public int toLevel = 2;

    public int timeToSpawnMs = 1000;
    public int howManyEnemyAtSameTime=1;

    public void SpawnEnemy(GameObject enemy){

        Vector3 spawnPos = this.transform.position;

        GameObject newObj = Instantiate(enemy);

        float x_pos=Random.Range(-width/2, width/2);
        float y_pos=Random.Range(-height/2, height/2);


        newObj.transform.position = new Vector3(spawnPos.x + x_pos, spawnPos.y + y_pos);
    } 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnDrawGizmos(){
        Vector3 pos = transform.position;
        Gizmos.DrawLine(new Vector3(pos.x+width/2, pos.y+height/2),new Vector3(pos.x+width/2, pos.y-height/2));
        Gizmos.DrawLine(new Vector3(pos.x+width/2, pos.y-height/2),new Vector3(pos.x-width/2, pos.y-height/2));
        Gizmos.DrawLine(new Vector3(pos.x-width/2, pos.y-height/2),new Vector3(pos.x-width/2, pos.y+height/2));
        Gizmos.DrawLine(new Vector3(pos.x-width/2, pos.y+height/2),new Vector3(pos.x+width/2, pos.y+height/2));
    }

}

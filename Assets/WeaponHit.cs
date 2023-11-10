using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponHit : MonoBehaviour
{   

    public float hitPointsPerHit = 2f;

    private List<GameObject> enemyList=new List<GameObject>();

    private bool shouldHit = true;

    private void setShouldHitTrue(){
        shouldHit=true;
    }

    void Update(){
        if (shouldHit){
            foreach (var go in enemyList)
            {
                EnemyBehaiviour enemyB= go.GetComponent<EnemyBehaiviour>();
                if (enemyB.hp-hitPointsPerHit*Time.deltaTime<=0) enemyList.Remove(go);
                enemyB.hitEnemy(hitPointsPerHit * Time.deltaTime);
            }

            // shouldHit=false;
            // Invoke("setShouldHitTrue", 1);
        }
    }

    void OnTriggerEnter2D(Collider2D col){

        if (col.gameObject.tag=="Enemy")
            enemyList.Add(col.gameObject);
    }

    void OnTriggerExit2D(Collider2D col){
        if (col.gameObject.tag=="Enemy")
            enemyList.Remove(col.gameObject);
    }
    
}
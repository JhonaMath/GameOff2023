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
        transform.localPosition= new Vector3(3,0,0);
    }

    // void OnTriggerEnter2D(Collider2D col){

    //     if (col.gameObject.tag=="Enemy")
    //         enemyList.Add(col.gameObject);
    // }

    // void OnTriggerExit2D(Collider2D col){
    //     if (col.gameObject.tag=="Enemy")
    //         enemyList.Remove(col.gameObject);
    // }
    
}
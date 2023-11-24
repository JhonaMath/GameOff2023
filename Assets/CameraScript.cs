using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameObject player;

    public float maxX=16;
    public float maxY=9;
    // Start is called before the first frame update
    void Start()
    {
       player=GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 p_pos=player.transform.position;

        float x_pos = (Mathf.Abs(p_pos.x) < maxX) ? p_pos.x:transform.position.x;//Mathf.Min(Mathf.Max(-maxX,p_pos.x),maxX);
        float y_pos = (Mathf.Abs(p_pos.y) < maxY) ? p_pos.y:transform.position.y;//Mathf.Min(Mathf.Max(-maxY,p_pos.y),maxY);

        transform.position= new Vector3(x_pos,y_pos,transform.position.z); 
    }
}

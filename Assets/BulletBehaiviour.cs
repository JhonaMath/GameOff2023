using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletPattern{
    Line,
    ZigZag
}
public class BulletBehaiviour : MonoBehaviour
{
    // Start is called before the first frame update
    public BulletPattern pattern;

    Rigidbody2D rb;
    Vector2 initVel;

    public float frequency = 2f;
    public float amplitude = 2f;

    public float timer=0;

    public float ttl = 3;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timer+=Time.deltaTime;

        if (timer>=ttl) Destroy(this.gameObject);
        // if (pattern==BulletPattern.ZigZag){
        //     if (initVel==null) initVel = rb.velocity;

        //     rb.velocity= initVel * Mathf.Sin(frequency * Time.deltaTime)*amplitude;
        // }


    }

    void OnTriggerEnter2D(Collider2D col){
        // Debug.Log("CHOQUEEE");
        if (col.gameObject.tag=="Expansive") Destroy(this.gameObject);
    }
}

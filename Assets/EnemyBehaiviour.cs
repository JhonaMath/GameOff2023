using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehaiviour : MonoBehaviour
{
    public float velocidad = 3.0f; // Velocidad de movimiento del enemigo

    // public float lifePerSecond = 0.5f;
    public float hp=10;
    public float minimumHp = 1;
    
    public float experienceByDeath=1;

    public float shrinkFactor = 1f;

    private Transform jugador; // Referencia al jugador\



    bool isStuned = false;

    Rigidbody2D rb;
    SpriteRenderer sprite;

    public void hitEnemy(float ammount){
        if (minimumHp<=hp){
            hp-=ammount;
            transform.localScale -= new Vector3(ammount, ammount) *shrinkFactor;
        }
    }

    // void addLife(){
    //     hp+=1;
    // ÿ

    void Start()
    {
        // Obtener la referencia al jugador (asegúrate de configurarla en el Inspector)
        jugador = GameObject.FindWithTag("Player").transform;

        rb = this.GetComponent<Rigidbody2D>();

        sprite= this.GetComponentsInChildren<SpriteRenderer>()[0];
        
        // InvokeRepeating("addLife", 0.5f, 0.5f);
    }

    void Update()
    {
        // float scale = hp;

        // transform.localScale = new Vector3(scale, scale);
        
        if (jugador != null && !isStuned)
        {
            Vector2 direccion;

            if (hp<=minimumHp)
                direccion = (-jugador.position + transform.position).normalized;
            else 
                direccion = (jugador.position - transform.position).normalized;           
            
            // Mover al enemigo hacia el jugador o alejarlo del jugador 
            transform.Translate(direccion * velocidad * Time.deltaTime);

            if (direccion.x<0) sprite.flipX=true;
            else sprite.flipX=false;
            // rb.velocity+=direccion * velocidad;

        }

        if (rb.velocity.sqrMagnitude!=0){
            rb.velocity-= rb.velocity*Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.tag=="Player" && hp<=minimumHp){
            GameController.gameController.addExperience(experienceByDeath);
            Destroy(this.gameObject);
        }

        
    }

    void OnTriggerStay2D(Collider2D col){
        Debug.Log(GameController.gameController.weaponDamage);
        

        if (col.gameObject.tag=="Weapon")
            this.hitEnemy(GameController.gameController.weaponDamage * Time.deltaTime);
    }
}
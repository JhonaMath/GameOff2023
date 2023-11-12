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

    private Transform jugador; // Referencia al jugador

    public void hitEnemy(float ammount){
        if (minimumHp<=hp)
            hp-=ammount;
    }

    // void addLife(){
    //     hp+=1;
    // }

    void Start()
    {
        // Obtener la referencia al jugador (asegúrate de configurarla en el Inspector)
        jugador = GameObject.FindWithTag("Player").transform;

        // InvokeRepeating("addLife", 0.5f, 0.5f);
    }

    void Update()
    {
        float scale = hp;

        transform.localScale = new Vector3(scale, scale);
        
        if (jugador != null)
        {
            Vector3 direccion;

            if (hp<=minimumHp)
                direccion = (-jugador.position + transform.position).normalized;
            else 
                direccion = (jugador.position - transform.position).normalized;           
            
            // Mover al enemigo hacia el jugador o alejarlo del jugador 
            transform.Translate(direccion * velocidad * Time.deltaTime);
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
using UnityEngine;

public class EnemyBehaiviour : MonoBehaviour
{
    public float velocidad = 3.0f; // Velocidad de movimiento del enemigo

    // public float lifePerSecond = 0.5f;
    public float hp=10;
    // public float maxHp=15;
    // public float maxScale = 5;
    // public float minScale = 1;
    private Transform jugador; // Referencia al jugador

    public void hitEnemy(float ammount){
        hp-=ammount;
        Debug.Log(ammount);
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

        if (hp<=0){
            Destroy(this.gameObject);
        }

        // hp+=lifePerSecond * Time.deltaTime;

        float scale = hp;


        transform.localScale = new Vector3(scale, scale);
        
        if (jugador != null)
        {
            // Calcular la dirección hacia el jugador
            Vector3 direccion = (jugador.position - transform.position).normalized;

            // Mover al enemigo hacia el jugador
            transform.Translate(direccion * velocidad * Time.deltaTime);
        }
    }
}
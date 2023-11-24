using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class MovePlayer : MonoBehaviour
{
    
    [SerializeField] private FieldOfView fieldOfView;

    public AudioClip stepSound;
    bool canReproduceStep = true;

    AudioSource audioSource;

    void setCanReproduceStep(){
        canReproduceStep=true;
    }

    SpriteRenderer sprite;
    public Transform weapon;

    public float life=10; //Setted from GameController
    public float maxLife = 10; //Setted from GameController

    public float regLife = 0.1f; //Setted from GameController 
    public GameObject healthBar;
    private Image healthBarReal;

    public float velocidad; //Setted from GameController

    Animator animator;

    bool resultUseStick;

    void setResultUseStickFalse(){
        resultUseStick=false;
    }

    bool isAnimatingHit=false;
    

    void Start(){
        sprite=GetComponent<SpriteRenderer>();
        // weapon=GetComponentInChildren<Transform>();
        animator=GetComponent<Animator>();

        healthBarReal=healthBar.GetComponent<Image>();

        audioSource= GetComponent<AudioSource>();

    }

    // void FixedUpdate(){

    // }
    void Update()
    {
        // Verificar la entrada del jugador 
        float movimientoHorizontal = Input.GetAxisRaw("Horizontal");
        float movimientoVertical = Input.GetAxisRaw("Vertical");

            
        if (movimientoHorizontal != 0 || movimientoVertical!=0) {
            animator.SetBool("is_mooving", true);
            if (!audioSource.isPlaying &&  canReproduceStep && stepSound!=null ){
                canReproduceStep=false;
                Invoke("setCanReproduceStep", 0.5f); //Delay for sounds
                audioSource.PlayOneShot(stepSound);
                }
        }
        else animator.SetBool("is_mooving", false);

            // Calcular la dirección de movimiento
        Vector3 movimiento = new Vector3(movimientoHorizontal, movimientoVertical, 0.0f);

        // Aplicar el movimiento al objeto
        transform.Translate(movimiento.normalized * velocidad * Time.deltaTime);


        if (movimientoHorizontal>0) {
            sprite.flipX=false;
            weapon.localPosition=new Vector3(0.065f,weapon.localPosition.y);

        }

        if (movimientoHorizontal<0) {
            sprite.flipX=true;
            weapon.localPosition=new Vector3(-0.065f,weapon.localPosition.y);
        }


        if (Input.GetMouseButtonDown(0)) GameController.gameController.useFov();
        else if (Input.GetMouseButtonUp(0)) GameController.gameController.stopUsingFov();

        if (Input.GetMouseButtonDown(1) && !resultUseStick){ 
            GameController.gameController.useStick(ref resultUseStick);

            if (resultUseStick){
                Invoke("setResultUseStickFalse", 0.4f);
            }
            
        }
    
        Vector3 posicionMouse = Input.mousePosition;
        Vector3 aimDir = (posicionMouse);


        //Evaluar si mori
        if (life<=0) {
            GameController.gameController.GameOver();
            //Loose effect music
            if (transform.localScale.x>0)
                transform.localScale -= Vector3.one * 0.5f * Time.deltaTime;
        }

        if (life<maxLife)
            life+=regLife*Time.deltaTime;

        healthBarReal.fillAmount=life/maxLife;

        

    }

IEnumerator hitAnimation()
    {   
        if (!isAnimatingHit){
        
            isAnimatingHit=true;
        
            sprite.color = new Color(255, 0,0);
            yield return new WaitForSeconds(0.2f);
            
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.2f);

            sprite.color = new Color(255, 0,0);
            yield return new WaitForSeconds(0.2f);
            
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.2f);

            sprite.color = Color.white;
            isAnimatingHit=false;
        }
    }

    void OnTriggerStay2D(Collider2D col){
        
        if (col.gameObject.tag == "Enemy" && !resultUseStick){
            life-=1*Time.deltaTime; //TODO: Agregar el daño del enemigo aca
            StartCoroutine(hitAnimation());


        }
    }
}

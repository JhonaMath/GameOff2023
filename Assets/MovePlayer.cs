using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    
    [SerializeField] private FieldOfView fieldOfView;

    SpriteRenderer sprite;
    public Transform weapon;

    public float life;

    public float velocidad = 5.0f; // Velocidad de movimiento ajustable

    Animator animator;
    

    void Start(){
        sprite=GetComponent<SpriteRenderer>();
        // weapon=GetComponentInChildren<Transform>();
        animator=GetComponent<Animator>();


    }


    void Update()
    {
        // Verificar la entrada del jugador 
        float movimientoHorizontal = Input.GetAxis("Horizontal");
        float movimientoVertical = Input.GetAxis("Vertical");

            
        if (movimientoHorizontal != 0 || movimientoVertical!=0) animator.SetBool("is_mooving", true);
        else animator.SetBool("is_mooving", false);

        // Calcular la dirección de movimiento
        Vector3 movimiento = new Vector3(movimientoHorizontal, movimientoVertical, 0.0f);

        // Aplicar el movimiento al objeto
        transform.Translate(movimiento * velocidad * Time.deltaTime);


        Debug.Log("MOVIMIENTO HOR: " + movimientoHorizontal);

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

        if (Input.GetMouseButtonDown(1)){ 
            GameController.gameController.useStick();
            Debug.Log("SecondButton");
        }
    
        Vector3 posicionMouse = Input.mousePosition;
        Vector3 aimDir = (posicionMouse);


    }
}

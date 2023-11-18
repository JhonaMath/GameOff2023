using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehaiviour : MonoBehaviour
{

    public Sprite spriteClick;
    public Sprite spriteNoClick;

    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update

    Vector3 initialScale;
float velocidad = 1.0f;
float amplitud = 0.1f;

    float acumulatorDeg = 0;
    void Start()
    {
        Cursor.visible = false;
        initialScale=transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;

        transform.position=mousePos;

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) {
            spriteRenderer.sprite=spriteClick;
            acumulatorDeg+=-45*Time.deltaTime;
            transform.rotation= Quaternion.Euler(new Vector3(0,0,acumulatorDeg+transform.rotation.z));
        }else{
            spriteRenderer.sprite=spriteNoClick;
        }

        //Scale
        float offsetY = Mathf.Sin(Time.time * velocidad) * amplitud;

        transform.localScale=initialScale + new Vector3(offsetY,offsetY,0);
    }
}

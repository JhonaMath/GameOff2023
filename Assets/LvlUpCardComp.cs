using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LvlUpCardComp : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public int cardNum;

    Image image;

    void Start(){

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       
        Image image=GetComponent<Image>();
        image.color=Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
        Image image=GetComponent<Image>();
        image.color=Color.gray;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        GameController.gameController.selectCard(cardNum);
    }
}

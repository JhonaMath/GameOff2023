using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExitText : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public Text text;
    // Start is called before the first frame update    public Text text;
    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color= new Color(255,255,255,1f);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color= new Color(255,255,255,0.6f);
        
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            Application.Quit();
        }
    }
}

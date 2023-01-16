using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActOnCannotSolveCubeGame : MonoBehaviour, IPointerEnterHandler
{
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
         Debug.Log("Cannot solve pressed !!!! this.name = " + this.name); // + " dragging? " + eventData.dragging);
    }
}

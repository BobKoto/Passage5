using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectARandomGame: MonoBehaviour
{
    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Debug.Log(this.name + "  " + other.name + " came thru! Let's do something now ");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
public class CubeEnteredSolutionMatrix : MonoBehaviour   
// Component of each CubePlacement Game Object -NOT the Cubes- -- sends Cube enter/exit events to CubePlacementHandler.cs
{
    public CubeTriggerEnterExitEvent cubeTriggerEnterExitEvent;
    private void OnTriggerEnter(Collider cube) => 
        cubeTriggerEnterExitEvent.Invoke(this.gameObject, this.name, cube.gameObject, true);  //Send event to CubePlacementHandler
    private void OnTriggerExit(Collider cube) => 
        cubeTriggerEnterExitEvent.Invoke(this.gameObject, this.name, cube.gameObject, false);  //Send event to CubePlacementHandler

} // end class 

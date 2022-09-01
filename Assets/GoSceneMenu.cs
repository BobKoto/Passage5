using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoSceneMenu : MonoBehaviour
{
    public Component lightComponent;
    public GameObject playerCameraRoot;
    bool lightDisabled;
    // Start is called before the first frame update
    void Start()
    {
        lightComponent = playerCameraRoot.GetComponent<Light>();
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}


    public void OnButtonMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void OnButtonLight()
    {
        lightComponent = playerCameraRoot.GetComponent<Light>();
        if (!lightDisabled)
        {
        //    Debug.Log("light button pressed light is ENABLED and will be Disabled...");
            lightComponent.GetComponent<Light>().enabled = false;
            lightDisabled = true;
        }
        else
        {
        //    Debug.Log("light button pressed light is DISABLED and will be Enabled");
            lightComponent.GetComponent<Light>().enabled = true;
            lightDisabled = false;
        }
    }
}

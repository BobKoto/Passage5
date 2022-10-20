using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SelectSceneMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void  OnButtonCapsulePlayer()
    {
        SceneManager.LoadScene("CapsuleScene"); 
    }
    public void OnButtonRobotAvatarPlayer()
    {
        SceneManager.LoadScene("AvatarScene");
    }
    public void OnButtonMirrorRoutine()
    {
        SceneManager.LoadScene("MirrorRoutine");
    }
    public void OnButtonExit()
    {
        Application.Quit();
    }


}


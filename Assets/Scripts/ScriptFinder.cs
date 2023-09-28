using UnityEngine;

public class ScriptFinder : MonoBehaviour
{
    public bool findScripts;
    public bool findAudiosources;
    void Start()
    {
        // Find all active scripts in the scene
        if (findScripts)
        {
            MonoBehaviour[] runningScripts = FindObjectsOfType<MonoBehaviour>();
            // Iterate through the found scripts
            foreach (MonoBehaviour script in runningScripts)
            {
                Debug.Log("Found script: " + script.GetType().Name);
            }
        }
        if (findAudiosources)
        {
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource aAudioSource in audioSources)
            {
                Debug.Log("Found AudioSource: " + aAudioSource.name    ); //            GetType().Name);
            }
        }
    }
}

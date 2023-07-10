using UnityEngine;

public class ScriptFinder : MonoBehaviour
{
    void Start()
    {
        // Find all active scripts in the scene
        MonoBehaviour[] runningScripts = FindObjectsOfType<MonoBehaviour>();

        // Iterate through the found scripts
        foreach (MonoBehaviour script in runningScripts)
        {
            Debug.Log("Found script: " + script.GetType().Name);
        }
    }
}

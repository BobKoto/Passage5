using System.Collections;
using UnityEngine;

public class AnimatorLister : MonoBehaviour
{
    int i;
    private void Start()
    {
        StartCoroutine(ShowAfterDelay(5));
    }
    private void ShowAnimators()
    {
        // Get all the active animators in the scene

        Animator[] animators = FindObjectsOfType<Animator>();
        Debug.Log("Show All Animators  Found " + animators.Length + " Animations" );
        // Iterate through the animators and print their names
        foreach (Animator animator in animators)
        {
            Debug.Log("Animator Name: " + animator.name);
        }
    }
    private void ShowAnimations()
    {
        // Get all the active animators in the scene
        Animator[] animators = FindObjectsOfType<Animator>();

        // Iterate through the animators and get the currently playing animations
        foreach (Animator animator in animators)
        {
            // Get the current state information from the Animator
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

            // Check if the animator is currently playing an animation
            if (currentState.normalizedTime < 1f)
            {
                // Get an array of AnimatorClipInfo for the current animator
                AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0);

                // Iterate through the clipInfos array
                foreach (AnimatorClipInfo clipInfo in clipInfos)
                {
                    // Get the name of the animation clip
                    string animationName = clipInfo.clip.name;

                    Debug.Log("Playing Animation: " + animationName);
                    i++;
                }
            }
        }
        Debug.Log("Found " + i + "  animation clips playing");
    }

    IEnumerator ShowAfterDelay(int delay)
    {
        yield return new WaitForSeconds(delay);
        ShowAnimators();
        ShowAnimations();
    }
}
/*
using UnityEngine;

public class AnimationLister : MonoBehaviour
{
    private void Update()
    {
        // Get all the active animators in the scene
        Animator[] animators = FindObjectsOfType<Animator>();

        // Iterate through the animators and get the currently playing animations
        foreach (Animator animator in animators)
        {
            // Get the current state information from the Animator
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

            // Check if the animator is currently playing an animation
            if (currentState.normalizedTime < 1f)
            {
                // Get the name of the currently playing animation
                string animationName = currentState.name;

                Debug.Log("Playing Animation: " + animationName);
            }
        }
    }
}
     {
        // Get all the active animations in the scene

        Animator[] animators = FindObjectsOfType<Animator>();
        Debug.Log("Show All Animations    Found " + animators.Length + " Animations" );
        // Iterate through the animators and get the currently playing animations

        foreach (Animator animator in animators)
        {
            // Get the current state information from the Animator
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

            // Check if the animator is currently playing an animation
            if (currentState.normalizedTime < 1f)
            {
                // Get the name of the currently playing animation
                // string animationName = currentState.fullPathHash.ToString();
                string animationName = currentState.name;

                Debug.Log("Playing Animation: " + animationName);
                i++;
            }
        }
        Debug.Log("Found " + i + " playing animations");
    }
 * */
using UnityEngine;

public class PlayOpenAnimation : MonoBehaviour
{
    public Animator screenAnimator;

    void Start()
    {
        screenAnimator.SetTrigger("Open");
    }
}

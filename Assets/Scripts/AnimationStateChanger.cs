using UnityEngine;

public class AnimationStateChanger : MonoBehaviour
{

    public Animator animator;
    public string currentState;


    void Awake()
    {
        currentState = "CharacterIdle";
        animator = GetComponent<Animator>();
        //currentState = false;
    }

    public bool overrideLocked = false;

    public void ChangeAnimation(string newState, float crossFadeTime = .2f) {
        if (currentState == newState) {
            return;
        }
        animator.CrossFade(newState, crossFadeTime);
        currentState = newState;
    }
    
}
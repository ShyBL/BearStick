using UnityEngine;

public class PlayerVisualizer : MonoBehaviour
{
    [SerializeField] public Animator animator;

    public void PlayAnimation(string animName)
    {
        animator.Play(animName);
    }
}
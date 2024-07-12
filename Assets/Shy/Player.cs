using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerPhysx playerPhysx;
    
    [Header(" Movement ")]
    [SerializeField] private bool canMove = true;
    [SerializeField] private float moveSpeed = 8f; 
    [SerializeField] private Vector3 moveInputVector;
    
    public static Player Instance;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    private void OnEnable()
    {
        playerInput.onMove += MovementHandler;
        playerInput.onMoveStopped += MovementHandler;
    }

    private void OnDisable()
    {
        playerInput.onMove -= MovementHandler;
        playerInput.onMoveStopped -= MovementHandler;
    }

    private void MovementHandler()
    {
        if (canMove) 
        {
            moveInputVector = playerInput.moveVector;

            playerPhysx.HandleMovement(moveInputVector, moveSpeed);
        }
    }
    
    public void EnableMovement() => canMove = true;
    public void DisableMovement() => canMove = false;
    public void StopInPlace() => playerPhysx.HandleMovement(new Vector3(0,0,0), 0);
    
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerActionsAsset actionAsset;
    
    [Header(" Actions ")]
    public Action onMove;
    public Action onMoveStopped;
    
    public Vector3 moveVector;
    
    private void OnEnable()
    {
        actionAsset = new PlayerActionsAsset();
        actionAsset.Enable();
        actionAsset.Player.Move.performed += OnMovePerformed;
        actionAsset.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        actionAsset.Player.Move.performed -= OnMovePerformed;
        actionAsset.Player.Move.canceled -= OnMoveCanceled;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        float xVector = actionAsset.Player.Move.ReadValue<Vector2>().x;
        float zVector = actionAsset.Player.Move.ReadValue<Vector2>().y;
        
        moveVector = new Vector3(xVector, 0, zVector).normalized;
        
        onMove?.Invoke();
    }
    
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveVector = Vector3.zero;
        onMoveStopped?.Invoke();
    }
    
}
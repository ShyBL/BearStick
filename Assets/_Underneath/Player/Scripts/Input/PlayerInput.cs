using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerActionsAsset actionAsset;
    
    [Header(" Actions ")]
    public Action onMove;
    public Action onMoveStopped;
    public Action onJump;
    public Action onInteract;

    public Action onBagOpened;
    public Action onDialogueEnd;
    public Action onPauseMenu;
    
    public Vector3 moveVector;
    
    private void OnEnable()
    {
        actionAsset = new PlayerActionsAsset();
        actionAsset.Enable();
        actionAsset.Player.Move.performed += OnMovePerformed;
        actionAsset.Player.Move.canceled += OnMoveCanceled;
        actionAsset.Player.Jump.performed += OnJumpPerformed;
        actionAsset.Player.Interact.performed += OnInteractPerformed;
        actionAsset.Player.OpenBag.performed += OnBagOpenedPerformed;
        actionAsset.Player.EndDialogue.performed += OnDialogueEnded;
        actionAsset.Player.PauseMenu.performed += OnPauseMenu;
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
    
    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        onJump?.Invoke();
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        onInteract?.Invoke();
    }
    
    private void OnBagOpenedPerformed(InputAction.CallbackContext context)
    {
        onBagOpened?.Invoke();
    }

    private void OnDialogueEnded(InputAction.CallbackContext context)
    {
        onDialogueEnd?.Invoke();
    }

    private void OnPauseMenu(InputAction.CallbackContext context)
    {
        onPauseMenu?.Invoke();
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 MoveVector;
    InputAction MoveAction;
    private void Start()
    {
        MoveAction = InputSystem.actions.FindAction("Move");
    }
    private void Update()
    {
        
    }
    void PlayerMovement()
    {
        MoveVector = MoveAction.ReadValue<Vector2>();
    }
}

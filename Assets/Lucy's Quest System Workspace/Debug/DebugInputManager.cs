// Author(s): Lucy Rubin
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]
public class DebugInputManager : MonoBehaviour
{
    public void DebugPressed(InputAction.CallbackContext context)
    {

        if (context.started)
        {
           
            DebugEventManager.instance.debugEvents.DebugPressed();
        }
    }

}

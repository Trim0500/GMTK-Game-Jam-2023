using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class which handles reading Input for other scripts to reference
/// </summary>
public class InputManager : MonoBehaviour
{
    // A global instance for scripts to reference
    public static InputManager instance;

    /// <summary>
    /// Description:
    /// Standard Unity Function called when the script is loaded
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    private void Awake()
    {
        ResetValuesToDefault();
        // Set up the instance of this
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Description:
    /// Sets all the input variables to their default values so that nothing weird happens in the game if you accidentally
    /// set them in the editor
    /// Input:
    /// none
    /// Return:
    /// void
    /// </summary>
    void ResetValuesToDefault()
    {
        //horizontalMovement = default;
        //verticalMovement = default;

        horizontalLookAxis = default;
        verticalLookAxis = default;

        //jumpStarted = default;
        //jumpHeld = default;

        pauseButton = default;
    }

    //[Header("Movement Input")]
    //[Tooltip("The horizontal movmeent input of the player.")]
    //public float horizontalMovement;
    //[Tooltip("The vertical movmeent input of the player.")]
    //public float verticalMovement;

    /// <summary>
    /// Description:
    /// Reads and stores the movement input
    /// Input: 
    /// CallbackContext callbackContext
    /// Return:
    /// void (no return)
    /// </summary>
    /// <param name="callbackContext">The context of the movement input</param>
    //public void GetMovementInput(InputAction.CallbackContext callbackContext)
    //{
    //    Vector2 movementVector = callbackContext.ReadValue<Vector2>();
    //    horizontalMovement = movementVector.x;
    //    verticalMovement = movementVector.y;
    //}

    //[Tooltip("Whether a run was started this frame.")]
    //public bool runStarted = false;
    //[Tooltip("Whether the run button is being held.")]
    //public bool runHeld = false;

    /// <summary>
    /// Description:
    /// Reads and stores the run input
    /// Input: 
    /// CallbackContext callbackContext
    /// Return: 
    /// void (no return)
    /// </summary>
    /// <param name="callbackContext">The context of the run input</param>
    //public void GetRunInput(InputAction.CallbackContext callbackContext)
    //{
    //    runStarted = !callbackContext.canceled;
    //    runHeld = !callbackContext.canceled;
    //    if (InputManager.instance.isActiveAndEnabled)
    //    {
    //        StartCoroutine("ResetRunStart");
    //    }
    //}

    /// <summary>
    /// Description
    /// Coroutine that resets the run started variable after one frame
    /// Input: 
    /// none
    /// Return: 
    /// IEnumerator
    /// </summary>
    /// <returns>Allows this to function as a coroutine</returns>
    //private IEnumerator ResetRunStart()
    //{
    //    yield return new WaitForEndOfFrame();
    //    runStarted = false;
    //}

    //[Header("Jump Input")]
    //[Tooltip("Whether a jump was started this frame.")]
    //public bool jumpStarted = false;
    //[Tooltip("Whether the jump button is being held.")]
    //public bool jumpHeld = false;

    /// <summary>
    /// Description:
    /// Reads and stores the jump input
    /// Input: 
    /// CallbackContext callbackContext
    /// Return: 
    /// void (no return)
    /// </summary>
    /// <param name="callbackContext">The context of the jump input</param>
    //public void GetJumpInput(InputAction.CallbackContext callbackContext)
    //{
    //    jumpStarted = !callbackContext.canceled;
    //    jumpHeld = !callbackContext.canceled;
    //    if (InputManager.instance.isActiveAndEnabled)
    //    {
    //        StartCoroutine("ResetJumpStart");
    //    } 
    //}

    /// <summary>
    /// Description
    /// Coroutine that resets the jump started variable after one frame
    /// Input: 
    /// none
    /// Return: 
    /// IEnumerator
    /// </summary>
    /// <returns>Allows this to function as a coroutine</returns>
    //private IEnumerator ResetJumpStart()
    //{
    //    yield return new WaitForEndOfFrame();
    //    jumpStarted = false;
    //}

    //[Header("Cling Input")]
    //[Tooltip("Whether a cling was started this frame.")]
    //public bool clingStarted = false;
    //[Tooltip("Whether the cling button is being held.")]
    //public bool clingHeld = false;

    /// <summary>
    /// Description:
    /// Reads and stores the jump input
    /// Input: 
    /// CallbackContext callbackContext
    /// Return: 
    /// void (no return)
    /// </summary>
    /// <param name="callbackContext">The context of the jump input</param>
    //public void GetClingInput(InputAction.CallbackContext callbackContext)
    //{
    //    clingStarted = !callbackContext.canceled;
    //    clingHeld = !callbackContext.canceled;
    //    if (InputManager.instance.isActiveAndEnabled)
    //    {
    //        StartCoroutine("ResetClingStart");
    //    }
    //}

    /// <summary>
    /// Description
    /// Coroutine that resets the jump started variable after one frame
    /// Input: 
    /// none
    /// Return: 
    /// IEnumerator
    /// </summary>
    /// <returns>Allows this to function as a coroutine</returns>
    //private IEnumerator ResetClingStart()
    //{
    //    yield return new WaitForEndOfFrame();
    //    clingStarted = false;
    //}

    [Header("Pause Input")]
    [Tooltip("The state of the pause button")]
    public float pauseButton = 0;

    /// <summary>
    /// Description:
    /// Collects pause button input
    /// Input: 
    /// CallbackContext callbackContext
    /// Returns:
    /// void (no return)
    /// </summary>
    /// <param name="callbackContext">The context of the pause input</param>
    public void GetPauseInput(InputAction.CallbackContext callbackContext)
    {
        pauseButton = callbackContext.ReadValue<float>();
    }
    
    [Header("Mouse Input")]
    [Tooltip("The horizontal mouse input of the player.")]
    public float horizontalLookAxis;
    [Tooltip("The vertical mouse input of the player.")]
    public float verticalLookAxis;

    /// <summary>
    /// Description:
    /// Collects movementInput
    /// Input: 
    /// CallbackContext callbackContext
    /// Return:
    /// void (no return)
    /// </summary>
    /// <param name="callbackContext">The context of the mouse input</param>
    public void GetMouseLookInput(InputAction.CallbackContext callbackContext)
    {
        Vector2 mouseLookVector = callbackContext.ReadValue<Vector2>();
        horizontalLookAxis = mouseLookVector.x;
        verticalLookAxis = mouseLookVector.y;
    }

    [Header("Jump Input")]
    [Tooltip("Whether a jump was started this frame.")]
    public bool selectStarted = false;
    [Tooltip("Whether the jump button is being held.")]
    public bool selectHeld = false;

    /// <summary>
    /// Description:
    /// Reads and stores the jump input
    /// Input: 
    /// CallbackContext callbackContext
    /// Return: 
    /// void (no return)
    /// </summary>
    /// <param name="callbackContext">The context of the jump input</param>
    public void GetSelectInput(InputAction.CallbackContext callbackContext)
    {
        selectStarted = !callbackContext.canceled;
        selectHeld = !callbackContext.canceled;
        if (InputManager.instance.isActiveAndEnabled)
        {
            StartCoroutine("ResetSelectStart");
        }
    }

    /// <summary>
    /// Description
    /// Coroutine that resets the jump started variable after one frame
    /// Input: 
    /// none
    /// Return: 
    /// IEnumerator
    /// </summary>
    /// <returns>Allows this to function as a coroutine</returns>
    private IEnumerator ResetSelectStart()
    {
        yield return new WaitForEndOfFrame();
        selectStarted = false;
    }
}
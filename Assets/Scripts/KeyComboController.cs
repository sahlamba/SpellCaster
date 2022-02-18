using System.Collections.Generic;
using UnityEngine;

/**
 * KeyComboController defines a list of combo keys and
 * a time interval between subsequent combo key presses.
 *
 * On the first key press, the controller starts listening
 * for the next combo key until the last. If the next key 
 * is not pressed within the specified time interval or an 
 * incorrect key is pressed, the combo is aborted. If all 
 * the combo keys are successfully pressed in the correct
 * order, a combo action is executed.
 **/
public class KeyComboController : MonoBehaviour
{
    public enum State
    {
        IDLE,
        LISTENING,
        EXECUTING
    };

    [SerializeField]
    private float comboKeyIntervalInMillis = 300;
    [SerializeField]
    private List<KeyCode> comboKeys = new List<KeyCode>();

    private Rigidbody2D rb;

    private State currentState;
    private float lastKeyTimeElapsed;
    private int nextComboKeyIndex;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentState = State.IDLE;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.IDLE:
                {
                    Idle();
                    break;
                }
            case State.LISTENING:
                {
                    Listen();
                    break;
                }
            case State.EXECUTING:
                {
                    Execute();
                    break;
                }
            default:
                {
                    Debug.LogError("Invalid state!");
                    TransitionToState(State.IDLE);
                    break;
                }
        }
    }

    private void Idle()
    {
        // Reset on state entry
        Reset();

        if (isNextComboKeyPressed())
        {
            nextComboKeyIndex++;
            if (nextComboKeyIndex == comboKeys.Count)
            {
                // The last combo key was just pressed
                TransitionToState(State.EXECUTING);
                return;
            }
            TransitionToState(State.LISTENING);
        }
    }

    private void Listen()
    {
        Debug.Log("Waiting for " + comboKeys[nextComboKeyIndex].ToString() + " to be pressed...");

        if (lastKeyTimeElapsed > comboKeyIntervalInMillis)
        {
            TransitionToState(State.IDLE);
            return;
        }

        if (!Input.anyKeyDown) // No key was pressed
        {
            lastKeyTimeElapsed += Time.deltaTime * 1000; // Add millis elapsed
            return;
        }

        // Some key was pressed
        if (isNextComboKeyPressed())
        {
            // Reset last key time
            lastKeyTimeElapsed = 0;

            nextComboKeyIndex++;
            if (nextComboKeyIndex == comboKeys.Count)
            {
                // The last combo key was just pressed
                TransitionToState(State.EXECUTING);
                return;
            }
        }
        else
        {
            // Next key was not pressed, abort combo
            Debug.Log("Aborting...");
            TransitionToState(State.IDLE);
        }
    }

    private void Execute()
    {
        Debug.Log("Executing...");

        // Make player jump
        rb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);

        Debug.Log("Execution complete.");
        TransitionToState(State.IDLE);
    }

    private bool isNextComboKeyPressed()
    {
        return Input.GetKeyDown(comboKeys[nextComboKeyIndex]);
    }

    private void TransitionToState(State nextState)
    {
        Debug.Log("Transitioning to " + nextState.ToString() + "...");
        currentState = nextState;
    }

    private void Reset()
    {
        lastKeyTimeElapsed = 0;
        nextComboKeyIndex = 0;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    public enum State
    {
        IDLE,
        LISTENING,
        EXECUTING
    };

    [SerializeField]
    private float totalComboDurationInMillis = 1200;
    [SerializeField]
    private KeyCode castButton = KeyCode.Space;

    private List<Spell> spells = new List<Spell> {
        // TODO: Make spell a scriptable object and hook the event system for actual execution
        new FireSpell(),
        new WaterSpell(),
        new EarthSpell(),
        new AirSpell()
    };

    private Rigidbody2D rb;

    private State currentState;
    private float comboTimeElapsed;
    private ComboInputNode comboInputTreeRoot;
    private ComboInputNode lastButtonPressed;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        comboInputTreeRoot = buildComboInputTree();

        comboTimeElapsed = 0;
        currentState = State.IDLE;
        lastButtonPressed = comboInputTreeRoot;
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
        var pressedButton = isNextComboButtonPressed();
        if (pressedButton.HasValue)
        {
            if (RegisterButtonPress(pressedButton))
            {
                TransitionToState(State.EXECUTING);
                return;
            };
            TransitionToState(State.LISTENING);
        }
    }

    private void Listen()
    {
        if (comboTimeElapsed > totalComboDurationInMillis)
        {
            TransitionToState(State.IDLE);
            return;
        }
        if (!Input.anyKeyDown) // No key was pressed
        {
            comboTimeElapsed += Time.deltaTime * 1000; // Add millis elapsed
            return;
        }
        // Some key was pressed
        var pressedButton = isNextComboButtonPressed();
        if (pressedButton.HasValue)
        {
            if (RegisterButtonPress(pressedButton))
            {
                TransitionToState(State.EXECUTING);
                return;
            }
        }
        else
        {
            // Next button in combo was not pressed, abort combo
            Debug.Log("Aborting...");
            TransitionToState(State.IDLE);
        }
    }

    private void Execute()
    {
        // Custom spell execution
        lastButtonPressed.spell.execute();

        TransitionToState(State.IDLE);
    }

    private ComboInputNode buildComboInputTree()
    {
        var root = new ComboInputNode(new Dictionary<KeyCode, ComboInputNode>());

        foreach (var spell in spells)
        {
            var comboButtonsWithCastButton = new List<KeyCode>(spell.comboButtons);
            comboButtonsWithCastButton.AddRange(new List<KeyCode> { castButton });

            AddSpellToComboInputTree(spell, comboButtonsWithCastButton, root);
        }

        return root;
    }

    private void AddSpellToComboInputTree(Spell spell, List<KeyCode> comboButtons, ComboInputNode current)
    {
        if (comboButtons.Count == 0)
        {
            current.spell = spell;
            return;
        }

        var button = comboButtons[0];
        if (!current.children.ContainsKey(button))
        {
            current.children.Add(button, new ComboInputNode(new Dictionary<KeyCode, ComboInputNode>()));
        }
        current = current.children[button];

        var nextComboButtons = new List<KeyCode>();
        if (comboButtons.Count > 1) nextComboButtons = comboButtons.GetRange(1, comboButtons.Count - 1);
        AddSpellToComboInputTree(spell, nextComboButtons, current);
    }

    private Nullable<KeyCode> isNextComboButtonPressed()
    {
        foreach (var button in lastButtonPressed.children.Keys)
        {
            if (Input.GetKeyDown(button))
            {
                return button;
            }
        }
        return null;
    }

    /**
     * Returns true if the spell cast/trigger button was pressed.
     **/
    private bool RegisterButtonPress(Nullable<KeyCode> pressedButton)
    {
        lastButtonPressed = lastButtonPressed.children[pressedButton.Value];
        return lastButtonPressed.spell != null;
    }

    private void TransitionToState(State nextState)
    {
        currentState = nextState;
    }

    private void Reset()
    {
        comboTimeElapsed = 0;
        lastButtonPressed = comboInputTreeRoot;
    }
}

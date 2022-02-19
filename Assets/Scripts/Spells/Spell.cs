using System.Collections.Generic;
using UnityEngine;

public abstract class Spell
{
    public List<KeyCode> comboButtons { get; private set; }

    public Spell(List<KeyCode> comboButtons)
    {
        this.comboButtons = comboButtons;
    }

    // Override exeucte method
    public abstract void execute();
}

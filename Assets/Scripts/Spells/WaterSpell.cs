using System.Collections.Generic;
using UnityEngine;

public class WaterSpell : Spell
{
    public WaterSpell() : base(new List<KeyCode> { KeyCode.W, KeyCode.A })
    {
    }

    override public void execute()
    {
        Debug.Log("Water!");
    }
}

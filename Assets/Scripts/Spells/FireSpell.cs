using System.Collections.Generic;
using UnityEngine;

public class FireSpell : Spell
{
    public FireSpell() : base(new List<KeyCode> { KeyCode.W, KeyCode.W })
    {
    }

    override public void execute()
    {
        Debug.Log("Fire!");
    }
}

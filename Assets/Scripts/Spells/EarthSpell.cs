using System.Collections.Generic;
using UnityEngine;

public class EarthSpell : Spell
{
    public EarthSpell() : base(new List<KeyCode> { KeyCode.A, KeyCode.A })
    {
    }

    override public void execute()
    {
        Debug.Log("Earth!");
    }
}

using System.Collections.Generic;
using UnityEngine;

public class AirSpell : Spell
{
    public AirSpell() : base(new List<KeyCode> { KeyCode.A, KeyCode.D })
    {
    }

    override public void execute()
    {
        Debug.Log("Air!");
    }
}

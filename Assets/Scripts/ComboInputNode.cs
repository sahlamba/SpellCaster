using System;
using System.Collections.Generic;
using UnityEngine;

public class ComboInputNode
{
    private Spell _spell;
    public Spell spell
    {
        get { return _spell; }
        set { _spell = value; }
    }
    public Dictionary<KeyCode, ComboInputNode> children { get; private set; }

    public ComboInputNode(Dictionary<KeyCode, ComboInputNode> children)
    {
        this.children = children;
    }
}
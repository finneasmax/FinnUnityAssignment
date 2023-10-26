using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public string name;
    public int exp = 0;
    public Character()
    { name = "not assigned"; }

    public Character(string name)
    { this.name = name; }

    public virtual void PrintCharacterStats()
    {
        Debug.LogFormat("Hero: {0} - {1} EXP", this.name, this.exp);
    }
    private void Reset()
    {
        this.name = "not assigned";
        this.exp = 0;
    }
}

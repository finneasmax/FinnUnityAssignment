using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paladin : Character
{
    public Weapon weapon;

    public Paladin(string name, Weapon weapon) : base(name)
    {
        this.weapon = weapon;
    }
    public override void PrintCharacterStats()
    {
        Debug.LogFormat("Hail {0}... Take  up your {1}!!", this.name, this.weapon.name);
    }

}


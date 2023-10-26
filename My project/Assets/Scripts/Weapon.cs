using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Weapon
{
    public int damage;
    public string name;
    public Weapon(string name, int damage)
    {
        this.name = name;
        this.damage = damage;
    }
    public void PrintWeaponStats()
    {
        Debug.LogFormat("Weapon: {0} - {1} damage", this.name, this.damage);
    }

}
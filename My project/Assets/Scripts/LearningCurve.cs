using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningCurve : MonoBehaviour
{
    public bool hasDungeonKey = true;
    public int CurrentGold = 32;
    public string CharacterAction = "Attack!!";
    public bool weaponEquipped = true;
    public string weaponType = "longsword";
    public Transform CamTransform;
    public GameObject DirectionalLight;
    public Transform LightTransform;
    public void PrintCharacterAction()
    {
        switch (CharacterAction)
        {
            case "Heal":
                Debug.Log("drnk your gatorade");
                break;
            case "Attack!!":
                Debug.Log("Kill kill kill");
                break;
            default:
                Debug.Log("watch out!");
                break;
        }
    }
    public void Thievery()
    {
        if (CurrentGold > 50)
        { Debug.Log("Hoo MAMA!"); }
        else if (CurrentGold < 15)
        { Debug.Log(":((("); }
        else
        { Debug.Log("looks like your purse in the sweet spot"); }
    }
   

    private void Start()
    {
        DirectionalLight = GameObject.Find("Drectional Light");
        LightTransform = DirectionalLight.GetComponent<Transform>();
        Debug.Log(LightTransform.localPosition);
        CamTransform = this.GetComponent<Transform>();
        Debug.Log(CamTransform.localPosition);
        Weapon Scythe = new Weapon("Scythe", 100);
        Weapon Cutter = Scythe;
        Cutter.name = "Cutter";
        Cutter.damage = 50;
        Cutter.PrintWeaponStats();
        Scythe.PrintWeaponStats();
        Paladin knight = new Paladin("Arty Bucco", Scythe);
        knight.PrintCharacterStats();
        Character Heroine = new Character("Agatha");
        Character Hero = new Character();
        Character Villain = Hero;
        Villain.name = "Bartholomew destroyer of worlds";
        Hero.PrintCharacterStats();
        Villain.PrintCharacterStats();
        PrintCharacterAction();
        if (weaponEquipped)
        {
            if (weaponType == "longsword")
            { Debug.Log("God save the Queen!"); }
        }
        else { Debug.Log("Fists wont work against armor"); }
        Thievery();
        if (hasDungeonKey)
        {
            Debug.Log("Yoous posses the  sacred key - enter");
        }
        else
        {
            Debug.Log("Youze hath not proven thy self");
        }


    }
    private void Update()
    {
        
    }
}

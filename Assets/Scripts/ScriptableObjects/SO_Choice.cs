using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SO/Choice")]
public class SO_Choice : ScriptableObject
{
    public Sprite spriteOfChoice;

    public EnumClass.ChoiceType typeOfChoice;

    [Header("Custom Inspector to do according to typeOfChoice")]
    public EnumClass.WeaponType typeOfWeapon;

    public EnumClass.WeaponBuffType typeOfWeaponBuff;
    
    
    public void ApplyChoice(Player targetPlayer)
    {
        GlobalManager.Instance.DestroyAllCommandsOfPlayer(targetPlayer);

        CommandInGame newCommand = null;
        
        switch (typeOfChoice)
        {
            case(EnumClass.ChoiceType.Weapon):

                newCommand = new CommandWeaponChoice(targetPlayer, typeOfWeapon);
                
                break;
            
            case(EnumClass.ChoiceType.WeaponBuff):
                
                //newCommand = newCommandWeaponBuffChoice(targetPlayer, typeOfWeaponBuff);
                break;
            
            
        }
        
        
        
        GlobalManager.Instance.AddActionInGameToList(newCommand);
        
    }

}

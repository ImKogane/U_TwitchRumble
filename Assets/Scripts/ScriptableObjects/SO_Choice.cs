using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SO/Choice")]
public abstract class SO_Choice : ScriptableObject
{
    [Header("Sprite of card")]

    public Sprite _cardSprite;

    public virtual void ApplyChoice(Player targetPlayer)
    {
        GlobalManager.Instance.DestroyAllCommandsOfPlayer(targetPlayer);

        CommandInGame newCommand = new CommandChoice(targetPlayer, this);

        GlobalManager.Instance.AddActionInGameToList(newCommand);

        /*        GlobalManager.Instance.DestroyAllCommandsOfPlayer(targetPlayer);

                CommandInGame newCommand = null;

                switch (typeOfChoice)
                {
                    case(EnumClass.ChoiceType.Weapon):

                        newCommand = new CommandWeaponChoice(targetPlayer, typeOfWeapon);

                        break;

                    case(EnumClass.ChoiceType.WeaponBuff):

                        newCommand = new CommandWeaponBuffChoice(targetPlayer, typeOfWeaponBuff);
                        break;

                    case(EnumClass.ChoiceType.MovementBuff):

                        newCommand = new CommandMovementBuffChoice(targetPlayer, typeOfMovementBuff);
                        break;
                }

                GlobalManager.Instance.AddActionInGameToList(newCommand);*/

    }

}

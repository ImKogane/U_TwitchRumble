using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SO/Choice")]
public abstract class SO_Choice : ScriptableObject
{
    [Header("Sprite of card")]

    public Sprite _cardSprite;

    public virtual void StartAChoice(Player ownerOfBuff)
    {

    }

    public virtual void ApplyChoice(Player targetPlayer)
    {
        GlobalManager.Instance.DestroyAllCommandsOfPlayer(targetPlayer);

        CommandInGame newCommand = new CommandChoice(targetPlayer, this);

        GlobalManager.Instance.AddActionInGameToList(newCommand);
    }

}

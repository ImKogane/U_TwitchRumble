using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/BuffMoving Data/TrapMoveBuff Data")]
public class SO_TrapMovementBuff : SO_BuffMoving
{
    public override void ApplyMoveBuff(Player ownerOfBuff)
    {
        CommandTrap trapCommand = new CommandTrap(ownerOfBuff);
        GlobalManager.Instance._listCommandsInGame.Insert(1, trapCommand);
    }

}

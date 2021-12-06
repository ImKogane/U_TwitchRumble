using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/BuffMoving Data/SpringMoveBuff Data")]
public class SO_SpringMovementBuff : SO_BuffMoving
{
    public virtual void StartAChoice(Player ownerOfBuff)
    {
        ownerOfBuff.playerMovement.CanJumpObstacle = true;
    }
    
}

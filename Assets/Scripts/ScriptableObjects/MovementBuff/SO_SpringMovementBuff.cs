using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/BuffMoving Data/SpringMoveBuff Data")]
public class SO_SpringMovementBuff : SO_BuffMoving
{
    public override void StartAChoice(Player ownerOfBuff)
    {
        ownerOfBuff._playerMovementComponent.CanJumpObstacle = true;
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringMoveBuff : MoveBuff
{
    public SpringMoveBuff(Player playerToBuff) : base(playerToBuff)
    {
        playerToBuff.playerMovement.CanJumpObstacle = true;
    }
}

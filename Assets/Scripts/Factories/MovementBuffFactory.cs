using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MovementBuffFactory
{
    public static MoveBuff CreateMovementBuff(EnumClass.MovementBuffType movementBuffType, Player targetPlayer)
    {
        switch (movementBuffType)
        {
            case(EnumClass.MovementBuffType.Trap):
                return new TrapMoveBuff(targetPlayer);
                break;
            
            case(EnumClass.MovementBuffType.Spring):
                return new SpringMoveBuff(targetPlayer);
                break;
            
            case(EnumClass.MovementBuffType.Magnet):
                return new MagnetMoveBuff(targetPlayer);
                break;
            
        }

        return null;
    }
}

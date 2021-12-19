using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/BuffMoving Data")]
[System.Serializable]
public abstract class SO_BuffMoving : SO_Choice
{
    public EnumClass.MovementBuffType movementBuffType;
    

    public virtual void ApplyMoveBuff(Player ownerOfBuff)
    {
        
    }
}

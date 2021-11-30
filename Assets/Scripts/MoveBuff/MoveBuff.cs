using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBuff 
{
    Player ownerOfMoveBuff;

    public MoveBuff(Player playerToBuff)
    {
        ownerOfMoveBuff = playerToBuff;
    }   

    public virtual void ApplyMoveBuff()
    {

    }
}

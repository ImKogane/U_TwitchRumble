

using UnityEditor.SceneTemplate;
using UnityEngine;

public class TrapMoveBuff : MoveBuff
{
    public int trapDamages = 15;

    public TrapMoveBuff(Player playerToBuff) : base(playerToBuff)
    {

    }

    public override void ApplyMoveBuff()
    {
        Tile currentTile = ownerOfMoveBuff.CurrentTile;
        GameObject newTrapGO = GameObject.Instantiate(ownerOfMoveBuff.trapPrefab, currentTile.transform);
        Trap trapComponent = newTrapGO.GetComponent<Trap>();
        if (trapComponent)
        {
            currentTile.trapList.Add(trapComponent);
            trapComponent.currentTile = currentTile;
        }

        //Play SFX
    }

}

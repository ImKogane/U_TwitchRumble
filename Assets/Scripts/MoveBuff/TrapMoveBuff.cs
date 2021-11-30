

using UnityEditor.SceneTemplate;
using UnityEngine;

public class TrapMoveBuff : MoveBuff
{
    public int trapDamages = 15;

    public Trap trapPrefab;
    
    public TrapMoveBuff(Player playerToBuff) : base(playerToBuff)
    {

    }

    public override void ApplyMoveBuff()
    {
        Tile currentTile = ownerOfMoveBuff.CurrentTile;
        Trap newTrap = GameObject.Instantiate(trapPrefab, currentTile.transform);
        currentTile.trapList.Add(newTrap);
        newTrap.currentTile = currentTile;

        //Play SFX
    }

}

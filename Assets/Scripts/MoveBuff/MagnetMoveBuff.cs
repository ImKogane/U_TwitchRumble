using System.Collections.Generic;
using UnityEngine;
public class MagnetMoveBuff : MoveBuff
{
    public MagnetMoveBuff(Player playerToBuff) : base(playerToBuff)
    {
        
    }

    public override void ApplyMoveBuff()
    {
        AttiranceOnRight(new Vector2Int(-1, 0));
        AttiranceOnRight(new Vector2Int(1, 0));
        AttiranceOnRight(new Vector2Int(0, 1));
        AttiranceOnRight(new Vector2Int(0, -1));
    }

    public void AttiranceOnRight(Vector2Int VectorAttirance)
    {
        Tile startTile = ownerOfMoveBuff.CurrentTile;

        if (VectorAttirance.x != 0) //Left and Right
        {
            for (int i = 15; i > 2; i--)
            {
                Tile currentTile = BoardManager.Instance.GetTileAtPos(new Vector2Int(startTile.tileRow + (i * VectorAttirance.x), startTile.tileColumn));
                if (currentTile != null)
                {
                    if (currentTile.hasPlayer)
                    {
                        Debug.Log($"New MagneticCommand place in List, owner : {ownerOfMoveBuff.name}, vector : {VectorAttirance}, playerAffect : {currentTile.currentPlayer.name}");
                        MagneticCommand magneticCommand = new MagneticCommand(ownerOfMoveBuff, VectorAttirance, currentTile);
                        GlobalManager.Instance.ListCommandsInGame.Insert(1, magneticCommand);
                    }
                }
                else
                {
                    //Tomber dans l'eau
                    MagneticCommand magneticCommand = new MagneticCommand(ownerOfMoveBuff, VectorAttirance, null);
                    GlobalManager.Instance.ListCommandsInGame.Insert(1, magneticCommand);
                }
            }
        }

        if (VectorAttirance.y != 0) //Left and Right
        {
            for (int i = 15; i > 2; i--)
            {
                Tile currentTile = BoardManager.Instance.GetTileAtPos(new Vector2Int(startTile.tileRow , startTile.tileColumn + (i * VectorAttirance.y)));
                if (currentTile != null)
                {
                    if (currentTile.hasPlayer)
                    {
                        Debug.Log($"New MagneticCommand place in List, owner : {ownerOfMoveBuff.name}, vector : {VectorAttirance}, playerAffect : {currentTile.currentPlayer.name}");
                        MagneticCommand magneticCommand = new MagneticCommand(ownerOfMoveBuff, VectorAttirance, currentTile);
                        GlobalManager.Instance.ListCommandsInGame.Insert(1, magneticCommand);
                    }
                }
                else
                {
                    //Tomber dans l'eau
                    MagneticCommand magneticCommand = new MagneticCommand(ownerOfMoveBuff, VectorAttirance, null);
                    GlobalManager.Instance.ListCommandsInGame.Insert(1, magneticCommand);
                }
            }
        }
    }
}



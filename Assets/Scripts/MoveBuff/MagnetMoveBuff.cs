using System.Collections.Generic;
using UnityEngine;
public class MagnetMoveBuff : MoveBuff
{
    public MagnetMoveBuff(Player playerToBuff) : base(playerToBuff)
    {
        
    }

    public override void ApplyMoveBuff()
    {
        
        List<Tile> tileToApply = new List<Tile>();

        
    }

    public void AttiranceOnRight(Vector2Int VectorAttirance)
    {
        Tile startTile = ownerOfMoveBuff.CurrentTile;

        if (VectorAttirance.x != 0) //Left and Right
        {
            for (int i = 2; i < 15; i++)
            {
                Tile currentTile = BoardManager.Instance.GetTileAtPos(new Vector2Int(startTile.tileRow + (i * VectorAttirance.x), startTile.tileColumn));
                if (currentTile != null)
                {
                    if (currentTile.hasPlayer)
                    {
                        //Get player infos.
                        PlayerMovement _currentMovementPlayer = currentTile.currentPlayer.playerMovement;
                        Vector2Int oldRotOfPlayer = _currentMovementPlayer.RotationOfPlayer;

                        //Move the player.
                        _currentMovementPlayer.RotationOfPlayer = VectorAttirance;
                        _currentMovementPlayer.CheckBeforeMoveToATile(BoardManager.Instance.GetTileAtPos(new Vector2Int(currentTile.tileRow + (-VectorAttirance.x), startTile.tileColumn)));

                        //Reset pmd rptation of player
                        _currentMovementPlayer.RotationOfPlayer = oldRotOfPlayer;
                    }
                }
            }
        }
    }
}



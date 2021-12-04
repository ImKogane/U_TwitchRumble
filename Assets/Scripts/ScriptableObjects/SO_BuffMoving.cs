using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/BuffMoving Data")]
public class SO_BuffMoving : SO_Choice
{
    public EnumClass.MovementBuffType buffType;


    public override void StartAChoice(Player ownerOfBuff)
    {
        if (buffType == EnumClass.MovementBuffType.Spring)
        {
            ownerOfBuff.playerMovement.CanJumpObstacle = true;
        }
    }

    public virtual void ApplyMoveBuff(Player ownerOfBuff)
    {
        switch (buffType)
        {
            case EnumClass.MovementBuffType.Trap:

                ownerOfBuff.playerAnimator.SetTrigger("IsPlacingTrap");
                break;

            case EnumClass.MovementBuffType.Magnet:
                AttiranceOnSpecificSide(new Vector2Int(-1, 0), ownerOfBuff);
                AttiranceOnSpecificSide(new Vector2Int(1, 0), ownerOfBuff);
                AttiranceOnSpecificSide(new Vector2Int(0, 1), ownerOfBuff);
                AttiranceOnSpecificSide(new Vector2Int(0, -1), ownerOfBuff);
                break;

            default:
                break;
        }
    }

    public void AttiranceOnSpecificSide(Vector2Int VectorAttirance, Player ownerOfBuff)
    {
        Tile startTile = ownerOfBuff.CurrentTile;

        if (VectorAttirance.x != 0) //Left and Right
        {
            for (int i = 15; i > 2; i--)
            {
                Tile currentTile = BoardManager.Instance.GetTileAtPos(new Vector2Int(startTile.tileRow + (i * VectorAttirance.x), startTile.tileColumn));

                if (currentTile != null)
                {
                    if (currentTile.hasPlayer)
                    {
                        Debug.Log($"New MagneticCommand place in List, owner : {ownerOfBuff.name}, vector : {VectorAttirance}, playerAffect : {currentTile.currentPlayer.name}");
                        MagneticCommand magneticCommand = new MagneticCommand(ownerOfBuff, VectorAttirance, currentTile);
                        GlobalManager.Instance.ListCommandsInGame.Insert(1, magneticCommand);
                    }
                }

            }
        }

        if (VectorAttirance.y != 0) //Left and Right
        {
            for (int i = 15; i > 2; i--)
            {
                Tile currentTile = BoardManager.Instance.GetTileAtPos(new Vector2Int(startTile.tileRow, startTile.tileColumn + (i * VectorAttirance.y)));
                if (currentTile != null)
                {
                    if (currentTile.hasPlayer)
                    {
                        Debug.Log($"New MagneticCommand place in List, owner : {ownerOfBuff.name}, vector : {VectorAttirance}, playerAffect : {currentTile.currentPlayer.name}");
                        MagneticCommand magneticCommand = new MagneticCommand(ownerOfBuff, VectorAttirance, currentTile);
                        GlobalManager.Instance.ListCommandsInGame.Insert(1, magneticCommand);
                    }
                }
            }
        }
    }
}

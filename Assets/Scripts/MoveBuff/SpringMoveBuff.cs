
public class SpringMoveBuff : MoveBuff
{
    public SpringMoveBuff(Player playerToBuff) : base(playerToBuff)
    {
        playerToBuff.playerMovement.CanJumpObstacle = true;
    }
}

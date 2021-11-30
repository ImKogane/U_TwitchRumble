
public class MoveBuff 
{
    protected Player ownerOfMoveBuff;

    public MoveBuff(Player playerToBuff)
    {
        ownerOfMoveBuff = playerToBuff;
    }   

    public virtual void ApplyMoveBuff()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [SerializeField]
    public List<Player> PlayerList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void MoveAllPlayer(EnumClass.Direction DirectionOfMovement)
    {
        foreach (Player player in PlayerList)
        {
            switch (DirectionOfMovement)
            {
                case EnumClass.Direction.Up:
                    player.RotateUpDirection();
                    break;
                case EnumClass.Direction.Down:
                    player.RotateDownDirection();
                    break;
                case EnumClass.Direction.Right:
                    player.RotateRightDirection();
                    break;
                case EnumClass.Direction.Left:
                    player.RotateLeftDirection();
                    break;
                default:
                    break;
            }
            player.MakeMovement();
        }
        
    }
}

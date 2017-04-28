using UnityEngine;
using System.Collections;

public class PlayerState : MonoBehaviour
{
    private static PlayerState _instance;
    public static PlayerState Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameObject("PlayerState").AddComponent<PlayerState>();

            return _instance;
        }
    }

    public Horizontal Horizontal;
    public Vertical Vertical;
    public DirectionFacing DirectionFacing;
}

public enum Horizontal
{
    Idle = 0,
    MovingLeft = -1,
    MovingRight = 1
}

public enum Vertical
{
    Grounded,
    Airborne
}

public enum DirectionFacing
{
    Left = -1,
    Right = 1
}


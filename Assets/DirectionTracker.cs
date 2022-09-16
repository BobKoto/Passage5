using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class  DirectionTracker
{
    public static bool wasMovingNorth, wasMovingSouth, wasMovingEast, wasMovingWest;

    public static void  RegisterDirection(bool north, bool south, bool east, bool west)
    {
        wasMovingNorth = north; wasMovingSouth = south; wasMovingEast = east; wasMovingWest = west;

        Debug.Log("Direction tracker report {0}, {1}, {2}, {3}" + north + south + east + west);
    }
    public static string MovingInDirection()
    {
        string movingInDirection = "empty";

        if (wasMovingNorth)
        {
            movingInDirection = "north";
          //  return movingInDirection;
        }
        else
        if (wasMovingSouth)
        {
            movingInDirection = "south";
          //  return movingInDirection;
        }
        else
        if (wasMovingEast)
        {
            movingInDirection = "east";
            //  return movingInDirection;
        }
        else
        if (wasMovingWest)
        {
            movingInDirection = "west";
            //  return movingInDirection;
        }

        Debug.Log("MovingInDirection reports " + movingInDirection  + "  out of itself");
        return movingInDirection;
    }
}

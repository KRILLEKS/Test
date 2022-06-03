using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Constants
{
   // for A* pathfinding
   public const int MOVE_STRAIGHT_COST = 10;
   public const int MOVE_DIAGONAL_COST = 14;
      
   public static readonly int2 GRID_SIZE = new int2 (20,20);
}

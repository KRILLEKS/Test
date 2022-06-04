using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Constants
{
   // means that grid will be displaces on this value
   public static readonly Vector3 OFFSET = new Vector3(.5f, 0, .5f);
   // one tile size
   public const int CELL_SIZE = 1;
   // determines on which height (layer) player is located
   public const int PLAYER_Y = 1;

   // for A* pathfinding
   public const int MOVE_STRAIGHT_COST = 10;
   public const int MOVE_DIAGONAL_COST = 14;
      
   public static readonly int2 GRID_SIZE = new int2 (20,20);
}

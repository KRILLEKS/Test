using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// not only pathfinding will use it but other classes also need information if tile is walkable
public static class GridHandler
{
   // this array shows which nodes are walkable and which are not
   // generate grid will initialize this array
   public static readonly bool[] isNodeWalkable = new bool[Constants.GRID_SIZE.x * Constants.GRID_SIZE.y];

   // based on index
   public static Vector3 CalculatePosition(int index)
   {
      return new Vector3(index % Constants.GRID_SIZE.x, 0, index / Constants.GRID_SIZE.x);
   }

#region CalculateDistanceCost

   /// <summary>
   /// floor val to int and also uses z as y
   /// </summary>
   /// <param name="aPos"></param>
   /// <param name="bPos"></param>
   /// <returns></returns>
   public static int CalculateDistanceCost(Vector3 aPos, Vector3 bPos)
   {
      return CalculateDistanceCost(new int2(Mathf.FloorToInt(aPos.x), Mathf.FloorToInt(aPos.z)),
                                    new int2(Mathf.FloorToInt(bPos.x), Mathf.FloorToInt(bPos.z)));
   }

   public static int CalculateDistanceCost(int2 aPos, int2 bPos)
   {
      int xDistance = math.abs(aPos.x - bPos.x);
      int yDistance = math.abs(aPos.y - bPos.y);
      int remaining = math.abs(xDistance - yDistance);

      return Constants.MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + Constants.MOVE_STRAIGHT_COST * remaining;
   }

#endregion

#region CalculateIndex

   /// <summary>
   /// floor val to int and also uses z as y
   /// </summary>
   /// <param name="val"></param>
   /// <returns></returns>
   public static int CalculateIndex(Vector3 val)
   {
      return CalculateIndex(Mathf.FloorToInt(val.x), Mathf.FloorToInt(val.z));
   }

   public static int CalculateIndex(int2 val)
   {
      return CalculateIndex(val.x, val.y);
   }

   public static int CalculateIndex(int x, int y)
   {
      return x + y * Constants.GRID_SIZE.x;
   }

#endregion

#region IsPositionInsideGrid

   /// <summary>
   /// floor val to int and also uses z as y
   /// </summary>
   /// <param name="cellPosition"></param>
   /// <returns></returns>
   public static bool IsPositionInsideGrid(Vector3 cellPosition)
   {
      return IsPositionInsideGrid(new int2(Mathf.FloorToInt(cellPosition.x), Mathf.FloorToInt(cellPosition.z)));
   }

   public static bool IsPositionInsideGrid(int2 cellPosition)
   {
      return cellPosition.x >= 0 && cellPosition.x < Constants.GRID_SIZE.x &&
             cellPosition.y >= 0 && cellPosition.y < Constants.GRID_SIZE.y;
   }

#endregion
}
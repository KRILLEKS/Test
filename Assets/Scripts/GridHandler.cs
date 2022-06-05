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
   
   // based on index
   public static Vector3 CalculatePosition(int index)
   {
      return new Vector3(index % Constants.GRID_SIZE.x, 0,index / Constants.GRID_SIZE.x);
   }
   
   /// <summary>
   /// floor val to int and also uses z as y
   /// </summary>
   /// <param name="cellPosition"></param>
   /// <returns></returns>
   public static bool isPositionInsideGrid(Vector3 cellPosition)
   {
      return isPositionInsideGrid(new int2(Mathf.FloorToInt(cellPosition.x), Mathf.FloorToInt(cellPosition.z)));
   }
   public static bool isPositionInsideGrid(int2 cellPosition)
   {
      return cellPosition.x >= 0 && cellPosition.x < Constants.GRID_SIZE.x &&
             cellPosition.y >= 0 && cellPosition.y < Constants.GRID_SIZE.y;
   }

}
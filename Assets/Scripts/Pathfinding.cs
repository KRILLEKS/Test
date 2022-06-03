using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

// I'll use DOTS to make pathfinding (using A* pathfinding algorithm) cause it'll have better performance
// Although I can make it without DOTS
public class Pathfinding : MonoBehaviour
{
   private struct PathNode
   {
      public int x;
      public int y;

      public int index; // index in array

      public int gCost; // walking cost from the start node
      public int hCost; // heuristic cost (minimum possible cost to reach end from this point)
      public int fCost; // g + h

      public bool isWalkable;

      public int cameFromNodeIndex; // to inverse algorithm and find path

      public void CalculateFCost()
      {
         fCost = gCost + hCost;
      }
   }

   private void FindPath(int2 startPosition, int2 endPosition)
   {
      NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>(Constants.GRID_SIZE.x * Constants.GRID_SIZE.y, Allocator.Temp);

      // initialize array
      for (int x = 0; x < Constants.GRID_SIZE.x; x++)
         for (int y = 0; y < Constants.GRID_SIZE.y; y++)
         {
            PathNode pathNode = new PathNode();
            pathNode.x = x;
            pathNode.y = y;
            pathNode.index = CalculateIndex(x, y, Constants.GRID_SIZE.x);

            // TODO: gCost looks strange for me
            pathNode.gCost = int.MaxValue; // we will change this so value doesn't really matter
            pathNode.hCost = CalculateDistanceCost(new int2(x, y), endPosition);
            pathNode.CalculateFCost();

            pathNode.isWalkable = true;
            pathNode.cameFromNodeIndex = -1; // we'll use -1 as invalid value

            pathNodeArray[pathNode.index] = pathNode;
         }

      PathNode startNode = pathNodeArray[CalculateIndex(startPosition.x, startPosition.y, Constants.GRID_SIZE.x)];
      startNode.gCost = 0;
      startNode.CalculateFCost();
      pathNodeArray[startNode.index] = startNode;
      
      
      
      pathNodeArray.Dispose();

      int CalculateIndex(int x, int y, int gridWidth)
      {
         return x + y * gridWidth;
      }

      int CalculateDistanceCost(int2 aPos, int2 bPos)
      {
         int xDistance = math.abs(aPos.x - bPos.x);
         int yDistance = math.abs(aPos.y - bPos.y);
         int remaining = math.abs(xDistance - yDistance);

         return Constants.MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + Constants.MOVE_STRAIGHT_COST * remaining;
      }
   }
}
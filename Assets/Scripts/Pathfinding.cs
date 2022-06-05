using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;
using System.Runtime.CompilerServices;

// I wanted to use DOTS to make pathfinding (using A* pathfinding algorithm) cause it'll have better performance
// but I saw that ECS is forbidden although my solution won't be too different
// I'll use list/array instead of Native list/array and I won't use jobs
public class Pathfinding
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

      public int2 Position()
      {
         return new int2(x, y);
      }
   }

   private static readonly int2[] neighbourOffsetsArray =
   {
      new int2(1, 0), // right
      new int2(-1, 0), // left 
      new int2(0, 1), // up
      new int2(0, -1), // down
      new int2(1, 1), // right up
      new int2(-1, 1), // left up
      new int2(1, -1), // right down
      new int2(-1, -1), // left down
   };

   public static List<Vector3> FindPath(int2 startPosition, int2 endPosition)
   {
      if (isPositionInsideGrid(endPosition) == false ||
          GridHandler.isNodeWalkable[GridHandler.CalculateIndex(endPosition)] == false)
         return new List<Vector3>();
      
      PathNode[] pathNodeArray = new PathNode[Constants.GRID_SIZE.x * Constants.GRID_SIZE.y];

      InitializeArray();
      

      PathNode startNode = pathNodeArray[GridHandler.CalculateIndex(startPosition)];
      startNode.gCost = 0;
      startNode.CalculateFCost();
      pathNodeArray[startNode.index] = startNode;

      int endNodeIndex = GridHandler.CalculateIndex(endPosition);

      List<int> openList = new List<int>();
      List<int> closedList = new List<int>();

      openList.Add(startNode.index);

      // pathfinding itself
      while (openList.Count > 0)
      {
         int currentNodeIndex = GetLowestFCostNodeIndex(openList, pathNodeArray);
         PathNode currentNode = pathNodeArray[currentNodeIndex];

         // reached end
         if (currentNode.index == endNodeIndex)
         {
            break;
         }

         // remove current node from list
         openList.RemoveAt(openList.FindIndex(_ => _ == currentNodeIndex));

         closedList.Add(currentNodeIndex);

         // check neighbours
         for (int i = 0; i < neighbourOffsetsArray.Length; i++)
         {
            int2 neighbourPos = new int2(currentNode.x + neighbourOffsetsArray[i].x, currentNode.y + neighbourOffsetsArray[i].y);
            int neighbourIndex = GridHandler.CalculateIndex(neighbourPos);
            
            if (isPositionInsideGrid(neighbourPos) == false)
               continue;
            
            PathNode neighbourNode = pathNodeArray[neighbourIndex];

            // check if node is valid
            if (closedList.Contains(neighbourIndex) ||
                pathNodeArray[neighbourIndex].isWalkable == false)
               continue;

            // change neighbour node values
            int currentGCost = currentNode.gCost + CalculateDistanceCost(currentNode.Position(), neighbourPos);
            if (currentGCost < neighbourNode.gCost)
            {
               neighbourNode.cameFromNodeIndex = currentNodeIndex;
               neighbourNode.gCost = currentGCost;
               neighbourNode.CalculateFCost();
               pathNodeArray[neighbourIndex] = neighbourNode;

               if (openList.Contains(neighbourIndex) == false)
                  openList.Add(neighbourIndex);
            }
         }
      }

      PathNode endNode = pathNodeArray[endNodeIndex];
      if (endNode.cameFromNodeIndex != -1)
      {
         List<Vector3> path = CalculatePath();
         path.Reverse();
         return path;
      }

      return null;

      void InitializeArray()
      {
         for (int x = 0; x < Constants.GRID_SIZE.x; x++)
            for (int y = 0; y < Constants.GRID_SIZE.y; y++)
            {
               PathNode pathNode = new PathNode();
               pathNode.x = x;
               pathNode.y = y;
               pathNode.index = GridHandler.CalculateIndex(x, y);

               pathNode.gCost = int.MaxValue; // we will change this so value doesn't really matter
               pathNode.hCost = CalculateDistanceCost(new int2(x, y), endPosition);
               pathNode.CalculateFCost();

               pathNode.isWalkable = GridHandler.isNodeWalkable[pathNode.index];
               pathNode.cameFromNodeIndex = -1; // we'll use -1 as invalid value

               pathNodeArray[pathNode.index] = pathNode;
            }
      }



      int CalculateDistanceCost(int2 aPos, int2 bPos)
      {
         int xDistance = math.abs(aPos.x - bPos.x);
         int yDistance = math.abs(aPos.y - bPos.y);
         int remaining = math.abs(xDistance - yDistance);

         return Constants.MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + Constants.MOVE_STRAIGHT_COST * remaining;
      }

      int GetLowestFCostNodeIndex(List<int> openList, PathNode[] pathNodeArray)
      {
         PathNode lowestCostNode = pathNodeArray[openList[0]];

         for (int i = 1; i < openList.Count; i++)
         {
            PathNode currentNode = pathNodeArray[openList[i]];

            if (currentNode.fCost < lowestCostNode.fCost)
               lowestCostNode = currentNode;
         }

         return lowestCostNode.index;
      }

      bool isPositionInsideGrid(int2 gridPosition)
      {
         return gridPosition.x >= 0 && gridPosition.x < Constants.GRID_SIZE.x &&
                gridPosition.y >= 0 && gridPosition.y < Constants.GRID_SIZE.y;
      }

      // we don't include start pos
      List<Vector3> CalculatePath()
      {
         var path = new List<Vector3>();
         PathNode currentNode = endNode;
         
         while (currentNode.cameFromNodeIndex != -1)
         {
            PathNode cameFromNode = pathNodeArray[currentNode.cameFromNodeIndex];
            path.Add(new Vector3(currentNode.x,Constants.PLAYER_Y, currentNode.y));
            currentNode = cameFromNode;
         }

         return path;
      }
   }
}
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;

// I wanted to use DOTS to make pathfinding (using A* pathfinding algorithm) cause it'll have better performance
// but I saw that ECS is forbidden although my solution won't be too different
// I'll use list/array instead of Native list/array and I won't use jobs
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

      public int2 Position()
      {
         return new int2(x, y);
      }
   }

   private readonly int2[] neighbourOffsetsArray =
   {
      new (1, 0), // right
      new (-1, 0), // left 
      new (0, 1), // up
      new (0, -1), // down
      new (1, 1), // right up
      new (-1, 1), // left up
      new (1, -1), // right down
      new (-1, -1), // left down
   };

   private void FindPath(int2 startPosition, int2 endPosition)
   {
      PathNode[] pathNodeArray = new PathNode[Constants.GRID_SIZE.x * Constants.GRID_SIZE.y];

      InitializeArray();

      PathNode startNode = pathNodeArray[CalculateIndex(startPosition.x, startPosition.y)];
      startNode.gCost = 0;
      startNode.CalculateFCost();
      pathNodeArray[startNode.index] = startNode;

      int endNodeIndex = CalculateIndex(endPosition.x, endPosition.y);
      PathNode endNode = pathNodeArray[endNodeIndex];

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
            break;

         // remove current node from list
         openList.RemoveAt(openList.FindIndex(_ => _ == currentNodeIndex));

         closedList.Add(currentNodeIndex);

         // check neighbours
         for (int i = 0; i < neighbourOffsetsArray.Length; i++)
         {
            int2 neighbourPos = new int2(currentNode.x + neighbourOffsetsArray[i].x, currentNode.y + neighbourOffsetsArray[i].y);
            int neighbourIndex = CalculateIndex(neighbourPos.x, neighbourPos.y);
            PathNode neighbourNode = pathNodeArray[neighbourIndex];

            // check if node is valid
            if (isPositionInsideGrid(neighbourPos) == false ||
                closedList.Contains(neighbourIndex) ||
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

      if (endNode.cameFromNodeIndex != -1)
      {
         List<int2> path = CalculatePath();
         path.Reverse();
      }

      void InitializeArray()
      {
         for (int x = 0; x < Constants.GRID_SIZE.x; x++)
            for (int y = 0; y < Constants.GRID_SIZE.y; y++)
            {
               PathNode pathNode = new PathNode();
               pathNode.x = x;
               pathNode.y = y;
               pathNode.index = CalculateIndex(x, y);

               // TODO: gCost looks strange for me
               pathNode.gCost = int.MaxValue; // we will change this so value doesn't really matter
               pathNode.hCost = CalculateDistanceCost(new int2(x, y), endPosition);
               pathNode.CalculateFCost();

               pathNode.isWalkable = true;
               pathNode.cameFromNodeIndex = -1; // we'll use -1 as invalid value

               pathNodeArray[pathNode.index] = pathNode;
            }
      }

      int CalculateIndex(int x, int y)
      {
         return x + y * Constants.GRID_SIZE.x;
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

      List<int2> CalculatePath()
      {
         var path = new List<int2>();
         path.Add(new int2 (endNode.x,endNode.y));

         PathNode currentNode = endNode;
         while (currentNode.cameFromNodeIndex != -1)
         {
            PathNode cameFromNode = pathNodeArray[currentNode.cameFromNodeIndex];
            path.Add(cameFromNode.index);
            currentNode = cameFromNode;
         }

         return path;
      }
   }
}
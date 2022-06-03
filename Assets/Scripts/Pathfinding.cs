using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I'll use DOTS to make pathfinding (using A* pathfinding algorithm) cause it'll have better performance
// Although I can make it without DOTS
public class Pathfinding : MonoBehaviour
{
   private struct PathNode
   {
      public int x;
      public int y;

      public int index;

      public int gCost; // walking cost from the start node
      public int hCost; // heuristic cost (minimum possible cost to reach end from this point)
      public int fCost; // g + h

      public bool isWalkable;
   }
}

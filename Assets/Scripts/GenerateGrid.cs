using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class GenerateGrid : MonoBehaviour
{
   [Header("Ground(0) layer")]
   // ground tile (to walk on)
   [FormerlySerializedAs("tile"), SerializeField]
   private GameObject groundTile;
   // all tiles will be attached to this GO to make it more convenient
   [SerializeField] private GameObject groundTilesHolder;

   [Header("Obstacles(1) layer")]
   [SerializeField] private GameObject borderTile;
   [SerializeField] private GameObject borderTilesHolder;
   [Space]
   // determines size of obstacles in a line
   [SerializeField] private int minLength;
   [SerializeField] private int maxLenght;
   [SerializeField, Range(0, .3f)] private float spawnChance;

   private void Awake()
   {
      // to prevent some possible issues
      groundTilesHolder.transform.position = Vector3.zero;
      borderTilesHolder.transform.position = Vector3.zero;

      for (int i = 0; i < GridHandler.isNodeWalkable.Length; i++)
         GridHandler.isNodeWalkable[i] = true;

      GenerateGroundLayer(); // 0
      GenerateObstaclesLayer(); // 1

      void GenerateGroundLayer()
      {
         for (int x = 0; x < Constants.GRID_SIZE.x; x++)
            for (int z = 0; z < Constants.GRID_SIZE.y; z++)
            {
               Instantiate(groundTile,
                           Constants.OFFSET + new Vector3(x * Constants.CELL_SIZE, 0, z * Constants.CELL_SIZE),
                           Quaternion.identity,
                           groundTilesHolder.transform);
            }
      }

      // it can be fixed but I decided to make procedural generation to make it a bit more random
      void GenerateObstaclesLayer()
      {
         // determine offset (direction). You can add diagonals either
         int2[] directions = new int2[]
         {
            new int2(1, 0), // right
            new int2(-1, 0), // left
            new int2(0, 1), // up
            new int2(0, -1) // down
         };
         
         for (int x = 0; x < Constants.GRID_SIZE.x; x++)
            for (int y = 0; y < Constants.GRID_SIZE.y; y++)
               // spawn obstacles
               if (spawnChance >= Random.Range(0, 1f))
               {
                  // +1 cause max value isn't included
                  int size = Random.Range(minLength, maxLenght + 1);
                  int2 direction = directions[Random.Range(0, directions.Length)];

                  Debug.Log(size);
                  for (int i = 0; i < size; i++)
                  {
                     Vector3 position = new Vector3(x, Constants.PLAYER_Y, y) + new Vector3(i * direction.x, 0, i * direction.y);
                     int index = GridHandler.CalculateIndex(position);

                     if (GridHandler.isPositionInsideGrid(position) && GridHandler.isNodeWalkable[index])
                     {
                        Instantiate(borderTile, position + Constants.OFFSET, Quaternion.identity, borderTilesHolder.transform);
                        GridHandler.isNodeWalkable[index] = false;
                     }
                  }
               }
      }
   }
}
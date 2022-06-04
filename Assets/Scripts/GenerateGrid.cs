using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
   // ground tile (to walk on)
   [SerializeField] private GameObject tile;
   // all tiles will be attached to this GO to make it more convenient
   [SerializeField] private GameObject groundTilesHolder;

   private void Awake()
   {
      // to prevent some possible issues
      groundTilesHolder.transform.position = Vector3.zero;
      
      for (int x = 0; x < Constants.GRID_SIZE.x; x++)
         for (int z = 0; z < Constants.GRID_SIZE.y; z++)
         {
            Instantiate(tile, 
                        Constants.OFFSET + new Vector3(x * Constants.CELL_SIZE, 0, z * Constants.CELL_SIZE),
                        Quaternion.identity,
                        groundTilesHolder.transform);
         }
   }
}
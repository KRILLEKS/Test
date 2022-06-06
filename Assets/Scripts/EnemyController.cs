using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
   [SerializeField] private float speed;
   // determines how frequently enemy moves. set to 0 to remove delay
   [SerializeField] private float movementRate;
   
   // private variables
   private int _index = 0;
   private Vector3[] _neighbours =
   {
      new Vector3(1, 0, 0),
      new Vector3(0, 0, 1),
      new Vector3(-1, 0, 0),
      new Vector3(0, 0, -1),
   };

   private void Awake()
   {
      StartCoroutine(walkCoroutine());
   }

   private IEnumerator walkCoroutine()
   {
      while (true)
      {
         // initialize
         Vector3 movePos = GetMovePosition();
         GridHandler.isNodeWalkable[_index] = true; // we want to "unlock" tile under enemy cause he'll move

         // movement
         while (transform.position != movePos)
         {
            transform.position = Vector3.MoveTowards(transform.position, movePos, Time.deltaTime * speed);
            yield return null;
         }

         // onStop
         // we can't move or go through enemy so we "lock" tile under enemy
         _index = GridHandler.CalculateIndex(transform.position);
         GridHandler.isNodeWalkable[_index] = false;
         
         // enemy destroys crystal
         CrystalGenerator.Try2DestroyCrystal(_index);
         
         yield return new WaitForSeconds(movementRate);
      }

      Vector3 GetMovePosition()
      {
         List<Vector3> possibleMoves = new List<Vector3>();
         for (int i = 0; i < _neighbours.Length; i++)
         {
            int neighbourIndex = GridHandler.CalculateIndex(transform.position + _neighbours[i]);

            if (GridHandler.isPositionInsideGrid(transform.position + _neighbours[i]) && GridHandler.isNodeWalkable[neighbourIndex])
               possibleMoves.Add(transform.position + _neighbours[i]);
         }

         return possibleMoves[Random.Range(0, possibleMoves.Count)];
      }
   }

   // invokes on collision
   // collisions handled by playerController
   public void Destroy()
   {
      // we want to "unlock" tile
      GridHandler.isNodeWalkable[_index] = true;
      EnemySpawner.enemiesAmount--;
      Destroy(gameObject); // coroutine will be destroyed with this obj
   }
}
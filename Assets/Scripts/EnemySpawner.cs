using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
   // spawn points will be randomly determined at the start of the game
   // and used as a spawn point
   [SerializeField] private int spawnPointsAmount;
   [SerializeField] private float spawnRate; // in seconds
   [SerializeField] private int maxEnemiesCount;
   [SerializeField] private GameObject spawnPointPrefab;
   [SerializeField] private GameObject enemyPrefab;
   [SerializeField] private GameObject enemyHolder;
   
   // public static variables
   public static List<GameObject> enemiesList = new List<GameObject>();
   // private variables
   private List<int> _spawnPoints = new List<int>(); // indexes

   private void Awake()
   {
      enemyHolder.transform.position = Vector3.zero;
      
      GetSpawnPoints();

      StartCoroutine(SpawnEnemyCoroutine());
      
      void GetSpawnPoints()
      {
         List<int> possiblePositions = new List<int>();

         for (int i = 0; i < GridHandler.isNodeWalkable.Length; i++)
            if (GridHandler.isNodeWalkable[i])
               possiblePositions.Add(i);

         for (int i = 0; i < spawnPointsAmount; i++)
         {
            int randomVal = possiblePositions[Random.Range(0, possiblePositions.Count)];
            _spawnPoints.Add(randomVal);
            possiblePositions.Remove(randomVal);

            Instantiate(spawnPointPrefab,
                        GridHandler.CalculatePosition(randomVal) + Constants.OFFSET + new Vector3(0, Constants.PLAYER_Y, 0),
                        Quaternion.identity,
                        enemyHolder.transform);
         }
      }
   }

   private IEnumerator SpawnEnemyCoroutine()
   {
      while (true)
      {
         if (enemiesList.Count >= maxEnemiesCount)
         {
            yield return null;
            continue;
         }

         int spawnPointIndex = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
         Vector3 position = GridHandler.CalculatePosition(spawnPointIndex) + Constants.OFFSET + new Vector3(0,Constants.PLAYER_Y,0);

         var enemy = Instantiate(enemyPrefab, position, quaternion.identity, enemyHolder.transform);
         enemiesList.Add(enemy);

         yield return new WaitForSeconds(spawnRate);
      }
   }
}
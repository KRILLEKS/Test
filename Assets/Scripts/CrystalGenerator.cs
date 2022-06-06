using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrystalGenerator : MonoBehaviour
{
   [SerializeField] private int startCrystalAmount;
   [SerializeField] private int maxCrystalAmount;
   [SerializeField] private float crystalGenerationSpeed; // in seconds
   [Space]
   [SerializeField] private GameObject crystalPrefab; // money
   [SerializeField] private GameObject crystalHolder;

   // public static variables
   public static readonly Dictionary<int, GameObject> _currentCrystals = new Dictionary<int, GameObject>(); // crystal, index

   // static private variables
   private static readonly List<int> _possibleCrystalSpawnPoints = new List<int>(); // it's list of indexes

   // invokes after generate grid
   private void Awake()
   {
      crystalHolder.transform.position = Vector3.zero;

      for (int i = 0; i < GridHandler.isNodeWalkable.Length; i++)
         if (GridHandler.isNodeWalkable[i])
            _possibleCrystalSpawnPoints.Add(i);

      for (int i = 0; i < startCrystalAmount; i++)
      {
         SpawnCrystal();
      }

      StartCoroutine(SpawnCrystalOverTime());
   }

   private void SpawnCrystal()
   {
      int index = _possibleCrystalSpawnPoints[Random.Range(0, _possibleCrystalSpawnPoints.Count)];
      Vector3 position = GridHandler.CalculatePosition(index) + Constants.OFFSET + new Vector3(0, Constants.PLAYER_Y, 0);

      GameObject crystal = Instantiate(crystalPrefab, position, Quaternion.identity, crystalHolder.transform);

      _currentCrystals.Add(index, crystal);

      _possibleCrystalSpawnPoints.Remove(index);
      
      PlayerController.UpdateCrystalInfoStatic();
   }


   // bool returns true if crystal was found
   public static bool Try2CollectCrystal(Vector3 position)
   {
      int index = GridHandler.CalculateIndex(position);

      if (_currentCrystals.ContainsKey(index))
      {
         Try2DestroyCrystal(index);
         return true;
      }

      return false;
   }
   
   public static void Try2DestroyCrystal(int index)
   {
      if (_currentCrystals.ContainsKey(index))
      {
         Destroy(_currentCrystals[index]);
         _currentCrystals.Remove(index);
         _possibleCrystalSpawnPoints.Add(index);
         PlayerController.UpdateCrystalInfoStatic();
      }
   }
   
   private IEnumerator SpawnCrystalOverTime()
   {
      while (true)
      {
         if (_currentCrystals.Count < maxCrystalAmount)
            SpawnCrystal();
         
         yield return new WaitForSeconds(crystalGenerationSpeed);
      }
   }
}
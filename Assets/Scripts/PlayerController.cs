using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// controls everything except movement
// attached to character
public class PlayerController : MonoBehaviour
{
   [SerializeField] private GameObject[] heartsIcons;
   [SerializeField] private float invincibleTime;
   [SerializeField] private float enemiesInfoUpdateRate; // in seconds
   [Space]
   [SerializeField] private TextMeshProUGUI pointsAmountText;
   [SerializeField] private TextMeshProUGUI enemyInfoText;
   [SerializeField] private TextMeshProUGUI crystalInfoText;
   [SerializeField] private GameObject deathScreen;
   [SerializeField] private TextMeshProUGUI highestScoreText;

   // private variables
   private static GameObject _player;
   private static float _pointsAmount = 0;
   // I didn't make health scalable so maxHealth = 3 either
   private static int _currentHealth = 3;
   private static float _tookDmgTime = int.MinValue;
   private static PlayerController _playerController;

   private void Awake()
   {
      _player = GameObject.FindWithTag($"Player");
      _playerController = this;

      StartCoroutine(UpdateEnemyInfo());
   }

   // invokes on stop
   // we separate this method cause it's logically incorrect to get player position in crystal generator
   public void Try2CollectCrystal()
   {
      if (CrystalGenerator.Try2CollectCrystal(_player.transform.position))
      {
         _pointsAmount += Constants.POINTS_PER_CRYSTAL;
         pointsAmountText.text = _pointsAmount.ToString();
         UpdateHealthInfo(_currentHealth, _currentHealth + 1);
      }
   }

   private void UpdateHealthInfo(int from, int to)
   {
      // death
      if (to == 0)
      {
         deathScreen.SetActive(true);
         PlayerRecord.highestScore = _pointsAmount;
         highestScoreText.text = $"HighestScore: {PlayerRecord.highestScore}";
      }
      // decrease (take dmg)
      else if (from > to)
      {
         // take dmg
         if (Time.time > _tookDmgTime + invincibleTime)
         {
            _tookDmgTime = Time.time;
            _currentHealth--;
            heartsIcons[from - 1].SetActive(false);
         }
      }
      // heal
      else
      {
         // TODO: be careful health isn't scalable
         if (to <= 3)
         {
            _currentHealth++;
            heartsIcons[to - 1].SetActive(true);
         }
      }
   }

   // we can't call it as crystals update info
   // cause there are too many enemies
   // so will update info every {enemiesInfoUpdateRate} seconds
   private IEnumerator UpdateEnemyInfo()
   {
      while (true)
      {
         float minDistance = float.MaxValue;

         // find min Distance
         for (int i = 0; i < EnemySpawner.enemiesList.Count; i++)
         {
            int distance = GridHandler.CalculateDistanceCost(transform.position, EnemySpawner.enemiesList[i].transform.position);
            
            if (distance < minDistance)
               minDistance = distance;
         }

         enemyInfoText.text = $"{minDistance/10} / {EnemySpawner.enemiesList.Count}";
         
         yield return new WaitForSeconds(enemiesInfoUpdateRate);
      }
   }
   
   // we can't make this method static because of TextMeshProUGUI
   // we can't use text property
   // and I think it's better than finding this class in every class where we want to use this method
   public static void UpdateCrystalInfoStatic()
   {
      _playerController.UpdateCrystalInfo();
   }
   // invokes every time when player reaches another tile
   // invokes every time when new crystal is spawned and destroyed
   private void UpdateCrystalInfo()
   {
      float minDistance = float.MaxValue;
      
      foreach (var crystal in CrystalGenerator._currentCrystals.Values)
      {
         int distance = GridHandler.CalculateDistanceCost(transform.position, crystal.transform.position);

         if (distance < minDistance)
            minDistance = distance;
      }

      crystalInfoText.text = $"{minDistance / 10} / {CrystalGenerator._currentCrystals.Count}";
   }

   private void OnTriggerEnter(Collider collider)
   {
      if (!collider.transform.CompareTag($"Enemy"))
         return;

      collider.GetComponent<EnemyController>().Destroy();
      UpdateHealthInfo(_currentHealth, _currentHealth - 1);
   }
}
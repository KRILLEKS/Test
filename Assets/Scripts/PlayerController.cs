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

   // private variables
   private static GameObject _player;
   private static float _pointsAmount = 0;
   // I didn't make health scalable so maxHealth = 3 either
   private static int _currentHealth = 3;
   private static float _tookDmgTime = int.MinValue;

   private void Awake()
   {
      _player = GameObject.FindWithTag($"Player");
   }

   // invokes on stop
   // we separate this method cause it's logically incorrect to get player position in crystal generator
   public void Try2CollectCrystal()
   {
      if (CrystalGenerator.Try2CollectCrystal(_player.transform.position))
      {
         _pointsAmount += Constants.POINTS_PER_CRYSTAL;
         pointsAmountText.text = _pointsAmount.ToString();
         UpdateHealthInfo(_currentHealth, _currentHealth+1);
      }
   }

   private void UpdateHealthInfo(int from, int to)
   {
      // death
      if (to == 0)
      {
         // TODO: death
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

   private IEnumerator UpdateEnemyInfo()
   {
      
   }
   private void OnTriggerEnter(Collider collider)
   {
      if (!collider.transform.CompareTag($"Enemy"))
         return;

      collider.GetComponent<EnemyController>().Destroy();
      UpdateHealthInfo(_currentHealth, _currentHealth-1);
   }
   
}

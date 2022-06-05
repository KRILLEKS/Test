using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// controls everything except movement
// attached to character
public class PlayerController : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI pointsAmountText;
   
   // private variables
   private static GameObject _player;
   private static float _pointsAmount = 0;

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
      }
   }
}

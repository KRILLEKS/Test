using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecord : MonoBehaviour
{
   // public static variables
   public static float highestScore
   {
      get
      {
         return _highestScore;
      }
      set
      {
         if (value > _highestScore)
            _highestScore = value;
      }
   }
   private static float _highestScore = 0;

   // private static variables
   private static PlayerRecord _playerRecord;

   private void Awake()
   {
      if (_playerRecord != null)
         Destroy(_playerRecord);

      _playerRecord = this;
      
      // DontDestroyOnLoad(_playerRecord);
   }
}

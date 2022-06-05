using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalGenerator : MonoBehaviour
{
   [SerializeField] private int startCrystalAmount;
   [SerializeField] private int maxCrystalAmount;
   [SerializeField] private float crystalGenerationSpeed; // in seconds
   
   // invokes after generate grid
   private void Awake()
   {
      
   }
}

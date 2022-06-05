using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
   // private variables
   private Animator _animator;

   // TODO: remove animation and make this script scalable (for any menu)
   // and use time.scale 
   private void Awake()
   {
      _animator = GetComponent<Animator>();
   }

   public void RemoveStartScreen()
   {
      _animator.SetTrigger($"RemoveStartScreen");
   }
}

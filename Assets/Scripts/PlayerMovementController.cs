using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
   [SerializeField] private float speed;
   
   // private variables
   private GameObject _character;

   private void Awake()
   {
      _character = GameObject.FindWithTag($"Player");
   }

   // invokes on left click (in input controller event)
   public void Move()
   {
      var path =
         Pathfinding.FindPath(new int2(Mathf.FloorToInt(_character.transform.position.x),
                                       Mathf.FloorToInt(_character.transform.position.z)),
                              new int2(Mathf.FloorToInt(InputController.mousePosition.x),
                                       Mathf.FloorToInt(InputController.mousePosition.z)));

      foreach (var p in path)
      {
         Debug.Log(p);
      }
   }
}
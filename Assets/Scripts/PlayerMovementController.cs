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
   private Coroutine _moveCoroutine;
   private List<Vector3> _path;

   private void Awake()
   {
      _character = GameObject.FindWithTag($"Player");
   }

   // invokes on left click (in input controller event)
   public void Move()
   {
      _path = Pathfinding.FindPath(new int2(Mathf.FloorToInt(_character.transform.position.x),
                                            Mathf.FloorToInt(_character.transform.position.z)),
                                   new int2(Mathf.FloorToInt(InputController.mousePosition.x),
                                            Mathf.FloorToInt(InputController.mousePosition.z)));

      Debug.Log(_path.Count);
      _moveCoroutine = StartCoroutine(moveCoroutine());
   }

   private IEnumerator moveCoroutine()
   {
      foreach (var point in _path)
         while (_character.transform.position != point + Constants.OFFSET)
         {
            _character.transform.position = Vector3.MoveTowards(_character.transform.position,
                                                                point + Constants.OFFSET,
                                                                speed * Time.deltaTime);
            yield return null;
         }
   }
}
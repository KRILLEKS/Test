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
   private bool _isWalking;
   private int _currentIndex2Walk;

   private void Awake()
   {
      _character = GameObject.FindWithTag($"Player");
   }

   // invokes on left click (in input controller event)
   public void Move()
   {
      // if we're walking we want to reach our current point and then walk towards another endNode to prevent issues
      if (_isWalking)
      {
         // TODO: it isn't the best way to handle this. I guess it can be made better. Look for another solution (or think) if time left
         _path = new List<Vector3> {_path[_currentIndex2Walk]};
         _currentIndex2Walk = 0;
         _path.AddRange(Pathfinding.FindPath(new int2(Mathf.FloorToInt(_character.transform.position.x),
                                                      Mathf.FloorToInt(_character.transform.position.z)),
                                             new int2(Mathf.FloorToInt(InputController.mousePosition.x),
                                                      Mathf.FloorToInt(InputController.mousePosition.z))));
      }
      else
      {
         _path = Pathfinding.FindPath(new int2(Mathf.FloorToInt(_character.transform.position.x),
                                               Mathf.FloorToInt(_character.transform.position.z)),
                                      new int2(Mathf.FloorToInt(InputController.mousePosition.x),
                                               Mathf.FloorToInt(InputController.mousePosition.z)));
         _moveCoroutine = StartCoroutine(moveCoroutine());
      }
   }

   private IEnumerator moveCoroutine()
   {
      _isWalking = true;
      
      for (_currentIndex2Walk = 0; _currentIndex2Walk < _path.Count; _currentIndex2Walk++)
      {
         while (_character.transform.position != _path[_currentIndex2Walk] + Constants.OFFSET)
         {
            _character.transform.position = Vector3.MoveTowards(_character.transform.position,
                                                                _path[_currentIndex2Walk] + Constants.OFFSET,
                                                                speed * Time.deltaTime);
            yield return null;
         }
      }

      _isWalking = false;
   }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

// attached to player
public class PlayerMovementController : MonoBehaviour
{
   [SerializeField] private float movementSpeed;
   [SerializeField] private float rotationSpeed;
   [SerializeField] private UnityEvent onStopEvent;

   // private variables
   private Animator _animator;
   private GameObject _character;
   private Coroutine _moveCoroutine;
   private List<Vector3> _path;
   private bool _isWalking;
   private int _currentIndex2Walk;
   private Quaternion _characterRotation;

   private void Awake()
   {
      _animator = GetComponent<Animator>();
      _character = GameObject.FindWithTag($"Player");
   }

   // invokes on left click (in input controller event)
   public void Move()
   {
      // if we're walking we want to reach our current point and then walk towards another endNode to prevent issues
      if (_isWalking)
      {
         // we'll use path to find new one (one line below) so we can't override it yet
         List<Vector3> _newPath = new List<Vector3> {_path[_currentIndex2Walk]};
         // we'll finish movement to the current point only and afterwards we'll start movement to another endpoint
         // because of that we start calculation from the current point which we move towards
         _newPath.AddRange(Pathfinding.FindPath(new int2(Mathf.FloorToInt(_path[_currentIndex2Walk].x),
                                                         Mathf.FloorToInt(_path[_currentIndex2Walk].z)),
                                                new int2(Mathf.FloorToInt(InputController.mousePosition.x),
                                                         Mathf.FloorToInt(InputController.mousePosition.z))));

         _path = _newPath;

         _currentIndex2Walk = 0;
      }
      else
      {
         _path = Pathfinding.FindPath(new int2(Mathf.FloorToInt(_character.transform.position.x),
                                               Mathf.FloorToInt(_character.transform.position.z)),
                                      new int2(Mathf.FloorToInt(InputController.mousePosition.x),
                                               Mathf.FloorToInt(InputController.mousePosition.z)));
         if (_path.Count > 0)
            _moveCoroutine = StartCoroutine(moveCoroutine());
      }
   }

   private IEnumerator moveCoroutine()
   {
      _isWalking = true;
      _animator.SetBool($"IsRun", true);

      for (_currentIndex2Walk = 0; _currentIndex2Walk < _path.Count; _currentIndex2Walk++)
      {
         _characterRotation =
            Quaternion.LookRotation(_path[_currentIndex2Walk] - _character.transform.position + Constants.OFFSET, Vector3.up);

         while (_character.transform.position != _path[_currentIndex2Walk] + Constants.OFFSET)
         {
            _character.transform.position = Vector3.MoveTowards(_character.transform.position,
                                                                _path[_currentIndex2Walk] + Constants.OFFSET,
                                                                movementSpeed * Time.deltaTime);

            _character.transform.rotation =
               Quaternion.Slerp(_character.transform.rotation, _characterRotation, rotationSpeed * Time.deltaTime);

            yield return null;
         }
         
         PlayerController.UpdateCrystalInfoStatic();
      }

      onStopEvent.Invoke();
      _isWalking = false;
      _animator.SetBool($"IsRun", false);
   }
}
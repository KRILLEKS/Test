using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// attach to camera
public class CameraController : MonoBehaviour
{
   [SerializeField] private Vector3 offset;

   // private variables
   private Transform _characterTransform;

   private void Awake()
   {
      _characterTransform = GameObject.FindWithTag("Player").transform;
   }

   private void Update()
   {
      transform.position = _characterTransform.position + offset;
   }
}

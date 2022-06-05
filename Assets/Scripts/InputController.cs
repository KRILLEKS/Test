using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputController : MonoBehaviour
{
   [Header("Events")]
   [SerializeField] private UnityEvent onLeftClick;
   
   [Space,Header("Values")]
   [SerializeField] private Camera mainCamera;
   // which layer will be our ground
   [SerializeField] private LayerMask layer;

   // public static variables
   public static Vector3 mousePosition;

   private void Update()
   {
      // TODO: prevent clicking through UI
      // handle keys press
      if (Input.GetKeyDown(KeyCode.Mouse0))
         onLeftClick.Invoke();
      
      // get mouse position
      Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

      if (Physics.Raycast(ray, out RaycastHit raycastHit, layer))
         mousePosition = raycastHit.point;
   }
}

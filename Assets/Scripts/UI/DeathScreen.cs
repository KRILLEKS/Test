using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
   // invokes on button press
   public void Restart()
   {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
   }
}

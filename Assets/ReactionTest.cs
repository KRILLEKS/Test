using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ReactionTest : MonoBehaviour
{
   [SerializeField] private GameObject _canvas;

   // arc
   [Space, SerializeField] private GameObject _ArcGO;
   [SerializeField, Range(0, 1)] private float _maxEmptyFill;
   [SerializeField, Range(0, 1)] private float _minEmptyFill;
   // if this = 0.5 and empty fill = 0.4 then filled arc fill = 0.2 (max)
   [SerializeField, Range(0, 1)] private float _maxSkillCheckDifference;
   // if this = 0.1 and empty fill = 0.4 then filled arc fill = 0.04 (min)
   [SerializeField, Range(0, 1)] private float _minSkillCheckDifference;

   // private variables
   private GameObject _currentGO;
   private GameObject _cursorGO;

   private void Update()
   {
      if (Input.GetKeyDown("d"))
         StartCheck();

      // if (_currentGO != null)
      // {
      //    _cursorGO.transform.Rotate(Vector3.forward,10 * Time.deltaTime);
      // }
   }

   private void StartCheck()
   {
      // TODO: remove this
      if (_currentGO != null)
         Destroy(_currentGO);

      _currentGO = Instantiate(_ArcGO, _canvas.transform);

      var emptyArcGO = _currentGO.transform.GetChild(0).gameObject;
      var skillCheckArcGO = _currentGO.transform.GetChild(1).gameObject;

      var randomEmptyArcRotation = Random.Range(0, 361);
      emptyArcGO.transform.rotation = Quaternion.Euler(0, 0, randomEmptyArcRotation);

      var randomEmptyArcFillAmount = Random.Range(_minEmptyFill, _maxEmptyFill);
      emptyArcGO.GetComponent<Image>().fillAmount = randomEmptyArcFillAmount;

      var randomSkillCheckFill =
         Random.Range(_minSkillCheckDifference, _maxSkillCheckDifference) * randomEmptyArcFillAmount;
      skillCheckArcGO.GetComponent<Image>().fillAmount = randomSkillCheckFill;

      skillCheckArcGO.transform.rotation =
         Quaternion.Euler(0,
                          0,
                          randomEmptyArcRotation
                          - Random.Range(0, (randomEmptyArcFillAmount - randomSkillCheckFill) * 360));

      _cursorGO = _currentGO.transform.GetChild(2).gameObject;
      _cursorGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
      Debug.Log(emptyArcGO.GetComponent<RectTransform>().anchoredPosition);
   }
}
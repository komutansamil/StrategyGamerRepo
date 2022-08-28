using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pathfinding 
{
    public class Soldier : MonoBehaviour
    {
        #region Values
        [SerializeField] private GameObject _colorArea; // Color will appear when select a soldier on gameboard.

        // This is public accesing area between two scripts, I revert the private object to public access!
        [HideInInspector] public GameObject ColorArea { get { return _colorArea; } set { _colorArea = value; } }
        [SerializeField] private GameObject target;

        private void Start()
        {
            target.transform.SetParent(null); // I took out the target transform for target mustn't follow the parent position.
        }
        #endregion
    }
}

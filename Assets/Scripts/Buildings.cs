using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Pathfinding
{
    public class Buildings : MonoBehaviour
    {
        #region Values
        [SerializeField] private Manager manager;
        [SerializeField] private GameObject _colorArea;
        [SerializeField] private Transform _spawnPoint;
        [HideInInspector] public Transform SpawnPoint { get { return _spawnPoint; } set { _spawnPoint = value; } }
        [HideInInspector] public GameObject ColorArea { get { return _colorArea; } set { _colorArea = value; } }
        #endregion

        #region Calling
        private void Start()
        {
            manager = GameObject.Find("Manager").GetComponent<Manager>();
        }
        #endregion

        #region Collision
        private void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log("Warning");
            if (col.gameObject.tag == "Barracks")
            {
                manager.Warning_Message = "The red area of the new building";
                manager.Message();
                manager.IsLocating = false;
            }
            if (col.gameObject.tag == "PowerPlant")
            {
                manager.Warning_Message = "The red area of the new building";
                manager.Message();
                manager.IsLocating = false;
            }
        }
        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.tag == "Barracks")
            {
                manager.Warning_Message = "";
                manager.Message();
                manager.IsLocating = true;
            }
            if (col.gameObject.tag == "PowerPlant")
            {
                manager.Warning_Message = "";
                manager.Message();
                manager.IsLocating = true;
            }
        }
        #endregion
    }
}

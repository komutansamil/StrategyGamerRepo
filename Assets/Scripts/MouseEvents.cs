using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pathfinding
{
    public class MouseEvents : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        #region Values
        [SerializeField] private AstarPath _astarPath;
        [SerializeField] private Manager _manager;
        [SerializeField] private GameObject _powerPlant_Obj;
        [SerializeField] private GameObject _barracks_Obj;
        [SerializeField] private Camera _camera_Obj;
        bool isBarracksLocating = false;
        bool isPowerPlantLocating = false;
        Vector3 targetPos;
        #endregion

        #region Calling
        void Start()
        {
            _manager = GameObject.Find("Manager").GetComponent<Manager>();
            _camera_Obj = GameObject.Find("MainCamera").GetComponent<Camera>();
            _astarPath = GameObject.Find("PathFinding").GetComponent<AstarPath>();
        }
        #endregion


        #region Events
        public void OnPointerDown(PointerEventData eventData)
        {
            if (this.gameObject.name == "Barracks_Button")
            {
                var barrack = Instantiate(_barracks_Obj, Input.mousePosition, Quaternion.identity);
                _manager.BarrackList.Add(barrack);
                isBarracksLocating = true;
            }
            if (this.gameObject.name == "PowerPlant_Button")
            {
                var powerplant = Instantiate(_powerPlant_Obj, Input.mousePosition, Quaternion.identity);
                _manager.PowerPlantList.Add(powerplant);
                isPowerPlantLocating = true;
            }
            if (this.gameObject.tag == "Barracks")
            {
                _manager.IsBarracks = true;
                _manager.IsPowerPlant = false;
                _manager.IsSoldier = false;
                Debug.Log("Barracks");
                for (int s = 0; s < _manager.SoldierList.Count; s++)
                {
                    _manager.SoldierList[s].GetComponent<Soldier>().ColorArea.SetActive((false));
                }
                _manager.SoldierList.Clear();
                _manager.SelectedBarracks.Clear();
                _manager.SelectedBarracks.Add(this.gameObject);
            }
            if (this.gameObject.tag == "PowerPlant")
            {
                _manager.IsPowerPlant = true;
                _manager.IsBarracks = false;
                _manager.IsSoldier = false;
                Debug.Log("PowerPlant");
                
                for (int s = 0; s < _manager.SoldierList.Count; s++)
                {
                    _manager.SoldierList[s].GetComponent<Soldier>().ColorArea.SetActive((false));
                }
                _manager.SoldierList.Clear();
            }
            if (this.gameObject.tag == "Soldier")
            {
                _manager.TargetPosition = _camera_Obj.ScreenToWorldPoint(Input.mousePosition);
                _manager.TargetPosition = this.gameObject.transform.position;

                _manager.IsSoldier = true;
                _manager.IsPowerPlant = false;
                _manager.IsBarracks = false;

                if (!Input.GetKey(KeyCode.LeftShift)) // Ýf we want to select all soldiers, we must press left shift with mouse
                    // left click.
                {
                    for (int s = 0; s < _manager.SoldierList.Count; s++)
                    {
                        _manager.SoldierList[s].GetComponent<Soldier>().ColorArea.SetActive((false));
                    }
                    if (!_manager.SoldierList.Contains(this.gameObject))
                    {
                        _manager.SoldierList.Clear();
                        _manager.SoldierList.Add(this.gameObject);
                    }

                    this.gameObject.GetComponent<Soldier>().ColorArea.SetActive((true));
                }
                else
                {
                    if (!_manager.SoldierList.Contains(this.gameObject))
                    {
                        _manager.SoldierList.Add(this.gameObject);
                    }
                    this.gameObject.GetComponent<Soldier>().ColorArea.SetActive((true));
                }
                Debug.Log("Soldier");
            }
            if (this.gameObject.tag == "Tile")
            {
                _manager.IsSoldier = false;
                if (_manager.SoldierList.Count != 0)
                {
                    _manager.TargetPosition = this.gameObject.transform.position;
                }
                Debug.Log("Tile");
                if (!Input.GetMouseButtonDown(1))
                {
                    for (int s = 0; s < _manager.SoldierList.Count; s++)
                    {
                        _manager.SoldierList[s].GetComponent<Soldier>().ColorArea.SetActive((false));
                    }
                }
            }
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_manager.IsLocating && isPowerPlantLocating)
            {
                Destroy(_manager.PowerPlantList[_manager.PowerPlantList.Count - 1]);
                _manager.PowerPlantList.RemoveAt(_manager.PowerPlantList.Count - 1);
            }
            if (!_manager.IsLocating && isBarracksLocating)
            {
                Destroy(_manager.BarrackList[_manager.BarrackList.Count - 1]);
                _manager.BarrackList.RemoveAt(_manager.BarrackList.Count - 1);
            }
            if (_manager.IsLocating && isBarracksLocating)
            {
                _manager.BarrackList[_manager.BarrackList.Count - 1].GetComponent<BoxCollider2D>().isTrigger = false;
            }
            if (_manager.IsLocating && isPowerPlantLocating)
            {
                _manager.PowerPlantList[_manager.PowerPlantList.Count - 1].GetComponent<BoxCollider2D>().isTrigger = false;
            }
            isPowerPlantLocating = false;
            isBarracksLocating = false;
            _astarPath.Scan();
        }
        public void OnDrag(PointerEventData eventData)
        {
            CreatedObjectsProcces();
        }
        #endregion

        #region Special Methods
        void CreatedObjectsProcces()
        {
            if (isBarracksLocating)
            {
                if (_manager.BarrackList.Count != 0)
                {
                    Vector3 pos = _camera_Obj.ScreenToWorldPoint(Input.mousePosition);
                    pos.z = 0;
                    _manager.BarrackList[_manager.BarrackList.Count - 1].transform.position = pos;
                }
            }
            if (isPowerPlantLocating)
            {
                if (_manager.PowerPlantList.Count != 0)
                {
                    Vector3 pos = _camera_Obj.ScreenToWorldPoint(Input.mousePosition);
                    pos.z = 0;
                    _manager.PowerPlantList[_manager.PowerPlantList.Count - 1].transform.position = pos;
                }
            }
        }
        #endregion
    }
}
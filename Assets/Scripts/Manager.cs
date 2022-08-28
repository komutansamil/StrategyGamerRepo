using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Pathfinding // I added this script to pathfinding namespace for accesing AStar script for scanning procces.
{
    public class Manager : MonoBehaviour
    {
        #region Values
        [Header("Warning Message")]
        [SerializeField] private Text _Warning_Txt;
        string warning_Message;
        [HideInInspector] public string Warning_Message { get { return warning_Message; } set { warning_Message = value; } }

        [Header("Building Image")]
        [SerializeField] private Text _buildingName_Txt;
        [SerializeField] private Image _Barracks_Img;
        [SerializeField] private Image _PowerPlant_Img;

        [Header("Sprite/Button/Panels")]
        [SerializeField] private Sprite _Barracks_Sprite;
        [SerializeField] private Sprite _PowerPlant_Sprite;
        [SerializeField] private Button _soldierProduction_Button;
        [SerializeField] private GameObject _powerPlant_Panel;
        [SerializeField] private GameObject _barracks_Panel;

        [Header("Lists Of Game Objects")]
        [SerializeField] private List<GameObject> _soldierList = new List<GameObject>();
        [SerializeField] private List<GameObject> _barrackList = new List<GameObject>();
        [SerializeField] private List<GameObject> _powerPlantList = new List<GameObject>();
        [SerializeField] private List<GameObject> _tileList = new List<GameObject>();
        [SerializeField] private List<GameObject> _selectedBarracks = new List<GameObject>();
        [SerializeField] private List<GameObject> _soldierPool = new List<GameObject>();

        [Header("Game Objects")]
        [SerializeField] private GameObject _soldier_Obj;
        [SerializeField] private GameObject _tile_Obj;

        [HideInInspector] public List<GameObject> BarrackList { get { return _barrackList; } set { _barrackList = value; } }
        [HideInInspector] public List<GameObject> SoldierList { get { return _soldierList; } set { _soldierList = value; } }
        [HideInInspector] public List<GameObject> PowerPlantList { get { return _powerPlantList; } set { _powerPlantList = value; } }
        [HideInInspector] public bool IsPowerPlant { get { return _isPowerPlant; } set { _isPowerPlant = value; } }
        [HideInInspector] public bool IsBarracks { get { return _isBarracks; } set { _isBarracks = value; } }
        [HideInInspector] public Vector3 TargetPosition { get { return targetPos; } set { targetPos = value; } }
        [HideInInspector] public List<GameObject> SelectedBarracks { get { return _selectedBarracks; } set { _selectedBarracks = value; } }
        [HideInInspector] public bool IsSelectingSoldier { get { return _isSelectingSoldier; } set { _isSelectingSoldier = value; } }
        [HideInInspector] public bool IsBarracksLocating { get { return _isBarracksLocating; } set { _isBarracksLocating = value; } }
        [HideInInspector] public bool IsPowerPlantLocating { get { return _isPowerPlantLocating; } set { _isPowerPlantLocating = value; } }
        [HideInInspector] public bool IsSoldier { get { return _isPowerPlantLocating; } set { _isPowerPlantLocating = value; } }
        [HideInInspector] public bool IsLocating { get { return _isLocating; } set { _isLocating = value; } }

        [Header("Setting Tiles")]
        [SerializeField] private int _width, _height;
        [SerializeField] private Camera _camera_Obj;

        bool _isPowerPlant = false;
        bool _isBarracks = false;
        bool _isSelectingSoldier = false;
        bool _isBarracksLocating = false;
        bool _isPowerPlantLocating = false;
        bool _isLocating = true;
        Vector3 targetPos;
        #endregion

        #region Calling
        void Start()
        {
            _camera_Obj = Camera.main; // I found the main camera in first time for optimization.
            CreateTiles(); // I created tiles for gameboard.
            _soldierProduction_Button.onClick.AddListener(SoldierProduction); //This is button listener of soldier production button.
        }

        // Update is called once per frame
        void Update()
        {
            SelectingSoldiersAndMovement();
            CheckSelectedObjects();
        }
        #endregion

        #region SpecialMethods

        public void Message()
        {
            Debug.Log(_isLocating);
            _Warning_Txt.text = warning_Message;
        }

        void CheckSelectedObjects() // I check if player click the barracks or powerplant, if is, I change the panels and texts.
        {
            _barracks_Panel.SetActive(_isBarracks);
            _powerPlant_Panel.SetActive(_isPowerPlant);

            if (_isBarracks)
            {
                _Barracks_Img.sprite = _Barracks_Sprite;
                _Barracks_Img.transform.GetChild(0).GetComponent<Text>().text = "Barracks";
            }
            if (_isPowerPlant)
            {
                _PowerPlant_Img.sprite = _PowerPlant_Sprite;
                _PowerPlant_Img.transform.GetChild(0).GetComponent<Text>().text = "Power Plant";
            }
        }

        void CreateTiles()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Debug.Log(x / 4f);
                    var tile = Instantiate(_tile_Obj, new Vector3(x / 4f, y / 4f), Quaternion.identity);
                    tile.transform.SetParent(this.gameObject.transform);
                    //tile.tag = "";
                    _tileList.Add(tile);
                }
            }
        }

        void SelectingSoldiersAndMovement()
        {
            if (Input.GetMouseButtonDown(1))
            {
                targetPos = _camera_Obj.ScreenToWorldPoint(Input.mousePosition);
                targetPos.z = 0;
                if (_soldierList.Count != 0)
                {
                    for (int s = 0; s < _soldierList.Count; s++)
                    {
                        // I set pathfinding target transform to here.
                        _soldierList[s].GetComponent<AIDestinationSetter>().target.position = targetPos;
                    }
                }
            }           
        }

        void SoldierProduction()
        {
            // I instantiated soldiers at the individual spawn points for only barracks.
            targetPos = SelectedBarracks[0].GetComponent<Buildings>().SpawnPoint.transform.position;
            var soldier = Instantiate(_soldier_Obj, targetPos, Quaternion.identity);
        }
        #endregion
    }
}

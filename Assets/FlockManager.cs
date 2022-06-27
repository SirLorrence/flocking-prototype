using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class FlockManager : MonoBehaviour {
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private int _amount = 1;
    private int _maxAmount = 50;
    private int _lastValue = default;
    [SerializeField] private Vector3 _flockingArea = Vector3.one; //the area if which the objects will be limited to

    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _maxFlockSpeed;

    [SerializeField] private float _neighborhoodFlockDist; // the area of which is consider its neighbor. a street or town

    [SerializeField] private float _neighbotDist; // how close an game-object can come close each other
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private bool _moveGoalManual = default;
    [SerializeField] private Vector3 _goal = Vector3.zero;


    private GameObject[] _allGameObjects;

    private Bounds _regionBounds;

    public float MinSpeed => _minSpeed;
    public float MaxSpeed => _maxSpeed;
    public float MaxFlockSpeed => _maxFlockSpeed;
    
    public float NeighborDist => _neighbotDist;
    public float NeighborhoodFlockDist => _neighborhoodFlockDist;
    
    public float RotationSpeed => _rotationSpeed;

    public GameObject[] AllGameObjects => _allGameObjects;

    public Vector3 Goal => _goal;

    public Bounds RegionBounds => _regionBounds;

    private void Start() {
        //create bounds to contain the flock
        _regionBounds = new Bounds(transform.position, _flockingArea * 2);

        
        if (Camera.main != null)
            Camera.main.transform.position = new Vector3(_flockingArea.x * -2, _flockingArea.y * 2);
        
        // flock initialization
        
        _allGameObjects = new GameObject[_maxAmount];
        
        for (int i = 0; i < _maxAmount; i++) {
            //creates a spawn point for each game object in random area within the flocking area 
            var pos = this.transform.position + new Vector3(Random.Range(-_flockingArea.x, _flockingArea.x),
                Random.Range(-_flockingArea.y, _flockingArea.y), Random.Range(-_flockingArea.z, _flockingArea.z));
            _allGameObjects[i] = Instantiate(_gameObject, pos, Quaternion.identity);
            _allGameObjects[i].transform.parent = transform;

            //if i forget to add the script to the prefab ... or just lazy
            try {
                _allGameObjects[i].GetComponent<FlockEntity>().Manager = this;
            }
            catch (Exception e) {
                var temp = _allGameObjects[i].AddComponent<FlockEntity>();
                temp.Manager = this;
            }
            _allGameObjects[i].SetActive(false);
        }
        StartCoroutine(MoveGoalPosition());
    }

    private void LateUpdate() {
        ChangeFlockSize();
    }

    private void ChangeFlockSize() {
        if (_amount != _lastValue) {
            int count = 0;
            foreach (var entity in _allGameObjects) {
                entity.SetActive(count <= _amount);
                count++;
            }
            _lastValue = _amount;
        }
    }

    private IEnumerator MoveGoalPosition() {
        while (true) {
            if (!_moveGoalManual) {
                _goal = new Vector3(Random.Range(-_flockingArea.x, _flockingArea.x),
                    Random.Range(-_flockingArea.y, _flockingArea.y), Random.Range(-_flockingArea.z, _flockingArea.z));
            }
            yield return new WaitForSeconds(2.5f);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        Handles.Label(transform.position, "Flocking Area", EditorStyles.textArea);
        Gizmos.DrawWireCube(this.transform.position, _flockingArea * 2);

        GUI.color = Color.yellow;
        Handles.Label(_goal + Vector3.down, "Goal", EditorStyles.textArea);
        Gizmos.DrawSphere(_goal, .5f);
    }
#endif
}
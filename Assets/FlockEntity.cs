using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlockEntity : MonoBehaviour {
    private FlockManager _manager;
    private float _speed;
    private bool _inGroup;
    private bool _enableRules;


    private RaycastHit _hit;

    public FlockManager Manager {
        set => _manager = value;
    }

    public float Speed => _speed;

    private void SetRandomSpeed() => _speed = Random.Range(_manager.MinSpeed, _manager.MaxSpeed);
    private bool ObstacleCheck() => Physics.Raycast(transform.position, transform.forward * 5, out _hit); 
    private void Start() {
         SetRandomSpeed();//starting speed
    }

    private void Update() {
        var dir = Vector3.zero;
        if (!_manager.RegionBounds.Contains(transform.position)) {
            _inGroup = false;
            dir = _manager.transform.position - transform.position; // get back into the zone 
        }
        else if (ObstacleCheck()) {
            _inGroup = false;
            dir = Vector3.Reflect(transform.forward, _hit.normal);
        }
        else _inGroup = true;

        if (!_inGroup) {
           
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir),
                _manager.RotationSpeed * Time.deltaTime);
        }
        else {
            AppleRules();
        }

        _speed = Mathf.Clamp(_speed, 0, _manager.MaxFlockSpeed); 
        transform.Translate(Vector3.forward * (_speed * Time.deltaTime));
    }

    /* ===========================
     *  Flocking Rules
     * ==========================
     * 1. Move towards the average position of the flock - Cohesion
     * 2. Align with forward heading of the flock - Alignment
     * 3. Avoid crowding together - Separation
     */

    void AppleRules() {
        var neighbors = _manager.AllGameObjects;

        Vector3 centerVector = Vector3.zero; // avg center of the flock
        Vector3 avoidanceVector = Vector3.zero; // avg of all avoidance vectors

        float globalSpeed = 0.01f;
        float closetNeighbor;

        int flockSize = 0;


        foreach (var neighbor in neighbors) {
            if (neighbor != this.gameObject && neighbor.activeSelf) {
                closetNeighbor = Vector3.Distance(neighbor.transform.position, transform.position);
                if (closetNeighbor <= _manager.NeighborhoodFlockDist) {
                    centerVector += neighbor.transform.position;
                    flockSize++;
                    if (closetNeighbor < _manager.NeighborDist) {
                        avoidanceVector += transform.position - neighbor.transform.position;
                    }

                    var anotherEnitiy = neighbor.GetComponent<FlockEntity>();
                    globalSpeed += anotherEnitiy.Speed;
                }
            }
        }

        if (flockSize > 0) {
            centerVector /= flockSize; // cohesion - getting the avg center
            centerVector += _manager.Goal - transform.position; // the direction to head towards the goal
            _speed = globalSpeed / flockSize;
            var desiredFacingDirection = (centerVector + avoidanceVector) - transform.position;
            if (desiredFacingDirection != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredFacingDirection),
                    _manager.RotationSpeed * Time.deltaTime);
        }
        else SetRandomSpeed(); // not in a group anymore, return to 'roaming' speed
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        Gizmos.color = (ObstacleCheck()) ? Color.red : Color.green;
        Gizmos.DrawRay(transform.position,transform.forward * 5);
    }
#endif
}
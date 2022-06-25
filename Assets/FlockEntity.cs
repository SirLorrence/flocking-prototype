using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlockEntity : MonoBehaviour {
    private FlockManager _manager;
    private float _speed;

    public FlockManager Manager {
        set => _manager = value;
    }

    public float Speed => _speed;

    private void Start() {
        _speed = Random.Range(_manager.MinSpeed, _manager.MaxSpeed);
    }

    private void Update() {
        AppleRules();
        transform.Translate(Vector3.forward * (_speed * Time.deltaTime));
    }
    
    /* ===========================
     *  Flocking Rules
     * ==========================
     * 1. Move towards the average position of the flock
     * 2. Align with forward heading of the flock
     * 3. Avoid crowding together
     */

    void AppleRules() {
        var neighbors = _manager.AllGameObjects;

        Vector3 centerVector = Vector3.zero; // avg center of the flock
        Vector3 avoidanceVector = Vector3.zero; // avg of all avoidance vectors

        float globalSpeed = 0.01f;
        float closetNeighbor;

        int flockSize = 0;


        foreach (var neighbor in neighbors) {
            if (neighbor != this.gameObject) {
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
            centerVector /= flockSize;
            _speed = globalSpeed/flockSize;
            var facingDirection = (centerVector + avoidanceVector) - transform.position;
            if (facingDirection != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(facingDirection),
                    _manager.RotationSpeed * Time.deltaTime);
        }
    }
}
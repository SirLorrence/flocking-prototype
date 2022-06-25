using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlockEntity : MonoBehaviour {
    private FlockManager _manager;
    private float _speed;

    public FlockManager Manager {
        set => _manager = value;
    }

    private void Start() {
        _speed = Random.Range(_manager.MinSpeed, _manager.MaxSpeed);
    }

    private void Update() {
        transform.Translate(Vector3.forward * (_speed * Time.deltaTime));
    }
}
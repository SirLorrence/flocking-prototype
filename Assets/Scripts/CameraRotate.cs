using System;
using UnityEngine;

public class CameraRotate : MonoBehaviour { 
    [SerializeField] private GameObject _target;
    [SerializeField] private float _speed = 10;
    private void LateUpdate() {
        transform.RotateAround(_target.transform.position, Vector3.up, _speed * Time.deltaTime);
    }
}

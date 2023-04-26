using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace JengaGame
{
    public class CameraControls : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float distance = 10.0f;
        [SerializeField] private float xSpeed = 120.0f;
        [SerializeField] private float ySpeed = 120.0f;
        [SerializeField] private float scrollStepSize = 5f;
        [SerializeField, Range(0, 1)] private float lerpFactor = 0.1f;

        private float _x, _y;
        private float _xInput, _yInput;
        private float _scrollInput;
        private Transform _cameraT, _cameraFollowT;

        private void Start()
        {
            InitializeVariables();
        }

        private void InitializeVariables()
        {
            _cameraT = transform;
            _cameraFollowT = new GameObject("CameraFollow").transform;
            _cameraFollowT.position = _cameraT.position;
            _cameraFollowT.rotation = _cameraT.rotation;

            var angles = _cameraT.eulerAngles;
            _x = angles.y;
            _y = angles.x;
        }

        private void Update()
        {
            UpdateCamera();
        }

        private void UpdateCamera()
        {
            var cameraT = transform;
            if (Input.GetMouseButton(0))
            {
                _xInput = Input.GetAxis("Mouse X");
                _yInput = Input.GetAxis("Mouse Y");
            }
            else
            {
                _xInput = Mathf.Lerp(_xInput, 0, lerpFactor);
                _yInput = Mathf.Lerp(_yInput, 0, lerpFactor);
            }

            var scrollInput = Input.GetAxis("Mouse ScrollWheel");
            _scrollInput = scrollInput != 0 ? scrollInput : Mathf.Lerp(_scrollInput, 0, lerpFactor);
            distance = Mathf.Clamp(distance - scrollInput * scrollStepSize, 1.0f, 100.0f);

            _x += _xInput * xSpeed * Time.deltaTime;
            _y -= _yInput * ySpeed * Time.deltaTime;

            _y = Mathf.Clamp(_y, 0, 80);
            var rotation = Quaternion.Euler(_y, _x, 0);
            var position = target.position + (rotation * new Vector3(0.0f, 0.0f, -distance));

            _cameraFollowT.rotation = rotation;
            _cameraFollowT.position = position;
            _cameraFollowT.LookAt(target);
            _cameraT.rotation = Quaternion.Lerp(_cameraT.rotation, _cameraFollowT.rotation, lerpFactor);
            _cameraT.position = Vector3.Lerp(_cameraT.position, _cameraFollowT.position, lerpFactor);
        }

        private void ChangeTarget(Transform newTarget)
        {
            target = newTarget;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace JengaGame
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField, BoxGroup("Config")] private Transform target;
        [SerializeField, BoxGroup("Config")] private float distance = 10.0f;
        [SerializeField, BoxGroup("Config")] private float xSpeed = 120.0f;
        [SerializeField, BoxGroup("Config")] private float ySpeed = 120.0f;
        [SerializeField, BoxGroup("Config")] private float scrollStepSize = 5f;
        [SerializeField, BoxGroup("Config"), Range(0, 1)]
        private float lerpFactor = 0.1f;
        private float _x, _y;
        private float _xInput, _yInput;
        private float _scrollInput;
        private Transform _cameraT, _cameraFollowT;

        private void Start()
        {
            InitializeVariables();
            InitializeListeners();
        }

        private void InitializeVariables()
        {
            _cameraT = transform;
            _cameraFollowT = new GameObject("CameraFollow").transform;
            _cameraFollowT.position = _cameraT.position;
            _cameraFollowT.rotation = _cameraT.rotation;

            var angles = _cameraT.eulerAngles;
            _x = 0;
            _y = 20;
        }

        private void InitializeListeners()
        {
            GameManager.Instance.OnStackSelected += OnStackSelected;
        }

        private void OnStackSelected(Stack stack)
        {
            target = stack.transform;
        }

        private void Update()
        {
            UpdateCameraValues();
        }

        private void UpdateCameraValues()
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

        private void OnDestroy()
        {
            if (!GameManager.HasInstance) return;
            GameManager.Instance.OnStackSelected -= OnStackSelected;
        }
    }
}
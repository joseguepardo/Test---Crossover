using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JengaGame
{
    public class CameraInput : MonoBehaviour
    {
        private GameManager _gameManager;
        [SerializeField, BoxGroup("RayCasting")]
        private Camera camera;
        [SerializeField, BoxGroup("RayCasting")]
        private LayerMask stackLayerMask, blockLayerMask;
        [SerializeField, BoxGroup("RayCasting")]
        private float clickThreshold = 0.2f;
        private float _mouseDownTime;
        private Stack _hoveringStack;
        private Block _hoveringBlock;

        private void Start()
        {
            _gameManager = GameManager.Instance;
        }

        private void Update()
        {
            EvaluateRaycasts();
            EvaluateSingleClick();
        }

        private void EvaluateRaycasts()
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            EvaluateStackRaycast(ray);
            EvaluateBlockRaycast(ray);
        }

        private void EvaluateStackRaycast(Ray ray)
        {
            _hoveringStack = Physics.Raycast(ray, out var hit, 1000, stackLayerMask)
                ? hit.collider.GetComponent<Stack>()
                : null;

            _gameManager.HoveringStack(_hoveringStack);
        }

        private void EvaluateBlockRaycast(Ray ray)
        {
            if (Physics.Raycast(ray, out var hit, 1000, blockLayerMask))
            {
                _hoveringBlock = hit.collider.GetComponent<Block>();
                return;
            }

            _hoveringBlock = null;
        }

        private void EvaluateSingleClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mouseDownTime = Time.time;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                var elapsedTime = Time.time - _mouseDownTime;

                if (elapsedTime <= clickThreshold)
                {
                    OnSingleClick();
                }
            }
        }

        private void OnSingleClick()
        {
            if (_hoveringStack != null)
                GameManager.Instance.SelectStack(_hoveringStack);
            // else if(_hoveringBlock != null)
            //     GameManager.Instance.SelectBlock(_hoveringBlock
        }
    }
}
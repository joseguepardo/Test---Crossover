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

        private IClickableStack _lastHoveredStack;
        private IClickableStack _lastSelectedStack;
        private IClickableBlock _lastHoveredBlock;
        private IClickableBlock _lastSelectedBlock;

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
            if (Physics.Raycast(ray, out var hit, 1000, stackLayerMask))
            {
                if (hit.collider.TryGetComponent<IClickableStack>(out var stack))
                {
                    if (stack == _lastHoveredStack) return;

                    _lastHoveredStack?.OnHover(false);
                    _lastHoveredStack = stack;
                    _lastHoveredStack.OnHover(true);
                }
                else Deselect();
            }
            else Deselect();

            void Deselect()
            {
                _lastHoveredStack?.OnHover(false);
                _lastHoveredStack = null;
            }
        }

        private void EvaluateBlockRaycast(Ray ray)
        {
            if (Physics.Raycast(ray, out var hit, 1000, blockLayerMask))
            {
                if (hit.collider.TryGetComponent<IClickableBlock>(out var block))
                {
                    if (block == _lastHoveredBlock) return;

                    _lastHoveredBlock?.OnHover(false);
                    _lastHoveredBlock = block;
                    _lastHoveredBlock.OnHover(true);
                }
                else Deselect();
            }
            else Deselect();

            void Deselect()
            {
                _lastHoveredBlock?.OnHover(false);
                _lastHoveredBlock = null;
            }
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
            void SelectStack()
            {
                _lastSelectedBlock?.OnClick(false);
                _lastSelectedStack?.OnClick(false);
                _lastSelectedStack = _lastHoveredStack;
                _lastSelectedStack?.OnClick(true);
            }

            void SelectBlock()
            {
                _lastSelectedBlock?.OnClick(false);
                _lastSelectedBlock = _lastHoveredBlock;
                _lastSelectedBlock?.OnClick(true);
            }

            if (_lastSelectedStack != null)
            {
                if (_lastHoveredStack != null && _lastHoveredStack != _lastSelectedStack)
                {
                    SelectStack();
                }
                else
                {
                    if (_lastHoveredBlock != null)
                    {
                        SelectBlock();
                    }
                }
            }
            else SelectStack();
        }
    }
}
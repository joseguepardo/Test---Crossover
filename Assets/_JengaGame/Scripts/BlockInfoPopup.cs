using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace JengaGame
{
    public class BlockInfoPopup : MonoBehaviour
    {
        [System.Serializable]
        public class InfoContainer
        {
            public Block.BlockType BlockType;
            [SerializeField] private GameObject containerGo;
            [SerializeField] private TMP_Text gradeText, domainText;

            public void Initialize(string grade, string domain)
            {
                containerGo.SetActive(true);
                gradeText.text = grade;
                domainText.text = domain;
            }

            public void Hide()
            {
                containerGo.SetActive(false);
            }
        }

        [SerializeField] private InfoContainer[] infoContainers;

        private RectTransform _rectTransform, _canvasRectTransform;
        private Canvas _canvas;
        private Vector3 _targetPosition;
        private Vector2 _rectHalfSize;
        [SerializeField, Range(0, 1)] private float lerpAmount;

        public void Initialize(Block.BlockType blockType, string grade, string domain)
        {
            foreach (var infoContainer in infoContainers)
            {
                if (infoContainer.BlockType == blockType)
                    infoContainer.Initialize(grade, domain);
                else
                    infoContainer.Hide();
            }
        }

        private void Awake()
        {
            InitializeVariables();
        }

        private void OnEnable()
        {
            _targetPosition = GetTargetPosition();
            _rectTransform.localPosition = _targetPosition;
        }

        private void InitializeVariables()
        {
            _rectTransform = transform as RectTransform;
            _canvas = GetComponentInParent<Canvas>();
            _canvasRectTransform = _canvas.transform as RectTransform;
            _rectHalfSize = _rectTransform!.sizeDelta / 2;
        }

        private void Update()
        {
            UpdateMousePosition();
        }

        private void UpdateMousePosition()
        {
            _targetPosition = GetTargetPosition();

            _rectTransform.localPosition = Vector3.Lerp(
                _rectTransform.localPosition,
                _targetPosition,
                lerpAmount
            );
        }

        private Vector2 GetTargetPosition()
        {
            var mousePosition = Input.mousePosition;
            var normalizedMousePosition = mousePosition / new Vector2(Screen.width, Screen.height);
            var normalizedOffset = new Vector2(
                normalizedMousePosition.x < 0.8f ? 1 : -1,
                normalizedMousePosition.y < 0.2f ? 1 : -1);
            var offset = normalizedOffset * _rectHalfSize;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvasRectTransform,
                mousePosition,
                _canvas.worldCamera,
                out var localMousePosition
            );
            return localMousePosition + offset;
        }
    }
}
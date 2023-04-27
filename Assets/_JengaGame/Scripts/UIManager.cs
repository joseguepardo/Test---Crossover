using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JengaGame
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private BlockInfoPopup blockInfoPopup;
        [SerializeField] private BlockInfoPanel blockInfoPanel;
        [SerializeField] private Button testButton, resetButton;

        private void Start()
        {
            InitializeVariables();
            InitializeListeners();
        }

        private void InitializeVariables()
        {
            blockInfoPopup.gameObject.SetActive(false);
            blockInfoPanel.gameObject.SetActive(false);
            testButton.interactable = false;
            resetButton.gameObject.SetActive(false);
        }

        private void InitializeListeners()
        {
            GameManager.Instance.OnBlockHovered += OnBlockHovered;
            GameManager.Instance.OnBlockSelected += OnBlockSelected;

            GameManager.Instance.OnStackSelected += (stack) => { testButton.interactable = true; };
            GameManager.Instance.OnStackTestFinished += () => { testButton.interactable = true; };
            testButton.onClick.AddListener(OnTestButton);
            resetButton.onClick.AddListener(OnResetButton);
        }

        private void OnBlockHovered(IBlock block)
        {
            if (block == null)
            {
                blockInfoPopup.gameObject.SetActive(false);
            }
            else
            {
                blockInfoPopup.gameObject.SetActive(true);
                blockInfoPopup.Initialize(block.BlockTypeValue, block.BlockData.Grade, block.BlockData.Domain);
            }
        }

        private void OnBlockSelected(IBlock block)
        {
            if (block == null)
            {
                blockInfoPanel.gameObject.SetActive(false);
            }
            else
            {
                blockInfoPanel.gameObject.SetActive(true);
                blockInfoPanel.Initialize(block);
            }
        }

        private void OnTestButton()
        {
            GameManager.Instance.TestStack();
            testButton.gameObject.SetActive(false);
            resetButton.gameObject.SetActive(true);
        }

        private void OnResetButton()
        {
            GameManager.Instance.ResetStack();
            resetButton.gameObject.SetActive(false);
            testButton.gameObject.SetActive(true);
            testButton.interactable = false;
        }

        private void OnDestroy()
        {
            if (!GameManager.HasInstance) return;
            GameManager.Instance.OnBlockHovered -= OnBlockHovered;
            GameManager.Instance.OnBlockSelected -= OnBlockSelected;
        }
    }
}
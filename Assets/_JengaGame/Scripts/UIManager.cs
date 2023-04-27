using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JengaGame
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private BlockInfoPopup blockInfoPopup;
        [SerializeField] private BlockInfoPanel blockInfoPanel;

        private void Start()
        {
            blockInfoPopup.gameObject.SetActive(false);
            blockInfoPanel.gameObject.SetActive(false);
            InitializeListeners();
        }

        private void InitializeListeners()
        {
            GameManager.Instance.OnBlockHovered += OnBlockHovered;
            GameManager.Instance.OnBlockSelected += OnBlockSelected;
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

        private void OnDestroy()
        {
            if (!GameManager.HasInstance) return;
            GameManager.Instance.OnBlockHovered -= OnBlockHovered;
            GameManager.Instance.OnBlockSelected -= OnBlockSelected;
        }
    }
}
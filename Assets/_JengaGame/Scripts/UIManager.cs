using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JengaGame
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private BlockInfoPopup blockInfoPopup;

        private void Start()
        {
            blockInfoPopup.gameObject.SetActive(false);
            InitializeListeners();
        }

        private void InitializeListeners()
        {
            GameManager.Instance.OnBlockHovered += OnBlockHovered;
        }

        private void OnBlockHovered(IClickableBlock block)
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

        private void OnDestroy()
        {
            if (!GameManager.HasInstance) return;
            GameManager.Instance.OnBlockHovered -= OnBlockHovered;
        }
    }
}
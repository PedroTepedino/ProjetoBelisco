﻿using System.Collections;
using System.Collections.Generic;
using Rewired.Utils.Attributes;
using UnityEngine;
using TMPro;

namespace RefatoramentoDoTioTepe{
    public class SignScript : MonoBehaviour
    {
        public GameObject sign;

        void OnEnable()
        {
            HideText();
        }

        void ShowText()
        {
            sign.SetActive(true);
        }

        void HideText()
        {
            sign.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            ShowText();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            HideText();
        }
    }
}

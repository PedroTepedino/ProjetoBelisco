using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Belisco
{
    [RequireComponent(typeof(Selectable))]
    public class SelectOnEnable : MonoBehaviour
    {
        private void OnEnable()
        {
            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(this.gameObject);
            }
        }
    }   
}

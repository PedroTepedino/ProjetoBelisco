using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace RefatoramentoDoTioTepe
{
    public class SaveOptions : MonoBehaviour
    {
        private static SaveOptions _instance;

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
        }

        private void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }

        protected virtual void OnEnable()
        {
            _instance = this;
            LoadAllOptions();
        }

        public static void LoadAllOptions()
        {
            if (_instance == null)
                return;
            
            var optionsParameters = _instance.GetComponentsInChildren<OptionsParameter>();
            foreach (var param in optionsParameters)
            {
                param.Load();
            }   
        }

        public static void SaveAllOptions()
        {
            if (_instance == null)
                return;
            
            var optionsParameters = _instance.GetComponentsInChildren<OptionsParameter>();
            foreach (var param in optionsParameters)
            {
                param.Save();
            }
        }
    }
}

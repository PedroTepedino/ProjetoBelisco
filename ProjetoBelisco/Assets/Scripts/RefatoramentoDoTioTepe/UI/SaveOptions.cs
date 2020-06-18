using UnityEngine;
using Debug = UnityEngine.Debug;

namespace RefatoramentoDoTioTepe
{
    public class SaveOptions : MonoBehaviour
    {
        private static SaveOptions _instance;

        protected virtual void OnEnable()
        {
            _instance = this;
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

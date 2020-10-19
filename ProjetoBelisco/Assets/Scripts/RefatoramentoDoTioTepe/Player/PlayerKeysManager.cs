using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class PlayerKeysManager : MonoBehaviour
    {
        public bool HasKey => _keyObject != null;
        private Key _keyObject;
        
        public void AddKey(Key newKey)
        {
            _keyObject = newKey;
        }

        public void UseKey()
        {
            _keyObject = null;
        }
    }
}
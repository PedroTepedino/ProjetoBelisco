using UnityEngine;

namespace Belisco
{
    public class PlayerKeysManager : MonoBehaviour
    {
        private Key _keyObject;
        public bool HasKey => _keyObject != null;

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
using UnityEngine;

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

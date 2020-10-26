using UnityEngine;

namespace Belisco
{
    public class SignScript : MonoBehaviour
    {
        public GameObject sign;

        private void OnEnable()
        {
            HideText();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            ShowText();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            HideText();
        }

        private void ShowText()
        {
            sign.SetActive(true);
        }

        private void HideText()
        {
            sign.SetActive(false);
        }
    }
}
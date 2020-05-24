using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.DevAuxCode
{
    public class AttackDummyLifeBar : MonoBehaviour
    {
        [SerializeField] private AttackDummy _attackDummy;
        [SerializeField] private List<GameObject> _lifeBar;

        private void Awake()
        {
            _attackDummy.OnLifeChange += UpdateBar;
        
            _lifeBar = new List<GameObject>();
        
            GameObject aux = new GameObject("LifePoint");
        
            int maxHealth = _attackDummy.MaxHealth;
            float increment = 1f / (float) maxHealth;
            for (int i = 0; i < maxHealth; i++)
            {
                var obj = Instantiate(aux, parent: this.transform);
                var rect = obj.AddComponent<RectTransform>();
                rect.anchorMin = new Vector2(0f + ((float)i * increment) + (increment / 10f), 0.1f);
                rect.anchorMax = new Vector2(0f + ((float)(i + 1) * increment) - (increment / 10f), 0.9f);
                rect.localPosition = Vector3.zero;
                rect.localScale = Vector3.one;
                rect.anchoredPosition = Vector2.zero;
                rect.sizeDelta = Vector2.zero;

                var img = obj.AddComponent<Image>();
                img.color = Color.red;
                img.sprite = Resources.Load<Sprite>("Resources/PlaceHolders/Square.png");
            
                _lifeBar.Add(obj);
            }

            Destroy(aux);
        }

        private void OnDestroy()
        {
            _attackDummy.OnLifeChange -= UpdateBar;
        }

        private void UpdateBar(int current, int max)
        {
            //_lifeBar.fillAmount = (float)(current) / (float)(max);
            for (int i = 0; i < _lifeBar.Count; i++)
            {
                _lifeBar[i].SetActive(i < current);
            }
        }
    }
}

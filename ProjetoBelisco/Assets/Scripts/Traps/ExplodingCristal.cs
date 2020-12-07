using System;
using System.Runtime.Remoting;
using UnityEngine;

namespace Belisco
{
    public class ExplodingCristal : MonoBehaviour, IHittable
    {
        [SerializeField] private float _radius = 2f;
        [SerializeField] private float _force = 50f;
        [SerializeField] private float _delay = 1f;
        private float _timer = 0;
        private bool _setOff = false;

        private void OnEnable()
        {
            _setOff = false;
        }

        private void Update()
        {
            if (_setOff)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    Explode();
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                SetTimer();                
            }
        }

        public void Hit(int damage, Transform attacker)
        {
            Explode();
        }

        public void SetTimer()
        {
            _timer = _delay;
            _setOff = true;
        }

        public void Explode()
        {
            var objectsArray = new Collider2D[100];
            var size = Physics2D.OverlapCircleNonAlloc(this.transform.position, _radius, objectsArray);

            for (int i = 0; i < size; i++)
            {
                if (objectsArray[i].gameObject.CompareTag("Player"))
                {
                    objectsArray[i].GetComponent<Player>().PushPlayer((objectsArray[i].transform.position - this.transform.position).normalized * _force);
                }
                else
                {
                    var rigid = objectsArray[i].gameObject.GetComponent<Rigidbody2D>();
                    if (rigid != null)
                    {
                        rigid.AddForce((rigid.transform.position - this.transform.position).normalized * _force);
                    }
                }
            }

            this.gameObject.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            
            Gizmos.DrawWireSphere(this.transform.position, _radius);
        }
    }
}
using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Debug = UnityEngine.Debug;

public class EnemyProjectile : MonoBehaviour, IPooledObject
{
    [SerializeField] private float _timeToDespawn = 3f;
    [SerializeField] private int _damage;
    [SerializeField] private float _velocity = 5f;

    [SerializeField] private DOTweenAnimation _animation;

    private Rigidbody2D _rigidbody2D;

    private WaitForSeconds _despawnWait;

    private DOTweenCYInstruction.WaitForRewind _waitForRewind;

    private void Awake()
    {
        _despawnWait = new WaitForSeconds(_timeToDespawn);
    }

    public void OnObjectSpawn()
    {
        this.gameObject.SetActive(true);
        this.transform.localScale = Vector3.zero;
        this.GetComponent<Rigidbody2D>().velocity = this.transform.right * _velocity;
        StartCoroutine(WaitToDespawn());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerLife life = other.gameObject.GetComponent<PlayerLife>();
        if (life)
        {
            life.Damage(_damage);
        }
        
        this.gameObject.SetActive(false);
    }
    

    private IEnumerator WaitToDespawn()
    {
        yield return _despawnWait;
        
        _animation.tween.SmoothRewind();

        yield return _animation.tween.WaitForRewind();
        
        this.gameObject.SetActive(false);
    }
}

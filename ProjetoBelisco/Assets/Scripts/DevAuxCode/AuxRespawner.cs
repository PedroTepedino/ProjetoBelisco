using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class: AuxRespawner
 * Class to help the development.
 */
[RequireComponent(typeof(Collider2D))]
public class AuxRespawner : MonoBehaviour
{
    [SerializeField] private Transform _respawn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.transform.position = _respawn.transform.position;
    }
}

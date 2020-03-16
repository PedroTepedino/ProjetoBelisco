using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/* Class: PlayerLife
 * Manages the life system of the player and fires its events.
 */
public class PlayerLife : BaseLifeSystem
{
    // Group: Events

    /* Variables: Actions 
     * OnPlayerDamage - Sends a signal when the player have recived damage.
     * OnPlayerDie - Sends  a signal when the player Dies.
     */
    public static System.Action OnPlayerDamage;
    public static System.Action OnPlayerDie;

    // Group: Health Logic

    /* Function: Damage
     * Deal damage to the player, verifies if the player is still alive afterwerds, and sends the damage <signal: OnPlayerDamage>.
     * Parameters: 
     * healPoints - The number of points to replanish in the entitie's health.
     */
    protected override void Damage(int damagePoints = 1)
    {
        if (damagePoints <= 0)
        {
            return;
        }
        else
        {
            _curentHealthPoints -= damagePoints;
            OnPlayerDamage?.Invoke();

            if (IsDead)
            {
                Die();
            }
        }
    }

    /* Function: Die
     * Manages the death of the player, and sends the <signal: OnPlayerDie>.
     */
    protected override void Die()
    {
        OnPlayerDie?.Invoke();
        this.gameObject.SetActive(false);
    }
}

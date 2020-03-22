using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class: PlayerLife
 * Manages the life system of the player and fires its events.
 */
public class PlayerLife : BaseLifeSystem
{
    // Group: Events

    /* Variables: Actions 
     * OnPlayerDamage - Sends a signal when the player have recived damage.
     * OnPlayerDie - Sends  a signal when the player Dies.
     * OnPlayerHeal - Sends a signal when  the player is healed.
     * OnPlayerSpawn - Sends a signal when the player spawns.
     */
    public static System.Action<int, int> OnPlayerDamage;
    public static System.Action OnPlayerDie;
    public static System.Action<int, int> OnPlayerHeal;
    public static System.Action<GameObject> OnPlayerSpawn;

    // Group: Health Logic

    /* Function: Damage
     * Deal damage to the player, verifies if the player is still alive afterwerds, and sends the damage <signal: OnPlayerDamage>.
     * Parameters: 
     * healPoints - The number of points to replanish in the entitie's health.
     */
    [Sirenix.OdinInspector.Button]
    public override void Damage(int damagePoints = 1)
    {
        if (damagePoints <= 0)
        {
            return;
        }
        else
        {
            _curentHealthPoints -= damagePoints;
            OnPlayerDamage?.Invoke(_curentHealthPoints, _maximumHealth);

            if (IsDead)
            {
                Die();
            }
        }
    }

    /* Function: RestoreHealth
     * Restores a certein amount of the entitie health points, and sends a <signal: OnPlayerHeal>
     * Parameters: 
     * healPoints - The number of points to replanish in the entitie's health.
     */
    [Sirenix.OdinInspector.Button]
    public override void RestoreHealth(int healPoints = 1)
    {
        if (healPoints <= 0)
        {
            return;
        }
        else
        {
            _curentHealthPoints += healPoints;
            OnPlayerHeal?.Invoke(_curentHealthPoints, _maximumHealth);

            if (IsHealthFull)
            {
                _curentHealthPoints = _maximumHealth;
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


    public void RespawnPlayer()
    {
        this.RestoreHealth(_maximumHealth);
        OnPlayerSpawn?.Invoke(this.gameObject);
    }
}

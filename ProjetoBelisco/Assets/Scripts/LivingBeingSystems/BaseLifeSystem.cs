using UnityEngine;
using Sirenix.OdinInspector;

/* Class: LifeSystemAbstract
 * Abstract Class that describes the essencial components of any Life system.
 */
public abstract class BaseLifeSystem : MonoBehaviour
{
    // Group: Protected Variables

    /* Variables: Health Parameters
     * _maximumHealth - The maximum number of health points an entitie can have.
     * _curentHelathPoints - The amount of health points an entitie have at a given time.
     */
    [SerializeField] [BoxGroup("HealthParameters")] protected int _maximumHealth = 3;
    [ShowInInspector] [BoxGroup("HealthParameters")] protected int _curentHealthPoints = 0;

    // Group: Properties
    public bool IsDead { get => (_curentHealthPoints <= 0); }
    public bool IsHealthFull { get => (_curentHealthPoints >= _maximumHealth); }
    public int MaxHealth { get => _maximumHealth; }
    public int CurentHealth { get => _curentHealthPoints; }

    // Group: Unity Methods

    /* Function: Awake
     * Just restores the health  of the entitie on awake.
     */
    private void Awake()
    {
        _curentHealthPoints = _maximumHealth;
    }

    // Group: Health Logic 

    /* Function: RestoreHealth
     * Restores a certein amount of the entitie health points.
     * Parameters: 
     * healPoints - The number of points to replanish in the entitie's health.
     */
    public virtual void RestoreHealth(int healPoints = 1)
    {
        if (healPoints <= 0)
        {
            return;
        }
        else
        {
            _curentHealthPoints += healPoints;
            if (IsHealthFull)
            {
                _curentHealthPoints = _maximumHealth;
            }
        }
    }

    /* Function: Damage
     * Remove a certein amount of points from the entitie's health.
     * Parameters: 
     * damagePoints - The number of points to take from the entitie's health.
     */
    protected virtual void Damage(int damagePoints = 1)
    {
        if (damagePoints <= 0)
        {
            return;
        }
        else
        {
            _curentHealthPoints -= damagePoints;
            if (IsDead)
            {
                Die();
            }
        }
    }

    /* Function: Die
     * Function that is called when the entitie dies. 
     */
    protected abstract void Die();
}
/* Interface: IPooledObject
 * Interface to manage the spawning of objects.
 */
namespace GameScripts.PoolingSystem
{
    public interface IPooledObject
    {
        /* Function: OnObjectSpawn
     * What to do when spawn an object.
     */
        void OnObjectSpawn();
    }
}

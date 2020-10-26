/* Interface: IPooledObject
 * Interface to manage the spawning of objects.
 */

namespace Belisco
{
    public interface IPooledObject
    {
        /* Function: OnObjectSpawn
     * What to do when spawn an object.
     */
        void OnObjectSpawn(object[] parameters = null);
    }
}
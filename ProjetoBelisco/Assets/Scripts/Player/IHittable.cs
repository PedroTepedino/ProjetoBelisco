using UnityEngine;

namespace Belisco
{
    public interface IHittable
    {
        void Hit(int damage, Transform attacker = null);
    }
}
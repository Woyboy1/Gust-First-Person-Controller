using UnityEngine;

namespace GustFPC
{
    /// <summary>
    /// [Core Script]
    /// A parent class shared by all Player-based scripts,
    /// providing access to Player.cs and PlayerDependencies.cs.
    /// </summary>
    public abstract class PlayerComponent : MonoBehaviour
    {
        protected Player player;
        protected PlayerDependencies Dependencies { get; private set; }

        public virtual void InitializeDependencies()
        {
            player = GetComponentInParent<Player>();
            Dependencies = GetComponentInParent<PlayerDependencies>();
        }
    }
}
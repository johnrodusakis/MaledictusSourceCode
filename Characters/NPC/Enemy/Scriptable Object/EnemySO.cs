using Maledictus.Inventory;
using Obvious.Soap;
using UnityEngine;

namespace Maledictus.Enemy
{
    public enum EnemyType
    {
        None,
        Normal,
        Undead,
        Poison,
        Electric,
        Dark,
        Ice,
        Fire,
        Spirit
    }

    public enum EnemyStage
    {
        None,
        Stage1,
        Stage2,
        Stage3
    }

    [System.Serializable]
    public enum DiscoveryStage
    {
        Unknown,
        Encountered,
        Captured,
    }

    public enum EnemyCategory
    {
        Humanoid,
        Undead,
        Reptile,
        Cursed,
        Spirit,
        Corrupted,
        Legendary
    }

    [CreateAssetMenu(menuName = "Character/Enemy")]
    public class EnemySO : ScriptableObject
    {
        public bool IsActive = false;

        public string Name;
        [TextArea(1, 5)]
        public string Description;

        public EnemyCategory Category;
        public EnemyStage Stage;
        public Rarity Rarity;

        public Sprite Figure;

        public EnemyType Type1;
        public EnemyType Type2;

        public ScriptableVariable<DiscoveryStage> Discovery;

        private void OnValidate()
        {
            Name = this.name;
        }
    }
}
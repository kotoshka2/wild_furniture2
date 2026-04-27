using UnityEngine;

namespace Architechture.Ritual.Data
{
    public enum RitualComponentId
    {
        None,
        Heart,
        Eye,
        Ruby,
        Sapphire,
        Bone,
        BlackWood,
        SilverNail
    }

    [CreateAssetMenu(menuName = "Rituals/Component")]
    public class RitualComponentData : ScriptableObject
    {
        public RitualComponentId id;
        public string displayName;
        public GameObject worldPrefab;
        public Sprite icon;
    }
}
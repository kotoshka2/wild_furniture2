using UnityEngine;

namespace Architechture.Ritual.Data
{
    public enum FurnitureTier
    {
        Cheap,
        Normal,
        Expensive,
        Luxury
    }
    [CreateAssetMenu(menuName = "Furniture/Furniture")]
    public class FurnitureData : ScriptableObject
    {
        public string id;
        public string displayName;
        public FurnitureTier tier;
        public GameObject prefab;
        public int basePrice;
    }
}
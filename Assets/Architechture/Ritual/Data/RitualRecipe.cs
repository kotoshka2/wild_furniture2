using UnityEngine;

namespace Architechture.Ritual.Data
{
    [CreateAssetMenu(menuName = "Rituals/Recipe")]
    public class RitualRecipe : ScriptableObject
    {
        public string id;

        public RitualComponentId[] requiredComponents = new RitualComponentId[5];

        public int minSacrificeValue;

        public FurnitureData resultFurniture;
    }
}

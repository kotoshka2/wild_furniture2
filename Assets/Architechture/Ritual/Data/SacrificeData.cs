using UnityEngine;

namespace Architechture.Ritual.Data
{
    public enum SacrificeId
    {
        None,
        Chicken,
        Goat,
        Cow
    }
    [CreateAssetMenu(menuName = "Rituals/Sacrifice")]
    public class SacrificeData : ScriptableObject
    {
        public SacrificeId id;
        public string displayName;
        public int value;
    }
}
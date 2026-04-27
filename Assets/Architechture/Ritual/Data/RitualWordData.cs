using UnityEngine;

namespace Architechture.Ritual.Data
{
    [CreateAssetMenu(menuName = "Rituals/Word")]
    public class RitualWordData : ScriptableObject
    {
        public string word;
        public Color resultColor;
        public float mistakeTolerance = 0.8f;
    }
}

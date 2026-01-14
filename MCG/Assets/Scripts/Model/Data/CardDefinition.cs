using UnityEngine;

namespace MCG.Model.Data
{
    [CreateAssetMenu(fileName = "CardDefinition", menuName = "MCG/New Card Definition")]
    public class CardDefinition : ScriptableObject
    {
        [field: SerializeField]
        public string Id { get; set; }
        [field: SerializeField]
        public Sprite Sprite { get; set; }
    }
}
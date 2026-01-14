using MCG.Model.Data;

namespace MCG.Model.Domain.Cards
{
    public class Card : ICard
    {
        public CardDefinition Definition { get; }
        public CardState State { get; set; }

        public Card(CardDefinition definition)
        {
            Definition = definition;
            State = definition == null ? CardState.Empty : CardState.Hidden;
        }

        public void Reveal()
        {
            if (State == CardState.Hidden)
                State = CardState.Revealed;
        }

        public void Hide()
        {
            if (State == CardState.Revealed)
                State = CardState.Hidden;
        }

        public void Match()
        {
            if (State == CardState.Revealed)
                State = CardState.Matched;
        }
    }
}
using MCG.Model.Data;

namespace MCG.Model.Domain.Cards
{
    public enum CardState
    {
        Empty,
        Hidden,
        Revealed,
        Matched
    }

    public interface ICard
    {
        CardDefinition Definition { get; }
        CardState State { get; set; }

        void Reveal();

        void Hide();

        void Match();
    }
}
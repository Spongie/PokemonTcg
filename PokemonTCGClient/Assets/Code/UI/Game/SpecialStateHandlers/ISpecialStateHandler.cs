namespace Assets.Code.UI.Game.SpecialStateHandlers
{
    public interface ISpecialStateHandler
    {
        void Clicked(CardRenderer cardRenderer);
        void EnableButtons();
    }
}

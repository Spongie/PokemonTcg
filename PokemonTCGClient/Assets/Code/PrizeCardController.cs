namespace Assets.Code
{
    public class PrizeCardController : CardController
    {
        public bool Revealed;

        protected override bool ActivatePointerEnterOrExit() => Revealed;

        protected override bool RenderBackSide() => !Revealed;
    }
}

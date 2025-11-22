namespace Entities.MVC
{
    public class ViewPlayer : ViewBase
    {
        private readonly Model _model;
        
        public ViewPlayer(Entity owner, Model model) : base(owner)
        {
            _model = model;
        }
    }
}
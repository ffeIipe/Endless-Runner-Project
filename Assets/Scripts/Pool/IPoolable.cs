using Entities;

namespace Pool
{
    public interface IPoolable
    {
        public Entity Owner { get; set; }
        public void SetOwner(Entity owner) { }
        public void Activate();
        public void Deactivate();

    }
}
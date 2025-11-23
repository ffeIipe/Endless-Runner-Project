using Entities;

namespace Pool
{
    public interface IPoolable
    {
        public void Activate();
        
        public void Deactivate();

        public virtual void SetOwner(Entity owner) {}
    }
}
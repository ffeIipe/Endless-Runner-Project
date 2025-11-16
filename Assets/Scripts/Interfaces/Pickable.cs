using Entities;

namespace Interfaces
{
    public interface IPickable
    {
        void PickUp(Entity user);
        
        void Drop();
    }
}
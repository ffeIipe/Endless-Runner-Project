using Scriptables;
using UnityEngine;

namespace Entities.MVC
{
    public class Model
    {
        private EntityData _entityData;
        private Entity _owner;
        
        public Model(Entity owner, EntityData entityData)
        {
            Debug.Log("Model has been initialized.");
            _owner = owner;
            _entityData = entityData;
        }
        
        public void Move()
        {
            _owner.transform.position += new Vector3(Input.GetAxisRaw("Horizontal") * _entityData.speed, 0, 0);
        }
        
        public void Jump()
        {
            var jumpVector = new Vector3(0, _entityData.jumpForce, 0);
            _owner.transform.position += jumpVector;
        }

        private void Pick()
        {
            
        }

        private void Crouch()
        {
            
        }

        private void Interact()
        {
            
        }
        
        
    }
}
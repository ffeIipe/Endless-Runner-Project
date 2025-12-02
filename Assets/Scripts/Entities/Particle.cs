using Enums;
using Managers;
using Pool;
using UnityEngine;

namespace Entities
{
    public class Particle : MonoBehaviour, IPoolable
    {
        [SerializeField] private PoolableType poolableType;
        private ParticleSystem _particle;
    
        public Entity Owner { get; set; }
        private void Awake()
        {
            _particle = GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            StartCoroutine(FactoryManager.Instance.ReturnObjectWithLifeTime(
                poolableType,
                this,
                _particle.main.duration + 1f
            ));
        }   

        public void Activate()
        {
            gameObject.SetActive(true);
            _particle.Play();
        }

        public void Deactivate()
        {
            _particle.Stop();
            gameObject.SetActive(false);
        }
    }
}

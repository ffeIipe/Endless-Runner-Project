using Managers.SoundManagerFolder;
using UnityEngine;

namespace Scriptables
{
        
    [CreateAssetMenu(menuName = "SoundManagerData", fileName = "SoundManagerData")]
    public class SoundManagerData : ScriptableObject
    {
        public SoundList[] Sounds;
    }
}
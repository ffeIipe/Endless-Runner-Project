using System;
using System.Collections.Generic;
using Enums;
using Scriptables;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

#if UNITY_EDITOR
namespace Managers.SoundManagerFolder
{
    [CustomEditor(typeof(SoundManagerData))]
    public class SoundsDataEditor : Editor
    {
        private void OnEnable()
        {
            ref var soundList = ref ((SoundManagerData)target).Sounds;

            if (soundList == null)
                return;

            var names = Enum.GetNames(typeof(SoundType));
            var differentSize = names.Length != soundList.Length;

            Dictionary<string, SoundList> sounds = new();

            if (differentSize)
            {
                for (var i = 0; i < soundList.Length; ++i)
                {
                    sounds.Add(soundList[i].name, soundList[i]);
                }
            }

            Array.Resize(ref soundList, names.Length);
            for (var i = 0; i < soundList.Length; i++)
            {
                var currentName = names[i];
                soundList[i].name = currentName;
                if (soundList[i].volume == 0) soundList[i].volume = 1;

                if (!differentSize) continue;
                
                if (sounds.TryGetValue(currentName, out var current))
                {
                    UpdateElement(ref soundList[i], current.volume, current.sounds, current.mixer);
                }
                else UpdateElement(ref soundList[i], 1, Array.Empty<AudioClip>(), null);

                continue;

                static void UpdateElement(ref SoundList element, float volume, AudioClip[] sounds,
                    AudioMixerGroup mixer)
                {
                    element.volume = volume;
                    element.sounds = sounds;
                    element.mixer = mixer;
                }
            }
        }
    }
}
#endif
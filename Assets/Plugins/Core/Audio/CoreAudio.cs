using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TDC.Audio
{
    [AddComponentMenu("TDC/Audio/AudioManager")]
    public class CoreAudio : MonoBehaviour
    {
        public List<DataSound> listDataSound = new List<DataSound>();
        public GameObject prefabAudio;

        public void PlayOneShot(DataSound.TType type)
        {
            if (!Globals.Instance.coreProfile.sound)
            {
                return;
            }

            GameObject newAudio = Instantiate(prefabAudio, transform);

            var ASource = newAudio.GetComponent<AudioSource>();

            DataSound get = GetSound(type);

            if (get == null)
            {
                return;
            }

            ASource.clip = get.audio;
            ASource.Play();

            Destroy(newAudio, ASource.clip.length + 0.2f);
        }

        private DataSound GetSound(DataSound.TType type)
        {
            var bufferList = new List<DataSound>();

            foreach (DataSound sound in listDataSound)
                if (sound.type == type)
                {
                    bufferList.Add(sound);
                }

            if (bufferList.Count == 0)
            {
                return null;
            }

            return bufferList[Random.Range(0, bufferList.Count)];
        }

        public void PlayLoopShot(float timeDestroy = 0)
        {
        }

        [Serializable]
        public class DataSound
        {
            public enum TType
            {
                Hit,
                Fail,
                Boss,
                Point,
                Switch,
                Buy,
                SwitchWindow,
                EndGame,
                Destroy
            }

            public AudioClip audio;

            public TType type;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace API.Sound
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Ins;

        public AudioMixer mainMix;

        private List<AudioSource> sourcePool = new List<AudioSource>();

        [SerializeField]
        private AudioSource musicSource;

        public List<AudioClip> sfxClips;

        public List<AudioClip> musicClips;

        public float onSFXVol = 0f;

        public float onMusicVol = -10f;

        private bool isMusicOn;

        private bool isSFXOn;

        public bool IsMusicOn
        {
            get => isMusicOn;
        }

        public bool IsSFXOn
        {
            get => isSFXOn;
        }

        private void Awake()
        {
            if (Ins == null)
            {
                Ins = this;
                DontDestroyOnLoad(transform.root.gameObject);
            }
            else
            {
                if (Ins != this)
                {
                    Destroy(transform.root.gameObject);
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            if (musicSource == null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
                musicSource.outputAudioMixerGroup = mainMix.FindMatchingGroups("Music")[0];
                musicSource.loop = true;
                if (musicClips.Count > 0)
                {
                    musicSource.clip = musicClips[0];
                    musicSource.Play();
                }
            }
            isSFXOn = PlayerPrefs.GetInt("IsSFXOn", 1) == 1;
            isMusicOn = PlayerPrefs.GetInt("IsMusicOn", 1) == 1;
            mainMix.SetFloat("SFXVol", isSFXOn ? onSFXVol : -80f);
            mainMix.SetFloat("MusicVol", isMusicOn ? onMusicVol : -80f);
        }
        /// <summary>
        /// Turn on or off sfx volume
        /// </summary>
        /// <returns> Is sfx on </returns>
        public void TurnOnOffSFX()
        {
            isSFXOn = !isSFXOn;
            PlayerPrefs.SetInt("IsSFXOn", isSFXOn ? 1 : 0);
            mainMix.SetFloat("SFXVol", isSFXOn ? onSFXVol : -80f);
            //return isSFXOn;
        }
        /// <summary>
        /// Turn on or off music volume
        /// </summary>
        /// <returns> Is music on </returns>
        public void TurnOnOffMusic()
        {
            isMusicOn = !isMusicOn;
            PlayerPrefs.SetInt("IsMusicOn", isMusicOn ? 1 : 0);
            mainMix.SetFloat("MusicVol", isMusicOn ? onMusicVol : -80f);
            //return isMusicOn;
        }
        /// <summary>
        /// Turn on or off sfx volume
        /// </summary>
        /// <param name="IsOn">Is on or off</param>
        public void TurnOnOffSFX(bool IsOn)
        {
            isSFXOn = IsOn;
            PlayerPrefs.SetInt("IsSFXOn", isSFXOn ? 1 : 0);
            mainMix.SetFloat("SFXVol", isSFXOn ? onSFXVol : -80f);
        }
        /// <summary>
        /// Turn on or off music volume
        /// </summary>
        /// <param name="IsOn">Is on or off</param>
        public void TurnOnOffMusic(bool IsOn)
        {
            isMusicOn = IsOn;
            PlayerPrefs.SetInt("IsMusicOn", isMusicOn ? 1 : 0);
            mainMix.SetFloat("MusicVol", isMusicOn ? onMusicVol : -80f);
        }

        public AudioSource GetAudioSource()
        {
            for (int i = 0; i < sourcePool.Count; i++)
            {
                if (!sourcePool[i].isPlaying)
                {
                    return sourcePool[i];
                }
            }
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = mainMix.FindMatchingGroups("SFX")[0];
            sourcePool.Add(source);
            return source;
        }
        /// <summary>
        /// Play button click sound
        /// </summary>
        public void PlayButtonSFX()
        {
            AudioSource source = GetAudioSource();
            source.clip = sfxClips[0];
            source.loop = false;
            source.Play();
        }
        /// <summary>
        /// Play game sfx sound
        /// </summary>
        /// <param name="sfxId">Sfx id</param>
        /// <param name="IsLoop">Is this loop play</param>
        /// <returns>The playing audio source</returns>
        public AudioSource PlaySFX(int sfxId, bool IsLoop = false)
        {
            AudioSource source = GetAudioSource();
            source.clip = sfxClips[sfxId];
            source.loop = IsLoop;
            source.Play();
            return source;
        }
        /// <summary>
        /// Change the BGM
        /// </summary>
        /// <param name="musicId">BGM clip Id</param>
        public void ChangeBGM(int musicId)
        {
            musicSource.clip = musicClips[musicId];
            musicSource.loop = true;
            musicSource.Play();
        }
    }
}

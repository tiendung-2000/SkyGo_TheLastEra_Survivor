using UnityEngine;
using System.Collections;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    GamePlayController gameManager;
    public AudioClip MainMenuBGM, SelectBGM;
    [Space(20)]
    public AudioClip[] soundEffects;
    public AudioClip[] soundUI;
    public AudioClip[] coinsSound;
    public AudioClip[] ingameBGM;
    public AudioClip[] winLoseSound;
    public AudioClip[] gunSound;
    public AudioClip[] footSteps;
    public AudioSource music;
    public AudioSource sound;
    private Coroutine demo;

    //public bool isVbration;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        if (Instance != this)
        {
            DestroyImmediate(gameObject);
        }
    }

    void Start()
    {
        //gameManager = GameManager.Instance;
        Register();
        //isVbration = (PlayerPrefs.GetInt("Vibration") == 1) ? true : false;
        //#if !UNITY_EDITOR
        //        Vibration.Init();
        //#endif
        //StartCoroutine(SetUpSound());
    }
    public void Register()
    {

    }

    IEnumerator SetUpSound()
    {
        yield return new WaitForSeconds(0.01f);
        if (PlayerPrefs.GetInt("music") == 1)
        {
            gameManager.isMusic = true;
            music.mute = false;
        }
        else
        {
            gameManager.isMusic = false;
            music.mute = true;
        }
        if (PlayerPrefs.GetInt("sound") == 1)
        {
            gameManager.isSound = true;
            sound.mute = false;
        }
        else
        {
            gameManager.isSound = false;
            sound.mute = true;
        }
    }

    public void PlayGamePlayMusic()
    {
        music.volume = 0f;
        music.Play();
        music.DOFade(1f, 0.3f);
    }

    public void PlayMainMenuBGM()
    {
        music.clip = MainMenuBGM;
        music.volume = 0f;
        music.Play();
        music.DOFade(1f, 0.5f);
    }

    public void PlayStageBGM()
    {
        music.clip = SelectBGM;
        music.volume = 0f;
        music.Play();
        music.DOFade(0.8f, 0.5f);
    }

    public void PlayIngameBGM(int id)
    {
        //int m = UnityEngine.Random.Range(0, ingameBGM.Length);
        music.clip = ingameBGM[id];
        music.volume = 1f;
        music.Play();
        music.DOFade(1f, 0f);
    }

    public void PlayWinLoseSound(int id)
    {
        sound.PlayOneShot(winLoseSound[id]);
        music.Stop();
    }

    public void PlayFootStepSound()
    {
        int m = UnityEngine.Random.Range(0, footSteps.Length);
        sound.PlayOneShot(footSteps[m]);
    }

    public void PlayCoinsSound()
    {
        int m = UnityEngine.Random.Range(0, coinsSound.Length);
        sound.PlayOneShot(coinsSound[m]);
    }

    public void Play(int id, float time)
    {
        //curSong = songInfos[id];
        //if (curSong == null)
        //    return;
        //song.clip = curSong.audioClip;
        music.clip = soundEffects[id];
        music.volume = 0f;
        music.Play();
        music.DOFade(1f, 3f);
        music.time = time;
    }


    public void PlayDemo(int id)
    {
        Stop();
        //curSong = songInfos[id];
        //if (curSong == null)
        //    return;
        //song.clip = curSong.audioClip;
        demo = StartCoroutine(DemoSong(15f));
    }
    IEnumerator DemoSong(float demoTime)
    {
        while (true)
        {
            music.volume = 0f;
            music.Play();
            music.DOFade(1f, 3f);
            music.time = 0;
            yield return new WaitForSeconds(demoTime);
            music.DOFade(0f, 3f);
            yield return new WaitForSeconds(3f);
        }
    }

    public void Pause(float time)
    {
        music.DOFade(0f, time).SetUpdate(true);
        StartCoroutine(WaitPause(time));
    }
    IEnumerator WaitPause(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        music.Pause();
    }
    public void Resume()
    {
        music.UnPause();
        music.DOFade(1f, .3f).SetUpdate(true);
        //song.time = time;
    }
    public void Stop()
    {
        music.pitch = 1f;
        music.Stop();
        if (demo != null)
        {
            StopCoroutine(demo);
        }
    }
    public void SoundPlayOneShot(int id)
    {
        sound.PlayOneShot(soundEffects[id]);
    }

    public void SoundUIPlay(int id)
    {
        sound.PlayOneShot(soundUI[id]);
    }

    public void FadeStopSound()
    {
        sound.DOFade(0, 0f);
        //music.DOFade(0, 1f);
        StartCoroutine(StopSound());

    }

    public void FadePlaySound()
    {
        sound.DOFade(1f, 0f);
        //music.DOFade(0, 1f);
        StartCoroutine(PlaySound());
    }

    public void FadeStopMusic()
    {
        music.DOFade(0, 1f);
        //music.DOFade(0, 1f);
        //StartCoroutine(StopMusic());

    }

    public void FadePlayMusic()
    {
        music.DOFade(1f, 0);
    }

    public void PauseMusic()
    {
        music.Pause();
    }

    public void ResumeMusic()
    {
        music.UnPause();
    }



    IEnumerator StopMusicDelay()
    {
        yield return new WaitForSeconds(1.5f);
        music.Stop();
        music.volume = 1;
    }

    IEnumerator StopSound()
    {
        yield return new WaitForSeconds(0.1f);
        sound.Stop();
        sound.volume = 1;
    }

    IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(0.1f);
        sound.Play();
        sound.volume = 1;
    }

    public void PlayGunSound(int id)
    {
        sound.PlayOneShot(gunSound[id]);
        //        if (isVbration)
        //        {
        //#if !UNITY_EDITOR
        //            Vibration.VibratePop();
        //#endif
        //        }
    }

    public void VibrateStart()
    {
        //        if (isVbration)
        //        {
        //#if !UNITY_EDITOR
        //            Vibration.VibratePop();
        //#endif
        //        }
    }

    public void PlayGunDrawSound(int id)
    {
        //sound.PlayOneShot(gunDrawSound[id]);
    }
    public void PlayTest(AudioClip audio)
    {
        music.PlayOneShot(audio);
    }
    private void OnPlayerPause()
    {
        Pause(0);
    }
    private void OnPlayerResume()
    {
        Resume();
    }
    private void OnPlayerNearDeath()
    {
        Pause(0);
    }
    private void OnPlayerStop()
    {
        Stop();
    }
    private void OnPlayerHitCoin()
    {
        //SoundPlayOneShot(3);
        //        if (isVbration)
        //        {
        //#if !UNITY_EDITOR
        //            Vibration.VibratePop();
        //#endif
        //        }
    }
}
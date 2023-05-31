using UnityEngine;
using System.Collections;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Ins;
    [SerializeField] GamePlayController gameManager;
    public AudioClip MainMenuBGM, SelectBGM, bossFight;
    [Space(20)]
    public AudioClip[] soundEffects;
    public AudioClip[] soundUI;
    public AudioClip[] collectsSound;
    public AudioClip[] ingameBGM;
    public AudioClip[] winLoseSound;
    public AudioClip[] gunSound;
    public AudioClip[] skillSound;
    public AudioClip gunDrawSound;
    public AudioClip[] footSteps;
    public AudioSource music;
    public AudioSource sound;
    private Coroutine demo;

    //public bool isVbration;


    void Awake()
    {
        if (Ins == null)
        {
            Ins = this;
        }
    }

    void Start()
    {
        Register();
        StartCoroutine(SetUpSound());
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
            music.volume = .3f;
            //music.mute = false;
            //PlayerPrefs.SetInt("music", 0);
        }
        else
        {
            gameManager.isMusic = false;
            music.volume = 0f;
        }
        if (PlayerPrefs.GetInt("sound") == 1)
        {
            gameManager.isSound = true;
            sound.volume = 1f;
            //sound.mute = false;
            //PlayerPrefs.GetInt("sound", 0);
        }
        else
        {
            gameManager.isSound = false;
            sound.volume = 0;
            //sound.mute = true;
        }
    }

    public void PlayGamePlayMusic()
    {
        //music.volume = 0f;
        music.Play();
        //music.DOFade(1f, 0.3f);
    }

    public void PlayMainMenuBGM()
    {
        music.clip = MainMenuBGM;
        //music.volume = 0f;
        music.Play();
        //music.DOFade(.3f, 0.5f);
    }

    public void PlaySelectBGM()
    {

        music.clip = SelectBGM;
        //music.volume = 0f;
        music.Play();
        //music.DOFade(0.3f, 0.5f);

    }

    public void PlayBossFightBGM()
    {
        music.clip = bossFight;
        //music.volume = 0f;
        music.Play();
        //music.DOFade(0.3f, 0.5f);
    }

    public void PlayIngameBGM(int id)
    {
        //int m = UnityEngine.Random.Range(0, ingameBGM.Length);
        music.clip = ingameBGM[id];
        //music.volume = 0;
        music.Play();
        //music.DOFade(.3f, 0f);
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

    public void PlayCollectsSound(int id)
    {
        //int m = UnityEngine.Random.Range(0, collectsSound.Length);
        sound.PlayOneShot(collectsSound[id]);
    }

    public void Play(int id, float time)
    {
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
    public void SoundEffect(int id)
    {
        sound.PlayOneShot(soundEffects[id]);
    }

    public void SoundUIPlay(int id)
    {
        sound.PlayOneShot(soundUI[id]);
    }

    public void SoundOff()
    {
        sound.DOFade(0, 0f);
        //music.DOFade(0, 1f);
        StartCoroutine(StopSound());

    }

    public void SoundOn()
    {
        sound.DOFade(1f, 0f);
        //music.DOFade(0, 1f);
        StartCoroutine(PlaySound());
    }

    public void MusicOff()
    {
        music.volume = 0f;
    }

    public void MusicOn()
    {
        music.volume = .3f;
        music.mute = false;
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
    public void PlaySkillSound(int id)
    {
        sound.PlayOneShot(skillSound[id]);
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

    public void PlayGunDrawSound()
    {
        sound.PlayOneShot(gunDrawSound);
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


    public void GamePlayBGM()
    {
        switch (DynamicDataManager.Ins.CurLevel)
        {
            case 0:
                PlayIngameBGM(0);
                break;
            case 1:
                PlayIngameBGM(1);
                break;
            case 2:
                PlayIngameBGM(2);
                break;
            case 3:
                PlayIngameBGM(3);
                break;
            case 4:
                PlayIngameBGM(4);
                break;
            case 5:
                PlayIngameBGM(5);
                break;
            case 6:
                PlayIngameBGM(6);
                break;
            case 7:
                PlayIngameBGM(7);
                break;
            case 8:
                PlayIngameBGM(8);
                break;
            case 9:
                PlayIngameBGM(9);
                break;
        }
    }
}
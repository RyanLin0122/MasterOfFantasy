using UnityEngine;

public class AudioSvc : MonoBehaviour
{
    public static AudioSvc Instance = null;
    public AudioSource BgAudio;
    public AudioSource UiAudio;
    public AudioSource EmbiAudio;
    public AudioSource CharacterAudio;
    public AudioSource MiniGameUIAudio;

    public float MonsterVolume = 0.5f;
    public void InitSvc()
    {
        Instance = this;
        Debug.Log("Init AudioSvc...");
    }


    public void PlayBGMusic(string name, bool isLoop = true)
    {
        AudioClip audio = ResSvc.Instance.LoadAudio("Sound/BGM/" + name, true);
        if (BgAudio.clip == null)
        {
            BgAudio.clip = audio;
            BgAudio.loop = isLoop;
            BgAudio.Play();
        }
        else if(BgAudio.clip.name == audio.name)
        {
            BgAudio.Play();
            return;
        }
        else
        {
            BgAudio.clip = audio;
            BgAudio.loop = isLoop;
            BgAudio.Play();
        }
    }
    public void PlayembiAudio(string name, bool isLoop = true)
    {
        AudioClip audio = ResSvc.Instance.LoadAudio("Sound/BGM/" + name, true);
        if (audio != null)
        {
            EmbiAudio.clip = audio;
            EmbiAudio.loop = isLoop;
            EmbiAudio.Play();
        }

    }
    public void StopembiAudio()
    {
        if (EmbiAudio.clip != null)
        {
            EmbiAudio.Stop();
        }      
    }
    public void PlaySkillAudio(string path)
    {
        AudioClip audio = ResSvc.Instance.LoadAudio(path, true);
        CharacterAudio.clip = audio;
        CharacterAudio.Play();
    }
    public void PlayCharacterAudio(string name)
    {
        AudioClip audio = ResSvc.Instance.LoadAudio("Sound/etc/" + name, true);
        CharacterAudio.clip = audio;
        CharacterAudio.Play();
    }
    public void PlayUIAudio(string name)
    {
        AudioClip audio = ResSvc.Instance.LoadAudio("Sound/UI/" + name, true);
        UiAudio.clip = audio;
        UiAudio.Play();
    }
    public void PlayUIAudio_ForMiniGame(string name)
    {
        AudioClip audio = ResSvc.Instance.LoadAudio("Sound/MiniGame/" + name, true);
        UiAudio.clip = audio;
        UiAudio.Play();
    }
    public void PlayMiniGameUIAudio(string name)
    {
        AudioClip audio = ResSvc.Instance.LoadAudio("Sound/MiniGame/" + name, true);
        MiniGameUIAudio.clip = audio;
        MiniGameUIAudio.Play();
    }
}
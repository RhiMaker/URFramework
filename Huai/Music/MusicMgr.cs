using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicMgr : BaseManager<MusicMgr>
{
    private AudioSource bgm = null;
    private float bgmValue = 1;
    private GameObject soundObj = null;
    //音效列表
    private List<AudioSource> soundList = new List<AudioSource>();
    //音效大小
    private float soundValue = 1;
    public MusicMgr()
    {
        MonoManager.GetInstance().AddUpdateListener(Update);
    }
    private void Update()
    {
        for (int i = soundList.Count - 1; i >= 0; --i)
        {
            if (!soundList[i].isPlaying)
            {
                GameObject.Destroy(soundList[i]);
                soundList.RemoveAt(i);
            }
        }
    }
    public void PlayBkMusic(string name)
    {

        if (bgm == null)
        {
            GameObject obj = new GameObject();
            obj.name = "BGM";
            bgm = obj.AddComponent<AudioSource>();
        }
        //异步加载背景音乐，加载完成后播放
        ResMgr.GetInstance().LoadAsync<AudioClip>("Music/BGM/" + name, (clip) => {
            bgm.clip = clip;
            bgm.loop = true;
            bgm.volume = bgmValue;
            bgm.Play();
        });
    }
    public void changeBKValue(float v)
    {
        bgmValue = v;
        if (bgm == null)
        {
            return;
        }
      bgm.volume =bgmValue;
    }
    public void PauseBKMusic()
    {
        if (bgm == null)
        {
            return;
        }
       bgm.Pause();

    }
    /// <summary>
    /// 停止播放背景音乐
    /// </summary>
    public void StopBkMusic()
    {
        if (bgm == null)
        {
            return;
        }
        bgm.Stop();
    }
    public void PlaySound(string name, bool isLoop, UnityAction<AudioSource> callback = null)
    {
        if (soundObj == null)
        {
            soundObj = new GameObject();
            soundObj.name = "Sound";
        }
        //当音效资源异步加载结束后，再添加一个音效
        ResMgr.GetInstance().LoadAsync<AudioClip>("Music/Sound/" + name, (clip) => {
            AudioSource source = soundObj.AddComponent<AudioSource>();
            source.clip = clip;
            source.loop = isLoop;
            source.volume = bgmValue;
            source.Play();
            if (callback != null)
            {
                callback(source);
            }
            soundList.Add(source);
        });
    }
    public void StopSound(AudioSource source)
    {
        if (soundList.Contains(source))
        {
            soundList.Remove(source);
            source.Stop();
            GameObject.Destroy(source);
        }
    }
    public void ChangeSoundValue(float value)
    {
        soundValue = value;
        for (int i = 0; i < soundList.Count; i++)
        {
            soundList[i].volume = value;
        }
    }
}

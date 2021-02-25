using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonBase<SoundManager>
{
    public List<AudioClip> BGM;
    public List<AudioClip> SFX;

    public int maxSFXSource = 5;
    AudioSource BGMsource;
    AudioSource[] SFXSources;
    private void OnEnable()
    {/*
        BGMsource = gameObject.AddComponent<AudioSource>();
        BGMsource.playOnAwake = false;
        BGMsource.volume = 1;
        BGMsource.loop = true;
        PlayBGM(BGM[0].name);*/
        SFXSources = new AudioSource[maxSFXSource];
        for (int i = 0; i < SFXSources.Length; i++)
        {
            SFXSources[i] = gameObject.AddComponent<AudioSource>();
            SFXSources[i].playOnAwake = false;
            SFXSources[i].volume = 1;
            SFXSources[i].loop = false;
        }
    }
    public void PlayBGM(string name, bool isLoop = true, float volume = 1.0f)
    {
        for (int i = 0; i < BGM.Count; i++)
        {
            if (BGM[i].name.CompareTo(name) == 0)
            {
                BGMsource.clip = BGM[i];

                BGMsource.volume = volume;
                BGMsource.loop = isLoop;
                BGMsource.Play();
                return;
            }
        }
        Debug.LogError(name + " is not exist!");
    }
    public void StopBGM()
    {
        if (BGMsource)
        {
            if (BGMsource.isPlaying)
            {
                BGMsource.Stop();
            }
        }
    }
    public void PlaySFX(string name, bool isLoop = false, float volume = 1.0f)
    {
        for (int i = 0; i < SFX.Count; i++)
        {
            if (SFX[i].name.CompareTo(name) == 0)
            {
                AudioSource source = GetEmptyAudio();
                source.clip = SFX[i];

                source.volume = volume;
                source.loop = isLoop;
                source.Play();
                return;
            }
        }
        Debug.LogError(name + " is not exist!");
    }
    public void StopSFX(string name)
    {
        for (int i = 0; i < SFXSources.Length; i++)
        {
            if (SFXSources[i].isPlaying)
            {
                if (SFXSources[i].clip.name.CompareTo(name) == 0)
                {
                    SFXSources[i].Stop();
                }
            }
        }

    }
    private AudioSource GetEmptyAudio()
    {
        float maxP = 0;
        int maxIdx = 0;
        for (int i = 0; i < SFXSources.Length; i++)
        {
            if (!SFXSources[i].isPlaying)
            {
                return SFXSources[i];
            }
            float p = SFXSources[i].time / SFXSources[i].clip.length;
            if (p > maxP && !SFXSources[i].loop)
            {
                maxP = p;
                maxIdx = i;
            }
        }
        return SFXSources[maxIdx];

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.playOnAwake = false;
            s.source.loop = false;
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
        }
    }

    public void PlayAudio(string audioName)
    {
        Sound soundToPlay = Array.Find<Sound>(sounds, x => x.name == audioName);
        /*if (soundToPlay.clipsPlaying > 0)
        {
            StartCoroutine(PlayOneShotDelayed(0.05f * soundToPlay.clipsPlaying, soundToPlay.source, soundToPlay.clip));
            soundToPlay.clipsPlaying++;
            StartCoroutine(DecrementClipsPlaying(soundToPlay.clip.length, soundToPlay));
        }
        else
        {
            soundToPlay.source.PlayOneShot(soundToPlay.clip);
            soundToPlay.clipsPlaying++;
            StartCoroutine(DecrementClipsPlaying(soundToPlay.clip.length, soundToPlay));
        }*/
        soundToPlay.source.Play();
    }

    private IEnumerator DecrementClipsPlaying(float delay, Sound soundToPlay)
    {
        yield return new WaitForSeconds(delay);
        soundToPlay.clipsPlaying--;
    }

    private IEnumerator PlayOneShotDelayed(float delay, AudioSource source, AudioClip clip)
    {
        yield return new WaitForSeconds(delay);
        source.PlayOneShot(clip);
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public float pitch = 1;
    public float volume = 1;

    [HideInInspector] public AudioSource source;
    [HideInInspector] public int clipsPlaying;
}
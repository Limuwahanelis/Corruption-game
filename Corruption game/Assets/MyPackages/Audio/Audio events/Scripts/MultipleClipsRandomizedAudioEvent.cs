using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio Event/MultipleRandomizedClipsEvent")]
public class MultipleClipsRandomizedAudioEvent : AudioEvent
{

    [MinMaxRange(0, 1)]
    public RangedFloat volume;
    [MinMaxRange(0, 2)]
    public RangedFloat pitch;
    [SerializeField] AudioClip[] audioclips;
    private bool canOverride;
    public override void Play(AudioSource audioSource)
    {
        audioSource.clip = audioclips[Random.Range(0, audioclips.Length)];
        float volumef = Random.Range(volume.minValue, volume.maxValue);
        audioSource.volume = volumef * (AudioVolumes.Master / 100.0f) * (AudioVolumes.SFX / 100.0f);
        audioSource.pitch = Random.Range(pitch.minValue, pitch.maxValue);
        if (audioSource.isPlaying)
        {
            if (!canOverride) return;
        }

        audioSource.Play();
    }
    //public override void Play(AudioSource audioSource, bool overPlay = false)
    //{
    //    audioSource.clip = audioClip;
    //    audioSource.volume = volume * (AudioVolumes.Master / 100.0f) * (AudioVolumes.SFX / 100.0f);
    //    audioSource.pitch = pitch;
    //    if (!overPlay) return;
    //    audioSource.Play();
    //}
    public override void Preview(AudioSource audioSource, float masterVol, float multVol)
    {
        audioSource.clip = audioclips[Random.Range(0, audioclips.Length)];
        float volumef = Random.Range(volume.minValue, volume.maxValue);
        audioSource.volume = volumef * (AudioVolumes.Master / 100.0f) * (AudioVolumes.SFX / 100.0f);
        audioSource.pitch = Random.Range(pitch.minValue, pitch.maxValue);
        audioSource.Play();
    }
}

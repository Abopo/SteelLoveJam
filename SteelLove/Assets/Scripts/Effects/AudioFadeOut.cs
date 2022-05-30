using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public static class AudioFadeOut
{

    static List<AudioSource> _audioSources = new List<AudioSource>();

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime) {
        if (_audioSources.Contains(audioSource)) {
            // Bail
            yield break;
        }

        _audioSources.Add(audioSource);
        float startVolume = audioSource.volume;

        audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
        while (audioSource.volume > 0.05f && audioSource.volume != startVolume) {
            yield return null;

            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
        }

        // If the audio sources volume gets reset, it's being used again, so don't stop it
        if (audioSource.volume != startVolume) {
            audioSource.Stop();
        }
        audioSource.volume = startVolume;
        _audioSources.Remove(audioSource);
    }

}
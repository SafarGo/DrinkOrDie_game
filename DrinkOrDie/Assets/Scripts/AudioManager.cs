using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] tracks;
    private int currentTrackIndex = 0;

    private void Start()
    {
        PlayTrack(currentTrackIndex);
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    private void PlayTrack(int index)
    {
        audioSource.clip = tracks[index];
        audioSource.Play();
    }

    private void PlayNextTrack()
    {
        currentTrackIndex++;
        if (currentTrackIndex >= tracks.Length)
            currentTrackIndex = 0;
        PlayTrack(currentTrackIndex);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Plugins.Audio.Core;
using Plugins.Audio.Utils;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using YG;

public class AudioControl : MonoBehaviour
{
    [SerializeField] private AudioMixer AudioMixerSF;
    [SerializeField] private SourceAudio BackgroundMusicSF;
    [SerializeField] private SourceAudio ClickSF;
    [SerializeField] private SourceAudio TrueSF;
    [SerializeField] private SourceAudio FalseSF;
    [SerializeField] private AudioDataProperty ClipBackgroundMusicSF;
    [SerializeField] private AudioDataProperty ClipClickSF;
    [SerializeField] private AudioDataProperty ClipTrueSF;
    [SerializeField] private AudioDataProperty ClipFalseSF;
    private bool _isMusicOn;
    private bool _isSoundsOn;

    private void Awake()
    {
        _isMusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        _isSoundsOn = PlayerPrefs.GetInt("SoundsOn", 1) == 1;
    }

    private void Start()
    {
        if (!YandexGame.nowFullAd) PlayMusic();
        else
            YandexGame.CloseFullAdEvent += PlayMusic;
    }

    private void OnDestroy()
    {
        YandexGame.CloseFullAdEvent -= PlayMusic;
    }

    public void PlayMusic()
    {
        if (_isMusicOn) BackgroundMusicSF.Play(ClipBackgroundMusicSF.Key);
        PlayerPrefs.SetInt("MusicOn", 1);
    }
    public void ToggleMusic()
    {
        if (_isMusicOn)
            PauseMusic();
        else
            BackgroundMusicSF.Play(ClipBackgroundMusicSF.Key);
        _isMusicOn = !_isMusicOn;
        PlayerPrefs.SetInt("MusicOn", _isMusicOn ? 1 : 0);
    }

    public void ToggleSounds()
    {
        _isSoundsOn = !_isSoundsOn;
        PlayerPrefs.SetInt("SoundsOn", _isSoundsOn ? 1 : 0);
    }

    public void MuteMusic()
    {
        BackgroundMusicSF.Mute = true;
    }
    public void UnMuteMusic()
    {
        BackgroundMusicSF.Mute = false;
    }
    public void PauseMusic()
    {
        BackgroundMusicSF.Pause();
    }
    public void UnPauseMusic()
    {
        BackgroundMusicSF.UnPause();
    }
    public void PlayClick()
    {
        if (_isSoundsOn) ClickSF.Play(ClipClickSF.Key);
    }
    public void PlayTrue()
    {
        if (_isSoundsOn) TrueSF.Play(ClipTrueSF.Key);
    }
    public void PlayFalse()
    {
        if (_isSoundsOn) FalseSF.Play(ClipFalseSF.Key);
    }

}

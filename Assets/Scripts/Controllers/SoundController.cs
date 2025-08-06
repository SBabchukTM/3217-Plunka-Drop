using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;

public class SoundController : MonoBehaviour, IService
{
    [SerializeField] private AudioSource _soundAudioSource;
    [SerializeField] private AudioSource _musicAudioSource;

    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private AudioClip _gameMusic;

    [SerializeField] private AudioClip _multiplierSound;
    [SerializeField] private AudioClip _pressedButtonSound;
    [SerializeField] private AudioClip _winSound;
    [SerializeField] private AudioClip _loseSound;

    private bool _isMusicEnabled;
    private bool _isSoundEnabled;

    private AudioClip _lastRequestedMusic;

    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<SettingsChangedSignal>(OnSettingsChanged);

        LoadSettings();
    }

    private void LoadSettings()
    {
        _isMusicEnabled = PlayerPrefs.GetInt(StringConstants.MUSIC_KEY, 1) == 1;
        _isSoundEnabled = PlayerPrefs.GetInt(StringConstants.SOUND_KEY, 1) == 1;
    }

    private void OnSettingsChanged(SettingsChangedSignal signal)
    {
        LoadSettings();

        if (_isMusicEnabled)
        {
            // якщо в налаштуванн€х ув≥мкнули музику - програЇмо останню запитану
            if (_lastRequestedMusic != null)
            {
                PlayMusic(_lastRequestedMusic);
            }
        }
        else
        {
            StopMusic();
        }
    }

    public void PlayMenuMusic()
    {
        _lastRequestedMusic = _menuMusic;

        if (_isMusicEnabled)
        {
            PlayMusic(_menuMusic);
        }
    }

    public void PlayGameMusic()
    {
        _lastRequestedMusic = _gameMusic;

        if (_isMusicEnabled)
        {
            PlayMusic(_gameMusic);
        }
    }

    public void StopMusic()
    {
        _musicAudioSource.Stop();
        _musicAudioSource.clip = null;
    }

    private void PlayMusic(AudioClip clip)
    {
        if (clip == null)
            return;

        if (_musicAudioSource.clip == clip && _musicAudioSource.isPlaying)
            return;

        _musicAudioSource.clip = clip;
        _musicAudioSource.loop = true;
        _musicAudioSource.Play();
    }

    public void PlayButtonSound()
    {
        PlaySound(_pressedButtonSound);
    }

    public void PlayMultiplierSound()
    {
        PlaySound(_multiplierSound);
    }

    public void PlayWinSound()
    {
        PlaySound(_winSound);
    }

    public void PlayLoseSound()
    {
        PlaySound(_loseSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (_isSoundEnabled && clip != null)
            _soundAudioSource.PlayOneShot(clip);
    }
}

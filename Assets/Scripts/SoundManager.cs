using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [System.Serializable]
    public class Sound
    {
        [SerializeField] private AudioClip _clip;
        [HideInInspector] public int simultaneousPlayCount;

        public AudioClip Clip => _clip;
    }

    [Header("Sound Settings")]
    [SerializeField] private int _maxSimultaneousSounds = 7;

    [Header("Game Sounds")]
    [SerializeField] private Sound _button;
    [SerializeField] private Sound _coin;
    [SerializeField] private Sound _move;
    [SerializeField] private Sound _gameOver;
    [SerializeField] private Sound _finish;
    [SerializeField] private Sound _rewarded;
    [SerializeField] private Sound _menuMusic;
    [SerializeField] private Sound _gameMusic;
    [SerializeField] private Sound _gameWinMusic;
    [SerializeField] private Sound _tick;

    private AudioSource _audioSource;

    public Sound Button => _button;
    public Sound Coin => _coin;
    public Sound Move => _move;
    public Sound GameOver => _gameOver;
    public Sound Finish => _finish;
    public Sound Rewarded => _rewarded;
    public Sound Menu => _menuMusic;
    public Sound Game => _gameMusic;
    public Sound GameWin => _gameWinMusic;
    public Sound Tick => _tick;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(Sound sound)
    {
        if (sound.simultaneousPlayCount >= _maxSimultaneousSounds) return;

        StartCoroutine(PlaySoundCoroutine(sound));
    }

    private IEnumerator PlaySoundCoroutine(Sound sound)
    {
        sound.simultaneousPlayCount++;
        _audioSource.PlayOneShot(sound.Clip);

        float delay = sound.Clip.length * 0.7f;
        yield return new WaitForSeconds(delay);

        sound.simultaneousPlayCount--;
    }

    public void PlayMusic(Sound music)
    {
        if (IsMusicOff) return;

        _audioSource.clip = music.Clip;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    public void ToggleMusic()
    {
        PlayerPrefs.SetInt("MusicOn", IsMusicOff ? 1 : 0);

        if (IsMusicOff)
        {
            _audioSource.Stop();
        }
        else if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }

    public bool IsMusicOff => PlayerPrefs.GetInt("MusicOn", 1) == 0;
}
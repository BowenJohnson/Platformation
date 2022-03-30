using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Object sends data from the sliders in the options UI to the
// corresponding audio mixer tracks
public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private Slider _musicVolume;
    [SerializeField] private Slider _sfxVolume;

    private void Start()
    {
        // get the audio mixer tracks and set the sliders to those values
        _audioMixer.GetFloat("MusicVolume", out float musicTmp);
        _musicVolume.value = musicTmp;

        _audioMixer.GetFloat("SfxVolume", out float sfxTmp);
        _sfxVolume.value = sfxTmp;
    }

    // sets the music volume track in mixer to slider input from UI
    public void SetMusicVolume(float volume)
    {
        _audioMixer.SetFloat("MusicVolume", volume);
    }

    // sets the SFX volume track in mixer to slider input from UI
    public void SetSfxVolume(float volume)
    {
        _audioMixer.SetFloat("SfxVolume", volume);
    }
}

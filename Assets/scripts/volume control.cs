using UnityEngine;
using UnityEngine.UI;

public class MusicAndVFXVolumeControl : MonoBehaviour
{
    // References to the AudioSource and Slider for Music and VFX
    public AudioSource musicSource;      // The music AudioSource
    public AudioSource vfxSource;        // The VFX AudioSource (if you have any VFX sounds)
    public Slider musicVolumeSlider;     // The UI Slider to control music volume
    public Slider vfxVolumeSlider;       // The UI Slider to control VFX volume

    private void Start()
    {
        // Ensure sliders are initialized with current volume values
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = musicSource.volume;
            musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);  // Add listener to music slider
        }

        if (vfxVolumeSlider != null)
        {
            vfxVolumeSlider.value = vfxSource.volume;
            vfxVolumeSlider.onValueChanged.AddListener(UpdateVFXVolume);  // Add listener to VFX slider
        }
    }

    // Update the music volume based on the slider value
    public void UpdateMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;  // Update the volume of the music AudioSource
        }
    }

    // Update the VFX volume based on the slider value
    public void UpdateVFXVolume(float volume)
    {
        if (vfxSource != null)
        {
            vfxSource.volume = volume;  // Update the volume of the VFX AudioSource
        }
    }
}

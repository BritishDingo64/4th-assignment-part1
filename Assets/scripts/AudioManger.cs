using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--------- Audio Sources -----------")]
    [SerializeField] private AudioSource musicSource;  // Background music audio source
    [SerializeField] private AudioSource SFXSource;    // General sound effects audio source

    [Header("--------- Audio Clips -----------")]
    public AudioClip background;      // Background music clip
    public AudioClip UISound;       // Jumpscare sound effect
    public AudioClip gunFire;         // Gun fire sound effect
    public AudioClip doorPhase;        // Door open sound effect

    void Start()
    {
        // Start background music when the game begins
        if (background != null && musicSource != null)
        {
            musicSource.clip = background;
            musicSource.loop = true;  // Loop the background music
            musicSource.Play();       // Play the background music
        }
    }

    // Method to get the current music volume
    public float GetMusicVolume()
    {
        if (musicSource != null)
        {
            return musicSource.volume;
        }
        return 0.1f;  // Default volume if the musicSource is not found
    }

    // Method to adjust the volume of the background music
    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = Mathf.Clamp01(volume);  // Clamp volume between 0 and 1
        }
    }

    // Play the jumpscare sound effect
    public void PlayUISound()
    {
        if (UISound != null && SFXSource != null)
        {
            SFXSource.PlayOneShot(UISound);  // Play jumpscare sound once
        }
    }

    // Play the gunfire sound effect
    public void PlayGunFire()
    {
        if (gunFire != null && SFXSource != null)
        {
            SFXSource.PlayOneShot(gunFire);  // Play gunfire sound once
        }
    }


    // Play the door open sound effect
    public void PlayDoorPhase()
    {
        if (doorPhase != null && SFXSource != null)
        {
            SFXSource.PlayOneShot(doorPhase);  // Play door open sound once
        }
    }
}

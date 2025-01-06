using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--------- Audio Sources -----------")]
    [SerializeField] private AudioSource musicSource;  // Background music audio source
    [SerializeField] private AudioSource SFXSource;    // General sound effects audio source
    [SerializeField] private AudioSource footstepSource;  // Separate audio source for footsteps

    [Header("--------- Audio Clips -----------")]
    public AudioClip background;      // Background music clip
    public AudioClip jumpscare;       // Jumpscare sound effect
    public AudioClip gunFire;         // Gun fire sound effect
    public AudioClip gunReload;       // Gun reload sound effect
    public AudioClip walking;         // Walking sound effect
    public AudioClip doorOpen;        // Door open sound effect

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
    public void PlayJumpscare()
    {
        if (jumpscare != null && SFXSource != null)
        {
            SFXSource.PlayOneShot(jumpscare);  // Play jumpscare sound once
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

    // Play the gun reload sound effect
    public void PlayGunReload()
    {
        if (gunReload != null && SFXSource != null)
        {
            SFXSource.PlayOneShot(gunReload);  // Play gun reload sound once
        }
    }

    // Play the walking sound effect
    public void PlayWalking()
    {
        if (walking != null && footstepSource != null)
        {
            footstepSource.clip = walking;  // Set the walking clip
            footstepSource.loop = true; // Loop the walking sound
            footstepSource.Play(); // Play the walking sound
        }
    }

    // Stop the walking sound effect
    public void StopWalking()
    {
        if (footstepSource != null && footstepSource.isPlaying && footstepSource.clip == walking)
        {
            footstepSource.Stop();  // Stop the walking sound from the separate source
        }
    }

    // Play the door open sound effect
    public void PlayDoorOpen()
    {
        if (doorOpen != null && SFXSource != null)
        {
            SFXSource.PlayOneShot(doorOpen);  // Play door open sound once
        }
    }
}

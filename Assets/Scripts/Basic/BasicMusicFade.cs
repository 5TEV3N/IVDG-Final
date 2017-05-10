using System.Collections;
using UnityEngine;

public class BasicMusicFade
{
    // TO BE REMOVED AS IT CURRENTLY HAS NO USE
    public void FadeMusic(AudioSource sound, float newSoundVolume, bool fadeOut)
    {
        if (fadeOut == true)
        {
            sound.volume = Mathf.Lerp(sound.volume, 0f, Time.deltaTime);
        }
        if (fadeOut == false)
        {
            sound.volume = Mathf.Lerp(sound.volume, newSoundVolume, Time.deltaTime);
        }
    }
}

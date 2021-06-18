#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
#endregion


/// <summary>
///		The Sound Effect
///		The key used to play an audio effect from the audio manager instance 
/// </summary>
public enum SoundEffect
{
	// Music 
	BackgroundMusic,
	Ambience,
	// Game 
	Footsteps,
	// UI 
	GUI_Selected,
	GUI_Move,
	GUI_MenuTransition,
	GUI_Confirmed
}

/// <summary>
///		A Static Class for Handling Audio Effects 
/// </summary>
public static class AudioManager
{ 

	/// <summary>
	///		Reference to an Audio Mixer Group 
	/// </summary>
	private static AudioMixerGroup m_AudioMixer;

	/// <summary>
	///		Reference to the Game Assets Script 
	/// </summary>
	private static GameAssets m_GameAssetsReference;

	public static void SetupSounds(SoundFX[] Sounds, AudioMixerGroup Mixer)
	{

		m_AudioMixer = Mixer;
		m_GameAssetsReference = Object.FindObjectOfType<GameAssets>();

		if (m_GameAssetsReference != null)
		{ 
		
			foreach (SoundFX sound in Sounds)
			{

				// Add the audio source component to the game assets game object 
				sound.source = m_GameAssetsReference.gameObject.AddComponent<AudioSource>();
			
				// Set the audio source clip to the audio clip added  
				sound.source.clip = sound.clip; 

				// Set whether the audio source should loop 
				sound.source.loop = sound.loop;

				// Set whether the audio source should play on awake 
				sound.source.playOnAwake = sound.awake;

				// Set the pitch of the audio source 
				sound.source.pitch = sound.pitch;
				
				// Set the volume of the audio source 
				sound.source.volume = sound.volume;

				// Set the Sound Effects Length based on the clips length 
				sound.length = sound.source.clip.length;



				sound.source.outputAudioMixerGroup = m_AudioMixer;
			}
		}
		else
		{
			Debug.LogWarning("[AudioManager.SetupSounds]: " + "Could not find Game Assets Reference!");
		}
	}

	public static bool IsPlayingSound(SoundEffect p_SoundEffect) => GetSoundEffect(p_SoundEffect).source.isPlaying == true;

	/// <summary>
	///		Plays a Sound Effect using the Sound Category Type 
	/// </summary>
	/// <param name="p_SoundCategory"></param>
	public static void PlaySound(SoundEffect p_SoundCategory)
	{
		SoundFX sound = GetSoundEffect(p_SoundCategory);
		if (sound == null)
		{
			Debug.LogWarning("[AudioManager.PlaySound]: " + "Could not find " + p_SoundCategory + " sound effect!");
			return;
		}

		sound.source.volume = sound.volume;
		sound.source.playOnAwake = sound.awake;
		sound.source.loop = sound.loop;
		sound.source.pitch = sound.pitch;
		sound.source.Play();
	}

	/// <summary>
	///		Get the SoundFX by the SoundCategory - Which is basically the sound's name 
	/// </summary>
	/// <param name="p_SoundCategory">The sound effects name </param>
	/// <returns></returns>
	private static SoundFX GetSoundEffect(SoundEffect p_SoundCategory)
	{
		foreach (SoundFX s_GameSoundEffect in GameAssets.instance.SoundEffects)
		{
			if (s_GameSoundEffect.Category == p_SoundCategory)
				return s_GameSoundEffect;
		}
		return null;
	}

}

#region Namespaces
using UnityEngine;
using UnityEngine.Audio;
using System.Reflection;
#endregion



/// <summary>
///		Displays Game Assets in the Inspector 
/// </summary>
public class GameAssets : MonoBehaviour 
{

	/// <summary>
	///		Reference to the static Game Assets Instance 
	/// </summary>
	public static GameAssets instance;


	#region Public Variables 

	[Header("Audio")]
	
	/// <summary>
	///  Reference to the Audio Mixer Group 
	/// </summary>
	[SerializeField] protected AudioMixerGroup m_AudioMixer;

	/// <summary>
	///		An array of game sound effect assets 
	/// </summary>
	[SerializeField] protected SoundFX[] m_SoundEffects;

	/// <summary>
	///		Returns the sound effects array 
	/// </summary>
	public SoundFX[] SoundEffects => m_SoundEffects;

	#endregion

	#region Unity References 

	private void OnEnable()
	{
		if (instance != null && instance != this)
		Destroy(instance.gameObject);
		
		instance = this;

		AudioManager.SetupSounds(m_SoundEffects, m_AudioMixer);
	}

	#endregion

}

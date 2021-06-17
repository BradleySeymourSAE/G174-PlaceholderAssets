#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#endregion


/// <summary>
///     Project Showcase Script for handling audio and UI events 
/// </summary>
public class Showcase : MonoBehaviour
{

	public enum SnowStates { Active, InActive };

	public static Showcase instance;
	public static UnityEvent ToggleSnowEvent = new UnityEvent();

	public ParticleSystem SnowParticleSystem;

	[Range(0.01f, 0.1f)] public float snowSpeed;

	#region Private Variables

	[SerializeField] private float m_SnowLevelValue = 0;
	[SerializeField] private bool m_SnowfallEnabled = false;


	public SnowStates currentSnowState
	{ 
		get 
		{
			if (m_SnowfallEnabled)
				return SnowStates.Active;
			else
				return SnowStates.InActive;
		}
	}


#endregion

#region Unity References

private void OnEnable()
	{
		if (instance != null && instance != this)
			Destroy(instance.gameObject);

		instance = this;

		m_SnowfallEnabled = false;
		m_SnowLevelValue = 0;
		
		if (SnowParticleSystem.isPlaying)
		{
			SnowParticleSystem.Stop();
		}


		ToggleSnowEvent.AddListener(ToggleSnow);
	}

	private void OnDisable()
	{
		ToggleSnowEvent.RemoveListener(ToggleSnow);
	}

	private void Update()
	{
		if (!m_SnowfallEnabled)
		{
			return;
		}
		
		if (m_SnowfallEnabled)
		{ 

			if (m_SnowLevelValue < 1f)
			{
				Shader.SetGlobalFloat("_SnowLevel", m_SnowLevelValue);
				m_SnowLevelValue += Time.deltaTime * snowSpeed;
			}
		}
	}



	/// <summary>
	///		Toggles displaying the snowfall 
	/// </summary>
	/// <param name="ShouldEnableSnowfall"></param>
	private void ToggleSnow()
	{
		Debug.Log("Toggling Snow Enabled: " + m_SnowfallEnabled);

		if (currentSnowState.Equals(SnowStates.Active))
		{
			m_SnowfallEnabled = false;
		}
		else if (currentSnowState.Equals(SnowStates.InActive))
		{
			m_SnowfallEnabled = true;
		}

		Debug.Log("Snowfall Enabled? " + m_SnowfallEnabled);
	}

	#endregion

}
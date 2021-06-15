#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion


/// <summary>
///     Project Showcase Script for handling audio and UI events 
/// </summary>
public class Showcase : MonoBehaviour
{

	#region Public Variables

	[Range(0.01f, 0.1f)]
	public float snowSpeed;


	#endregion

	#region Private Variables

	private float m_SnowLevelValue = 0;
	private bool m_ShouldStartSnowing = false;

	#endregion

	#region Unity References

	private void Start()
	{
		m_ShouldStartSnowing = true;


		AudioManager.PlaySound(SoundEffect.BackgroundAmbience);
	}

	private void Update()
	{
		
		if (m_ShouldStartSnowing)
		{ 
			if (m_SnowLevelValue < 1f)
			{
				Shader.SetGlobalFloat("_SnowLevel", m_SnowLevelValue);
				m_SnowLevelValue += Time.deltaTime * snowSpeed;
			}
		}
	}

	#endregion

	#region Public Methods


	#endregion

	#region Private Methods


	#endregion

}

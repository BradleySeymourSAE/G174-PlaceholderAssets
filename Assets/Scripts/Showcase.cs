#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
#endregion


/// <summary>
///     Project Showcase Script for handling audio and UI events 
/// </summary>
public class Showcase : MonoBehaviour
{

	public enum SnowStates { Active, InActive };

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

	public static Showcase instance;
	public static UnityEvent ToggleSnowEvent = new UnityEvent();
	public static UnityEvent ToggleEnvironmentChange = new UnityEvent();
	public GameObject snowSystemPrefab;
	public GameObject firePrefab;


	[Range(0f, 1f)]
	public float elementScaleX = 0.75f;
	[Range(0f, 1f)]
	public float elementScaleY = 0.5f;

	[Range(-5f, 5f)]
	public float elementOffsetYPosition = 3f;
	
	[Range(0.01f, 0.1f)]
	public float snowSpeed = 0.05f;

	[SerializeField] 
	protected ParticleSystem[] m_SnowParticleSystems;
	[SerializeField] protected Terrain m_Terrain;
	[SerializeField] protected Light m_DirectionalLight;
	[SerializeField] protected Color m_EnvironmentColor;


	[SerializeField]
	protected Vector3 m_EnvironmentalOffsetPosition = Vector3.zero;

	[SerializeField] 
	private float m_SnowLevelValue = 0;
	
	[TagSelector] 
	[SerializeField] 
	protected string m_TagSelector;

	[SerializeField] 
	protected List<Transform> m_EnvironmentSpawnPositions = new List<Transform>();
	
	[SerializeField]
	protected List<GameObject> m_SpawnedEffects = new List<GameObject>();

	private bool m_SnowfallEnabled;
	private bool m_EnvironmentChanged;


	#region Unity References

	private void OnEnable()
	{
		if (instance != null && instance != this)
			Destroy(instance.gameObject);

		instance = this;

		m_SnowfallEnabled = false;
		m_EnvironmentChanged = false;
		m_EnvironmentalOffsetPosition.y -= elementOffsetYPosition;
		
		GameObject s_SnowfallPrefab = Instantiate(snowSystemPrefab, new Vector3(0, 40, 0), Quaternion.identity);

		for (var i = 0; i < s_SnowfallPrefab.GetComponentsInChildren<ParticleSystem>().Length; i++)
		{ 
			m_SnowParticleSystems = s_SnowfallPrefab.GetComponentsInChildren<ParticleSystem>();

			ParticleSystem s_ParticleSystem = m_SnowParticleSystems[i];

			if (s_ParticleSystem.isPlaying)
				s_ParticleSystem.Stop();
		}

		ToggleSnowEvent.AddListener(ToggleSnow);
		ToggleEnvironmentChange.AddListener(() =>
		{
			SetEnvironment();
		});
	}

	private void OnDisable()
	{
		ToggleSnowEvent.RemoveListener(ToggleSnow);
		ToggleEnvironmentChange.RemoveListener(() =>
		{
			SetEnvironment();
		});
	}

	private void Start()
	{
		foreach (var s_SpawnPosition in GameObject.FindGameObjectsWithTag(m_TagSelector))
		{
			m_EnvironmentSpawnPositions.Add(s_SpawnPosition.transform);
		}
		m_EnvironmentChanged = false;
	}

	private void Update()
	{
		UpdateSnowfallLevel();
		
	}

	private void UpdateSnowfallLevel()
	{ 
		if (!m_SnowfallEnabled)
		{
			if (m_SnowLevelValue <= 0) 
			{ 
				m_SnowLevelValue = 0;
			}
			else if (m_SnowLevelValue > 0 && m_SnowLevelValue <= 1f)
			{
				m_SnowLevelValue -= Time.deltaTime * snowSpeed;
			}
		}
		
		if (m_SnowfallEnabled)
		{
			if (m_SnowLevelValue <= 1f)
			{
				m_SnowLevelValue += Time.deltaTime * snowSpeed;
			}
		}

		Shader.SetGlobalFloat("_SnowLevel", m_SnowLevelValue);
	}


	private void SetEnvironment()
	{

		// If the environment has already been changed (Fire has been spawned in) 
		if (m_EnvironmentChanged && m_SpawnedEffects.Count > 0)
		{
			// Set environment to not be changed anymore 
			m_EnvironmentChanged = false;

			foreach (GameObject effect in m_SpawnedEffects) { Destroy(effect); }

			m_DirectionalLight.color = Color.white;
		}
		else if (!m_EnvironmentChanged)
		{
			m_EnvironmentChanged = true;

			if (m_EnvironmentSpawnPositions.Count > 0)
			{
				m_SpawnedEffects.Clear();

				for (int i = 0; i < m_EnvironmentSpawnPositions.Count; i++)
				{ 
					Vector3 s_SpawnPosition = m_EnvironmentSpawnPositions[Random.Range(1, m_EnvironmentSpawnPositions.Count / 2)].position;

					s_SpawnPosition += m_EnvironmentalOffsetPosition;

					GameObject s_SpawnedFire = Instantiate(firePrefab, s_SpawnPosition, Quaternion.identity);

					s_SpawnedFire.transform.localScale = new Vector3(elementScaleX, elementScaleY, elementScaleX);
					m_SpawnedEffects.Add(s_SpawnedFire);
				}

				m_DirectionalLight.color = m_EnvironmentColor;
			}
		}
	}


	/// <summary>
	///		Toggles displaying the snowfall 
	/// </summary>
	/// <param name="ShouldEnableSnowfall"></param>
	private void ToggleSnow()
	{

		if (currentSnowState.Equals(SnowStates.Active))
		{
			m_SnowfallEnabled = false;

			foreach (ParticleSystem s_SnowEffect in m_SnowParticleSystems)
			{
				if (s_SnowEffect.isPlaying)
					s_SnowEffect.Stop();
			}
		}
		else if (currentSnowState.Equals(SnowStates.InActive))
		{
			m_SnowfallEnabled = true;

			foreach (ParticleSystem s_SnowEffect in m_SnowParticleSystems)
			if (!s_SnowEffect.isPlaying)
			{
				s_SnowEffect.Play();
			}
		}

		Debug.Log("Toggling Snow Enabled: " + m_SnowfallEnabled);
	}

	#endregion

}
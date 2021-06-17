#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#endregion



public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public enum GameStates { StartMenu, Loading, Game, GameEnded };
	public GameStates currentGameState;


	public static UnityEvent StartGameEvent = new UnityEvent();
	public static UnityEvent RestartEvent = new UnityEvent();
	public static UnityEvent GameEndedEvent = new UnityEvent();
	
	public KeyCode quitGameKey;
	public KeyCode toggleSnowKey;



	private void OnEnable()
	{
		if (instance != null && instance != this)
		{
			Destroy(instance.gameObject);
		}

		instance = this;

		RestartEvent.AddListener(Reset);
		LoadingManager.LoadingEvent.AddListener(() => {  currentGameState = GameStates.Loading; });
		LoadingManager.MainScreenLoadingEvent.AddListener(() => {  currentGameState = GameStates.StartMenu; if (AudioManager.IsPlayingSound(SoundEffect.BackgroundMusic)) { return; } else AudioManager.PlaySound(SoundEffect.BackgroundMusic); });
		Showcase.ToggleSnowEvent.AddListener(() => { Debug.Log("Toggling snow event"); });
	}

	private void OnDisable()
	{
		RestartEvent.RemoveListener(Reset);
		LoadingManager.LoadingEvent.RemoveListener(() => { currentGameState = GameStates.Loading; });
		LoadingManager.MainScreenLoadingEvent.RemoveListener(() => { currentGameState = GameStates.StartMenu; if (AudioManager.IsPlayingSound(SoundEffect.BackgroundMusic)) { return; } else AudioManager.PlaySound(SoundEffect.BackgroundMusic); });
	}

	private void Start()
	{
		LoadingManager.BootstrapEvent?.Invoke();
	}


	private void Update()
	{

		if (Input.GetKeyDown(toggleSnowKey))
		{
			Showcase.ToggleSnowEvent?.Invoke();
		}



		if (Input.GetKeyDown(quitGameKey))
		{
			if (currentGameState.Equals(GameStates.Game) || currentGameState.Equals(GameStates.GameEnded))
			{
				GameEndedEvent?.Invoke();
				LoadingManager.ReturnToMainScreen?.Invoke();
			}
			else if (currentGameState.Equals(GameStates.StartMenu))
			{
				#if UNITY_STANDALONE
								Debug.Log("[MainMenu.QuitApplication]: " + "Quitting Application!");
								Application.Quit();
				#endif
				#if UNITY_EDITOR
								// Stop playing the scene 
								Debug.Log("[MainMenu.QuitApplication]: " + "Running in the Editor - Editor application has stopped playing!");
								UnityEditor.EditorApplication.isPlaying = false;
				#endif
			}
		}
	}

	private void Reset()
	{
		currentGameState = GameStates.Game;


		if (!LoadingManager.IsSceneLoading)
		{
			GameUIManager.instance.loadingScreen.DisplayScreen(false);
		}
	}
}
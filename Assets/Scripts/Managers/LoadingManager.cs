#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
#endregion




public class LoadingManager : MonoBehaviour
{
    public static UnityEvent BootstrapEvent = new UnityEvent();
    public static UnityEvent MainScreenLoadingEvent = new UnityEvent();
    public static UnityEvent CreditsScreenLoadingEvent = new UnityEvent();

    public static UnityEvent ReturnToMainScreen = new UnityEvent();
    public static UnityEvent LoadingEvent = new UnityEvent();
    public static LoadingManager instance;
    private static AsyncOperation m_CurrentSceneloading;
    private static Coroutine m_CurrentRoutine;



    public static bool IsSceneLoading
    {
        get
        {
            return m_CurrentSceneloading != null;
        }
    }


    private void OnEnable()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance.gameObject);
        }
        instance = this;

        BootstrapEvent.AddListener(StartBootStrapping);
        GameManager.StartGameEvent.AddListener(StartMainGame);
        ReturnToMainScreen.AddListener(ReturnToMainMenu);
    }

    private void OnDisable()
    {
        BootstrapEvent.RemoveListener(StartBootStrapping);
        GameManager.StartGameEvent.RemoveListener(StartMainGame);
        ReturnToMainScreen.RemoveListener(ReturnToMainMenu);
    }

    private void StartBootStrapping()
    {
        if (m_CurrentRoutine != null)
        {
            StopCoroutine(m_CurrentRoutine);
        }
        m_CurrentRoutine = StartCoroutine(BootStrapper());
    }

    private IEnumerator BootStrapper()
    {
        m_CurrentSceneloading = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        while (!m_CurrentSceneloading.isDone)
        {
            // Just wait
            yield return null;
        }
        m_CurrentSceneloading = null;

        // In theory our UI is now loaded in, so lets display the loading screen.
        LoadingEvent?.Invoke();
        yield return new WaitForSeconds(2f);
        m_CurrentSceneloading = null;

        MainScreenLoadingEvent?.Invoke();
        yield return null;
    }

    private void StartMainGame()
    {
        if (m_CurrentRoutine != null)
        {
            StopCoroutine(m_CurrentRoutine);
        }
        m_CurrentRoutine = StartCoroutine(LoadGame());
    }

    private void ReturnToMainMenu()
    {
        if (m_CurrentRoutine != null)
        {
            StopCoroutine(m_CurrentRoutine);
        }
        m_CurrentRoutine = StartCoroutine(UnloadGame());
    }

    private IEnumerator UnloadGame()
    {
        LoadingEvent?.Invoke();
        // Start Loading In the next scene

        m_CurrentSceneloading = SceneManager.UnloadSceneAsync(2);
        m_CurrentSceneloading.allowSceneActivation = false;
        // Lets just add some artificial loading
        yield return new WaitForSeconds(4f);
        while (m_CurrentSceneloading.progress < 0.89f)
        {
            // Just wait
            yield return null;
        }
        m_CurrentSceneloading.allowSceneActivation = true;
        m_CurrentSceneloading = null;
        // Call the show main screen event.

        MainScreenLoadingEvent?.Invoke();

        yield return null;
    }

    private IEnumerator LoadGame()
    {
        LoadingEvent?.Invoke();
        // Start Loading In the next scene

        m_CurrentSceneloading = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        m_CurrentSceneloading.allowSceneActivation = false;
        // Lets just add some artificial loading
        yield return new WaitForSeconds(4f);
        
        while (m_CurrentSceneloading.progress < 0.89f)
        {
            // Just wait
            yield return null;
        }

        m_CurrentSceneloading.allowSceneActivation = true;
        m_CurrentSceneloading = null;

        // Call the reset event to start the game fresh
        GameManager.RestartEvent?.Invoke();
        yield return null;
    }
}

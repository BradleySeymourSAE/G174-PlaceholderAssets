#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
#endregion




public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;
    public MainMenu mainMenuScreen;
    public CreditsScreen creditsMenuScreen;
    public LoadingScreen loadingScreen;
    private List<BaseUIScreen> m_AllUIScreens = new List<BaseUIScreen>();


    private void OnEnable()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance.gameObject);
        }
        instance = this;

     
        LoadingManager.MainScreenLoadingEvent.AddListener(() => EnableScreen(mainMenuScreen));
        LoadingManager.LoadingEvent.AddListener(() => EnableScreen(loadingScreen));
    }

    private void OnDisable()
    {
  
        LoadingManager.MainScreenLoadingEvent.RemoveListener(() => { EnableScreen(mainMenuScreen); });
        LoadingManager.LoadingEvent.RemoveListener(() => { EnableScreen(loadingScreen); });
    }

	private void Awake()
    {
        SetupGameUIScreens();
    }

    private void EnableScreen(BaseUIScreen screen)
    {
        HideAll();

        screen.DisplayScreen(true);
    }

    private void HideAll()
    {
        for (int i = 0; i < m_AllUIScreens.Count; i++)
        {
            m_AllUIScreens[i].DisplayScreen(false);
        }
    }

    private void SetupGameUIScreens()
    {
        AddAllGameUIScreens();
        for (int i = 0; i < m_AllUIScreens.Count; i++)
        {
            m_AllUIScreens[i].SetUpButtons();
        }
    }

    private void AddAllGameUIScreens()
    {
        m_AllUIScreens.Add(mainMenuScreen);
        m_AllUIScreens.Add(creditsMenuScreen);
        m_AllUIScreens.Add(loadingScreen);
    }
    
    public void DisplayCredits(bool ShouldDisplayCreditsScreen)
	{
        if (ShouldDisplayCreditsScreen)
            EnableScreen(creditsMenuScreen);
        else
            EnableScreen(mainMenuScreen);

        AudioManager.PlaySound(SoundEffect.GUI_MenuTransition);
	}
    

    [System.Serializable]
    public class LoadingScreen : BaseUIScreen
    {
        public TMP_Text loading;
        private Coroutine m_Routine;

        public override void DisplayScreen(bool Enable)
        {
            if (m_Routine != null)
            {
                GameUIManager.instance.StopCoroutine(m_Routine);
            }
            // Only animate the text if we are opening the screen
            if (Enable)
            {
                m_Routine = GameUIManager.instance.StartCoroutine(LoadingText());
            }
            base.DisplayScreen(Enable);
        }

        private IEnumerator LoadingText()
        {
            while (true)
            {
                loading.text = GameUI.LoadingScreen_Loading;
                yield return new WaitForSeconds(0.25f);
                loading.text = GameUI.LoadingScreen_Loading + ".";
                yield return new WaitForSeconds(0.25f);
                loading.text = GameUI.LoadingScreen_Loading + "..";
                yield return new WaitForSeconds(0.25f);
                loading.text = GameUI.LoadingScreen_Loading + "...";
                yield return new WaitForSeconds(0.25f);
                loading.text = GameUI.LoadingScreen_Loading + "..";
                yield return new WaitForSeconds(0.25f);
                loading.text = GameUI.LoadingScreen_Loading + ".";
                yield return new WaitForSeconds(0.25f);
            }
        }
    }

   
    [System.Serializable]
    public class MainMenu : BaseUIScreen
    {
        public Button StartGameButton;
        public Button CreditsMenuButton;
        public Button QuitButton;

        public override void SetUpButtons()
        {
            UI.SetupButton(StartGameButton, () => GameManager.StartGameEvent?.Invoke(), GameUI.MainMenu_StartButton);
            UI.SetupButton(CreditsMenuButton, () => GameUIManager.instance.DisplayCredits(true), GameUI.MainMenu_CreditsButton);
            UI.SetupButton(QuitButton, () => Application.Quit(), GameUI.MainMenu_QuitButton);
        }
    }


    /// <summary>
    ///     Credits Menu UI 
    /// </summary>
    [System.Serializable]
    public class CreditsScreen : BaseUIScreen
	{
        public TMP_Text creditsTitle;
        public TMP_Text creditsTagline;
        public Button ReturnButton;


		public override void SetUpButtons()
		{
			UI.SetupButton(ReturnButton, () => GameUIManager.instance.DisplayCredits(false), GameUI.CreditsMenu_ReturnButton);
		}
	}



    [System.Serializable]
    public class BaseUIScreen
    {
        public GameObject Screen;

        public virtual void DisplayScreen(bool ShouldDisplayScreen)
        {
            Screen.SetActive(ShouldDisplayScreen);
        }

        public virtual void SetUpButtons()
        {

        }
    }

    public class GameUI
    {
        public const string MainMenu_GameTitle = "Lucid Dream - Alpine Experience";
        public const string MainMenu_Tagline = "G174 - Creating placeholder assets";
        public const string MainMenu_GameVersion = "Development v1.0.0";
        public const string MainMenu_StartButton = "Play Game";
        public const string MainMenu_CreditsButton = "Credits";
        public const string MainMenu_QuitButton = "Leave";

        public const string CreditsMenu_Title = "Lucid Alpine Experience - Credits";
		public const string CreditsMenu_Subtitle = "G174 Game Asset Creation \n\n" + "Created by Bradley Seymour 21T2";
        public const string CreditsMenu_ReturnButton = "Return";


        public const string LoadingScreen_Loading = "Loading";
    }

}

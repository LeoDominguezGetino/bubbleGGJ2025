using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Localization.Settings;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using Unity.VisualScripting;


public class GameManager : MonoBehaviour
{
    AudioSource m_AudioSource;
    public AudioSource pirates_AudioSource;
    public AudioSource tiny_AudioSource;
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    public static GameManager Instance;

    public List<GameObject> Players;

    public CinemachineTargetGroup cameraMultiTarget;
    public CinemachineCamera menuCamera;
    public CinemachineCamera inGameCamera;
    public CinemachineCamera interactableCamera;

    public bool gameOver;
        bool gameStarted = false;

    public Sprite[] playerBubbleSprites;

    public bool victory;
    
    bool selectingLocale = false;

    public List<BubbleDeviceSelector> deviceSelectors;
    [SerializeField] BubbleDeviceSelector bubbleDevicePrefab;
    [SerializeField] Transform bubbleDeviceParent;

    [SerializeField] GameObject mainMenuScreen;
    [SerializeField] GameObject gameOptionsScreen;
    [SerializeField] GameObject languageScreen;
    [SerializeField] GameObject successScreen;
    [SerializeField] GameObject pauseScreen;
    public DeathPanel deathScreen;

    private void Start()
    {
        Instance = this;
        gameOver = false;

        m_AudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (gameOver)
        {
            foreach (GameObject go in Players)
            {
                go.GetComponent<BubbleMovement>().Respawn();
                gameOver = false;
            }
        }

        //if (Players[0].GetComponent<BubbleMovement>().air < 0.5f) {
        //    pirates_AudioSource.volume = 0;
        //    tiny_AudioSource.volume = 1;
        //} else
        //{
        //    pirates_AudioSource.volume = 1;
        //    tiny_AudioSource.volume = 0;
        //}

        
    }

    public void RefreshDevices()
    {
        foreach (BubbleDeviceSelector bubble in  deviceSelectors)
        {
            Destroy(bubble.gameObject);
        }

        deviceSelectors.Clear();

        foreach (var device in InputSystem.devices)
        {
            if (device is Keyboard)
            {
                deviceSelectors.Add(Instantiate(bubbleDevicePrefab, bubbleDeviceParent));
                deviceSelectors[deviceSelectors.Count-1].controlScheme = "WASD";
                deviceSelectors[deviceSelectors.Count-1].device = device;

                deviceSelectors.Add(Instantiate(bubbleDevicePrefab, bubbleDeviceParent));
                deviceSelectors[deviceSelectors.Count - 1].controlScheme = "Arrows";
                deviceSelectors[deviceSelectors.Count - 1].device = device;
            }
            else if (device is Gamepad)
            {
                deviceSelectors.Add(Instantiate(bubbleDevicePrefab, bubbleDeviceParent));
                deviceSelectors[deviceSelectors.Count - 1].controlScheme = "Gamepad";
                deviceSelectors[deviceSelectors.Count - 1].device = device;
            }
            else { continue; }
        }
    }

    private void OnEnable()
    {
        GetComponent<PlayerInputManager>().onPlayerJoined += OnPlayerJoined;
    }


    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Players.Add(playerInput.gameObject);
        cameraMultiTarget.Targets.Add(new CinemachineTargetGroup.Target());
        cameraMultiTarget.Targets[cameraMultiTarget.Targets.Count - 1].Object = playerInput.gameObject.transform;
        playerInput.gameObject.GetComponent<BubbleMovement>().ApplyAppearance(Players.Count-1);
    }

    public void StartGame()
    {
        if (gameStarted) return;

        gameStarted = true;

        gameOptionsScreen.SetActive(false);
        languageScreen.SetActive(false);
        mainMenuScreen.SetActive(false);
        successScreen.SetActive(false);

        SceneManager.LoadSceneAsync("Level1", LoadSceneMode.Additive);

        StartCoroutine(JoinPlayers());
        menuCamera.Priority = -1;

        m_AudioSource.Stop();
        m_AudioSource.clip = gameMusic;
        m_AudioSource.Play();
        pirates_AudioSource.Play();
        tiny_AudioSource.Play();
    }

    private IEnumerator JoinPlayers()
    {
        foreach (BubbleDeviceSelector bubble in deviceSelectors)
        {
            PlayerInput.Instantiate(GetComponent<PlayerInputManager>().playerPrefab, controlScheme: bubble.controlScheme, pairWithDevice: bubble.device);
            yield return new WaitForSeconds(1);
        }        
        //GetComponent<PlayerInputManager>().EnableJoining();
    }

    public void LevelCleared()
    {
        foreach (var player in Players)
        {
            Destroy(player);            
        }
        Players.Clear();
        cameraMultiTarget.Targets.Clear();

        menuCamera.Priority = 1;
        successScreen.SetActive(true);

        SceneManager.UnloadSceneAsync("Level1");
        gameStarted = false;
    }

    public void Replay()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator SetLocale(int _localeID)
    {
        selectingLocale = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
        selectingLocale = false;
    }

    public void ChangeLocale(int localeID)
    {
        if (selectingLocale) { return; }
        StartCoroutine(SetLocale(localeID));
    }

    public void PauseGame()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void ToMenu()
    {
        pauseScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
        Time.timeScale = 1;

        StartCoroutine(Unload());

        menuCamera.Priority = 1;        
        gameStarted = false;
    }

    IEnumerator Unload()
    {
        yield return new WaitForSeconds(2f);

        foreach (var player in Players)
        {
            Destroy(player);
        }
        Players.Clear();
        cameraMultiTarget.Targets.Clear();

        SceneManager.UnloadSceneAsync("Level1");
    }
}

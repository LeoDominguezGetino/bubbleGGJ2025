using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;
using System.Collections;

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

    public bool gameOver;

    public Sprite[] playerBubbleSprites;

    public bool victory;

    public Animator successScreenAnimator;

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

        if (Players[0].GetComponent<BubbleMovement>().air < 0.5f) {
            pirates_AudioSource.volume = 0;
            tiny_AudioSource.volume = 1;
        } else
        {
            pirates_AudioSource.volume = 1;
            tiny_AudioSource.volume = 0;
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
        PlayerInput.Instantiate(GetComponent<PlayerInputManager>().playerPrefab, controlScheme: "WASD", pairWithDevice: Keyboard.current);
        yield return new WaitForSeconds(1);
        PlayerInput.Instantiate(GetComponent<PlayerInputManager>().playerPrefab, controlScheme: "Arrows", pairWithDevice: Keyboard.current);    
    }

    public void Success()
    {
        foreach (var player in Players)
        {
            player.gameObject.SetActive(false);
        }

        menuCamera.Priority = 1;
        successScreenAnimator.Play("",0);
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

}

using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<GameObject> Players;

    public CinemachineTargetGroup cameraMultiTarget;
    public CinemachineCamera menuCamera;
    public CinemachineCamera inGameCamera;

    public bool gameOver;

    public Sprite[] playerBubbleSprites;

    public bool victory;

    public Animator successScreenAnimator;

    public Vector2 playerStart;
    bool playersJoined = false;

    private void Start()
    {
        Instance = this;
        gameOver = false;
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
        if (!playersJoined) { StartCoroutine(JoinPlayers()); }
        else { StartCoroutine(SpawnPlayers()); }
        

        menuCamera.Priority = -1;
    }

    private IEnumerator JoinPlayers()
    {
        PlayerInput.Instantiate(GetComponent<PlayerInputManager>().playerPrefab, controlScheme: "WASD", pairWithDevice: Keyboard.current);
        yield return new WaitForSeconds(1);
        PlayerInput.Instantiate(GetComponent<PlayerInputManager>().playerPrefab, controlScheme: "Arrows", pairWithDevice: Keyboard.current);    
    }

    private IEnumerator SpawnPlayers()
    {
        foreach (var player in Players)
        {
            player.transform.position = playerStart;
            yield return new WaitForSeconds(1);
            player.gameObject.SetActive(true);
        }
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

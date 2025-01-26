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
        StartCoroutine(SpawnPlayers());

        menuCamera.Priority = -1;
    }

    private IEnumerator SpawnPlayers()
    {
        PlayerInput.Instantiate(GetComponent<PlayerInputManager>().playerPrefab, controlScheme: "WASD", pairWithDevice: Keyboard.current);
        yield return new WaitForSeconds(1);
        PlayerInput.Instantiate(GetComponent<PlayerInputManager>().playerPrefab, controlScheme: "Arrows", pairWithDevice: Keyboard.current);    
    }

}

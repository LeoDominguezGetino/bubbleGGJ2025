using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<GameObject> Players;

    public CinemachineTargetGroup cameraMultiTarget;
    public CinemachineCamera menuCamera;
    public CinemachineCamera inGameCamera;

    public bool gameOver = false;

    public Sprite[] playerBubbleSprites;

    public bool victory;
    private void Start()
    {
        Instance = this;
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
        menuCamera.Priority = -1;
    }
}

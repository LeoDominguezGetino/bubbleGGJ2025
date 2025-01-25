using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<GameObject> Players;

    public CinemachineTargetGroup cameraMultiTarget;

    public bool gameOver;

    public bool victory;

    private void Awake()
    {
        Instance = this;
        gameOver = false;
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
    }
}

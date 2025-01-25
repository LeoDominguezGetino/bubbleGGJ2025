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

    
    public bool inLanguageSelection;
    public bool inMenu = false;
    public bool gameOver = false;

<<<<<<< Updated upstream
    public bool victory;

    private void Awake()
=======
    private void Start()
>>>>>>> Stashed changes
    {
        Instance = this;
        inLanguageSelection = true;
    }

    private void Update()
    {
        if (inLanguageSelection)
        {

        }

        if (inMenu)
        {

        }

        if (gameOver)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //PlayerInputManager.JoinPlayer(InputDevice[]);
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
    }
}

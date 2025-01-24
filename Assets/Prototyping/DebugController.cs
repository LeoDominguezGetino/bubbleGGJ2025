using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class DebugController : MonoBehaviour
{
    bool showConsole;
    bool showHelp;
    string input;
    Vector2 scroll;
    InputSystem_Actions inputSystem;

    public static DebugCommand HELP;
    public static DebugCommand<string> CURSOR;
    public static DebugCommand<string> TP;
    public List<object> commandList;

    // Custom commands
    public static DebugCommand<int> SETAIR;

    private bool wasCursorLocked;

    [SerializeField] private List<TeleportPoint> teleportPoints = new List<TeleportPoint>();
    private Dictionary<string, Vector3> teleportDictionary;

    private void Awake()
    {
        inputSystem = new InputSystem_Actions();

        teleportDictionary = new Dictionary<string, Vector3>();
        foreach (var teleportPoint in teleportPoints)
        {
            if (!teleportDictionary.ContainsKey(teleportPoint.id))
            {
                teleportDictionary.Add(teleportPoint.id, teleportPoint.position);
            }
            else
            {
                Debug.LogWarning($"Duplicate ID found: {teleportPoint.id}, ignoring...");
            }
        }
        string teleportHelp = "-";
        if (teleportDictionary.Count > 0)
        {
            int i = 0;
            foreach (var id in teleportDictionary.Keys)
            {
                if (i == 0) { teleportHelp = id; }
                else { teleportHelp += ", " + id; }
                i++;
            }
        }


        HELP = new DebugCommand("help", "Shows a list of commands", "help", () => showHelp = !showHelp);
        CURSOR = new DebugCommand<string>("cursor", "Edits CursorLockMode (none, confined, locked)", "cursor <lockmode>", (x) => { CursorMode(x); });
        TP = new DebugCommand<string>("tp", "Teleports the player at a specified POI (" + teleportHelp + ")", "tp <poi>", (x) => { Teleport(x); });
        //SETAIR = new DebugCommand<int>("setair",);

        // Custom Commands

        commandList = new List<object>
        {
            HELP,
            CURSOR,
            TP,
            SETAIR
        };      

        Debug.Log("DebugConsole active! Yeah! Press . to toggle the console and type help for... Help... duh");
    }

    public void OnToggleDebug(InputAction.CallbackContext inputValue) { ToggleDebug(); }

    void ToggleDebug()
    {
        input = "";
        showConsole = !showConsole;
        //GameManager.Instance.Player.GetComponent<CharacterMovement>().enabled = !showConsole;
        //GameManager.Instance.Player.GetComponent<Interactor>().enabled = !showConsole;

        if (showConsole)
        {
            wasCursorLocked = Cursor.lockState == CursorLockMode.Locked;
            Cursor.lockState = CursorLockMode.None;
            GUI.FocusControl("DebugInputField");
        }
        else { if (wasCursorLocked) { Cursor.lockState = CursorLockMode.Locked; } }
    }
    private void OnGUI()
    {
        if (!showConsole) { return; }

        float y = 0f;

        if (showHelp)
        {
            GUI.Box(new Rect(0, y, Screen.width, 100), "");

            Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);
            scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);

            for (int i = 0; i < commandList.Count; i++)
            {
                DebugCommandBase command = commandList[i] as DebugCommandBase;
                string label = $"{command.commandFormat} - {command.commandDescription}";
                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);

                GUI.Label(labelRect, label);
            }

            GUI.EndScrollView();

            y += 100;
        }

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);

        // Nome univoco per il campo di input
        GUI.SetNextControlName("DebugInputField");

        // Casella di input
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);

        // Imposta il focus se necessario
        if (Event.current.type == EventType.Repaint && showConsole && string.IsNullOrEmpty(input))
        {
            GUI.FocusControl("DebugInputField");
        }
    }
    private void OnEnable()
    {
        inputSystem.Enable();
        inputSystem.Player.DebugConsole.performed += OnToggleDebug;
        inputSystem.Player.Return.performed += OnReturn;
    }
    private void OnDisable()
    {
        inputSystem.Player.DebugConsole.performed -= OnToggleDebug;
        inputSystem.Player.Return.performed -= OnReturn;
        inputSystem.Disable();
    }
    public void OnReturn(InputAction.CallbackContext inputValue)
    {
        if (showConsole)
        {
            HandleInput();
            input = "";
        }
    }
    private void HandleInput()
    {
        string[] properties = input.Split(' ');

        for( int i=0; i<commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;
            if (input.Contains(commandBase.commandId))
            {
                if (commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }
                else if (commandList[i] as DebugCommand<int> != null)
                {
                    (commandList[i] as DebugCommand<int>).Invoke(int.Parse(properties[1]));
                }
                else if (commandList[i] as DebugCommand<string> != null)
                {
                    (commandList[i] as DebugCommand<string>).Invoke((properties[1]));
                }
            }
        }
    }

    void CursorMode (string mode)
    {
        if (mode == "locked") { Cursor.lockState = CursorLockMode.Locked; }
        else if (mode == "confined") { Cursor.lockState = CursorLockMode.Confined; }
        else if (mode == "none") { Cursor.lockState = CursorLockMode.None; }
        wasCursorLocked = false;
        ToggleDebug();
    }

    [Serializable] public class TeleportPoint
    {
        public string id;           // Identificativo unico
        public Vector3 position;    // Coordinate del punto di teletrasporto
    }
    private void OnDrawGizmos()
    {
        // Colore dei Gizmo
        Gizmos.color = Color.cyan;

        foreach (var teleportPoint in teleportPoints)
        {
            // Disegna una sfera alle coordinate specificate
            Gizmos.DrawSphere(teleportPoint.position, 0.5f);

            // Aggiungi un'etichetta con l'ID
            UnityEditor.Handles.Label(teleportPoint.position, teleportPoint.id);
        }
    }
    public void Teleport(string location)
    {
        if (teleportDictionary.TryGetValue(location, out var targetPoint))
        {
            GameManager.Instance.Player.GetComponent<CharacterController>().enabled = false;
            GameManager.Instance.Player.transform.position = targetPoint;

            Debug.Log($"Teleported to {location}");

            GameManager.Instance.Player.GetComponent<CharacterController>().enabled = true;

            ToggleDebug();
        }
        else
        {
            Debug.LogError($"Teleport ID '{location}' not found!");
        }
    }
}
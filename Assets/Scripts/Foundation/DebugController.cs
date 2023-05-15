using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    bool showConsole;
    string input;

    public static DebugCommand<float> SET_PLAYER_SPEED;
    public static DebugCommand RESPAWN;
    public static DebugCommand SWITCH_CHARACTER;
    public static DebugCommand<float> SET_PENTE;
    public static DebugCommand CALIBRATEGYRO;
    public List<object> commandList;
    // Start is called before the first frame update
    void Awake()
    {
        SET_PLAYER_SPEED = new DebugCommand<float>("/set_player_speed", "Set the character speed to x value", "/set_player_speed <value>", (x) =>
        {
             GameManager.Instance.player.charaSpeed = x;
        });

        RESPAWN = new DebugCommand("/reset", "Make player respawn at the spawn position", "/reset", () =>
        {
            GameManager.Instance.SpawnPlayer();
        });

        SWITCH_CHARACTER = new DebugCommand("/switch_character", "Change from human to tractor or the opposite", "/switch_character", () =>
        {
            if (GameManager.Instance.player.isCharacterControlled)
            {
                StartCoroutine(GameManager.Instance.player.SwitchControls("Tractor",false));
            }
            else
            {
                StartCoroutine(GameManager.Instance.player.SwitchControls("Character",false));
            }
        });

        SET_PENTE = new DebugCommand<float>("/set_pente", "Change la pente du velo", "/set_pente <value>", (x) =>
         {
             GameManager.Instance.velo.ChangePente((int)x);
         });

        CALIBRATEGYRO = new DebugCommand("/calibrate", "Calibrate the gyroscope", "/calibrate", () =>
         {
             ArduinoConnector.Instance.Calibrate();
             Debug.Log("truing to calibrate");
         });


        commandList = new List<object>
        {
            SET_PLAYER_SPEED,
            RESPAWN,
            SWITCH_CHARACTER,
            SET_PENTE,
            CALIBRATEGYRO
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Quote))
        {
            showConsole = !showConsole;
        }
        /*if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Trying to enter data");
            if (showConsole)
            {
                HandleInput();
                input = "";
            }
        }*/
    }

    private void OnGUI()
    {
        if (!showConsole) { return; }

        float y = 0f;

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 50f), input);
        if (Event.current.type == EventType.KeyDown && Event.current.character.ToString() == "\n")
        {
            HandleInput();
            input = "";
        }
    }
    
    void HandleInput()
    {
        string[] properties = input.Split(' ');

        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if (input.Contains(commandBase.commandId))
            {
                if(commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }else if(commandList[i] as DebugCommand<float> != null)
                {
                    (commandList[i] as DebugCommand<float>).Invoke(float.Parse(properties[1]));
                }
            }
        }
    }
}

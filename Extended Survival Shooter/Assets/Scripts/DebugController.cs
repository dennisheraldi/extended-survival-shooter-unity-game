using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    bool showConsole;
    bool showHelp;

    string input;

    public static DebugCommand ROSE_GOLD;
    public static DebugCommand IMMUNE;
    public static DebugCommand KILLER;
    public static DebugCommand HELP;

    public List<object> commandList;

    public void OnToggleDebug(InputValue value)
    {
        showConsole = !showConsole;
    }

    public void OnReturn(InputValue value)
    {
        if (showConsole)
        {
            HandleInput();
            input = "";
        }
    }

    private void Awake()
    {
        ROSE_GOLD = new DebugCommand("rose_gold", "Gives you 1000 gold", "rose_gold", () => {
            MainManager.Instance.currentMoney += 1000;
        });
        IMMUNE = new DebugCommand("immune", "Makes you immune to damage", "immune", () => {
            MainManager.Instance.immunity = true;
        });
        KILLER = new DebugCommand("killer", "Makes you kill enemies in one hit", "killer", () => {
            MainManager.Instance.instantKill = true;
        });
        HELP = new DebugCommand("help", "Shows all available commands", "help", () => {
            showHelp = true;
        });

        commandList = new List<object>{
            ROSE_GOLD,
            IMMUNE,
            KILLER,
            HELP
        };
    }

    Vector2 scroll;

    private void OnGUI()
    {
        if (!showConsole){
            return;
        }
        
        float y = 0f;

        if (showHelp)
        {
            GUI.Box(new Rect(0, y, Screen.width, 100), "");
            
            Rect viewPort = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);

            scroll = GUI.BeginScrollView(new Rect(0, y = 5f, Screen.width, 90), scroll, viewPort);

            for (int i = 0; i < commandList.Count; i++)
            {
                DebugCommandBase command = commandList[i] as DebugCommandBase;
                string label = $"{command.CommandFormat} - {command.CommandDescription}";

                Rect labelRect = new Rect(5, 20 * i, viewPort.width - 100, 20);
                GUI.Label(labelRect, label);

            }

            GUI.EndScrollView();

            y += 100;

        }

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);

        
    }

    private void HandleInput(){
        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;
            if (commandBase.CommandId == input)
            {
                if (commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }
            }
        }

    }

}

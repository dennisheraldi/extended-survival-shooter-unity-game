using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    bool showConsole;
    bool showHelp;

    string input;

    private bool displayAlert = false;
    private float alertStartTime;

    public static DebugCommand ROSE_GOLD;
    public static DebugCommand IMMUNE;
    public static DebugCommand KILLER;
    public static DebugCommand HELP;
    public static DebugCommand KILLPET;
    public static DebugCommand USAIN_BOLT;
    public static DebugCommand IMMORTAL;

    public List<object> commandList;

    public void OnToggleDebug(InputValue value)
    {
        showConsole = !showConsole;
        input = "";
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
            MainManager.Instance.currentMoney = 999999999;
            MainManager.Instance.infMoney = true;
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
        KILLPET = new DebugCommand("kill_pet", "Kills your pet", "kill_pet", () => {
            switch (MainManager.Instance.currentPet)
            {
                case "Healer":
                    PetHealerHealth healerHealth = GameObject.FindGameObjectWithTag("Healer").GetComponent<PetHealerHealth>();
                    healerHealth.TakeDamage(healerHealth.currentHealth);
                    MainManager.Instance.currentPet = "";
                    break;
                case "Attacker":
                    PetHealth attackerHealth = GameObject.FindGameObjectWithTag("Pet").GetComponent<PetHealth>();
                    attackerHealth.TakeDamage(attackerHealth.currentHealth);
                    MainManager.Instance.currentPet = "";
                    break;
                case "AuraBuff":
                    PetBuffHealth buffHealth = GameObject.FindGameObjectWithTag("Buff").GetComponent<PetBuffHealth>();
                    buffHealth.TakeDamage(buffHealth.currentHealth);
                    MainManager.Instance.currentPet = "";
                    break;
                default:
                    break;
            }
        });
        USAIN_BOLT = new DebugCommand("usain_bolt", "Makes you run faster", "usain_bolt", () => {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            playerMovement.speed = 12;
        });
        IMMORTAL = new DebugCommand("immortal", "Makes your pet immortal", "immortal", () => {
            MainManager.Instance.pet_immune = true;
        });

        commandList = new List<object>{
            ROSE_GOLD,
            IMMUNE,
            KILLER,
            HELP,
            KILLPET,
            USAIN_BOLT,
            IMMORTAL
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
        GUI.SetNextControlName("DebugInput");
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
        GUI.FocusControl("DebugInput");

        if (displayAlert)
        {
            float timeElapsed = Time.time - alertStartTime;
            if (timeElapsed < 2f)
            {
                GUI.Label(new Rect(10f, y + 30f, Screen.width - 20f, 20f), "Cheat activated");
            }
            else
            {
                displayAlert = false;
            }
        }

        
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
                    if (commandBase.CommandId != "help")
                    {
                        displayAlert = true;
                        alertStartTime = Time.time;
                    }
                }
            }
        }

    }

}

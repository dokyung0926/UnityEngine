using System;
using System.Collections.Generic;
using UnityEngine;
public class PlayerStateMachineManager : MonoBehaviour
{
    PlayerStateMachine[] playerStateMachines;
    PlayerStateMachine currentMachine;
    KeyCode keyInput;

    private void Awake()
    {
        playerStateMachines = GetComponents<PlayerStateMachine>();
    }

    private void Update()
    {
        CompareKeyInput();
        UpdateMachineState();
    }

    private void CompareKeyInput()
    {
        foreach (var machine in playerStateMachines)
        {
            if (keyInput == machine.keyCode)
            {
                if (machine.IsExecuteOK())
                    machine.Execute();
                keyInput = KeyCode.None;
                break;
            }
        }
    }

    private void UpdateMachineState()
    {
        if(currentMachine != null)
            currentMachine.UpdateState();
    }

    private void OnGUI()
    {
        Event e = Event.current;
        if(e.isKey && e.keyCode != KeyCode.None)
        {
            keyInput = e.keyCode;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static GameState gameState;

    public static void Play()
    {
        gameState = GameState.StatGame;
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    private void Update()
    {
        switch (gameState)
        {
            case GameState.Idle:
                break;
            case GameState.StatGame:
                break;
            case GameState.SpawnGap:
                break;
            case GameState.WaitForMapSpawned:
                break;
            case GameState.StartRun:
                break;
            case GameState.WaitForGameFinished:
                break;
            case GameState.Finish:
                break;
            default:
                break;
        }
    }
}
public enum GameState
{
    Idle,
    StatGame,
    SpawnGap,
    WaitForMapSpawned,
    StartRun,
    WaitForGameFinished,
    Finish
}

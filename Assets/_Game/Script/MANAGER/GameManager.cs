using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MainMenu,
    OnUI,
    GamePlay,
    OnCombat
}

public class GameManager : Singleton<GameManager>
{


    public GameState currentGameState;
    void Start()
    {
        currentGameState = GameState.GamePlay;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

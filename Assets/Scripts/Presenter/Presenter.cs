using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;

public class Presenter : MonoBehaviour
{
    private GameModel gameModel;
    [SerializeField] private View view;

    public delegate void GameOverHandler(bool? player, int? points);
    public event GameOverHandler GameOver;

    private void Awake()
    {
        gameModel = new GameModel();
        gameModel.GameOver += GameResults;
    }

    public void GetRecords()
    {
        view.GetRecords(gameModel.GetRecords());
    }

    public string RandomWord()
    {
        return gameModel.RandomWord();
    }

    public string StartGame(string startWord, bool isTimeControlEnabled, int timer, int addTime)
    {
        return gameModel.StartGame(startWord, isTimeControlEnabled, timer, addTime);
    }

    public string GetBoardSize()
    {
        return gameModel.GetBoardSize();
    }

    public (int, int) GetPoints()
    {
        return gameModel.GetPoints();
    }

    public (string, string) GetTime()
    {
        return gameModel.GetTime();
    }

    public bool GetTurn()
    {
        return gameModel.GetTurn();
    }

    public bool TimeControlEnable()
    {
        return gameModel.TimeControlEnable();
    }

    public string MakeMove(string word, string letter, (int, int) cell)
    {
        return gameModel.MakeMove(word, letter, cell);
    }

    public void SkipTurn()
    {
        gameModel.SkipTurn();
    }

    private void GameResults(bool? player, int? points)
    {
        GameOver.Invoke(player, points);
    }
}

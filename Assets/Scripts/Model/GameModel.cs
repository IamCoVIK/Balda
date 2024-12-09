using System.Collections;
using System.Collections.Generic;

public class GameModel
{
    private Records records;
    private Settings settings;
    private GameLogic gameLogic;

    public delegate void GameOverHandler(bool? player, int? points);
    public event GameOverHandler GameOver;

    public GameModel()
    {
        records = new Records();
        settings = new Settings();
    }

    public (string, string)[] GetRecords()
    {
        return records.GetRecords();
    }

    public string RandomWord()
    {
        return settings.RandomWord();
    }

    public string StartGame(string startWord, bool isTimeControlEnabled, int timer, int addTime)
    {
        string error = settings.CheckStartConditions(startWord, isTimeControlEnabled, timer, addTime);
        if (error == string.Empty)
        {
            gameLogic = new GameLogic(settings);
            gameLogic.GameOver += GameResults;
        }
        return error;
    }

    public string GetBoardSize()
    {
        return gameLogic.GetBoardSize();
    }

    public (int, int) GetPoints()
    {
        return gameLogic.GetPoints();
    }

    public (string, string) GetTime()
    {
        return gameLogic.GetTime();
    }

    public bool GetTurn()
    {
        return gameLogic.GetTurn();
    }

    public bool TimeControlEnable()
    {
        return gameLogic.TimeControlEnable();
    }

    public string MakeMove(string word, string letter, (int, int) cell) 
    {
        return gameLogic.MakeMove(word, letter, cell);
    }

    public void SkipTurn()
    {
        gameLogic.SkipTurn();
    }
    private void GameResults(bool? player, int? points)
    {
        GameOver.Invoke(player, points);
    }
}

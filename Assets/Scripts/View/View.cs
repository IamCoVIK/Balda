using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class View : MonoBehaviour
{
    [SerializeField] private Presenter presenter;
    [Space]
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private NewGame newGame;
    //[SerializeField] private LoadGame loadGame;
    [SerializeField] private Game game;
    //[SerializeField] private Records records;
    //[SerializeField] private SaveGame saveGame;

    public UnityEvent MainMenuLoaded;
    public UnityEvent NewGameLoaded;
    public UnityEvent GameLoaded;

    private void Awake()
    {
        presenter.GameOver += GameResults;
        mainMenu.gameObject.SetActive(false);
        newGame.gameObject.SetActive(false);
        game.gameObject.SetActive(false);
    }

    private void Start()
    {
        mainMenu.gameObject.SetActive(true);
        MainMenuLoaded.Invoke();
    }

    //Main Menu
    public void GetRecords((string, string)[] records)
    {
        mainMenu.GetRecords(records);
    }

    public void NewGame()
    {
        mainMenu.gameObject.SetActive(false);
        newGame.gameObject.SetActive(true);
        NewGameLoaded.Invoke();
    }

    public void LoadGame()
    {
        mainMenu.gameObject.SetActive(false);
        //loadGame.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    //New Game
    public string RandomWord()
    {
        return presenter.RandomWord();
    }

    public void Back()
    {
        newGame.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }

    public string StartGame(string startWord, bool isTimeControlEnabled, int timer, int addTime)
    {
        string error = presenter.StartGame(startWord, isTimeControlEnabled, timer, addTime);
        if (error == string.Empty)
        {
            newGame.gameObject.SetActive(false);
            game.gameObject.SetActive(true);
            GameLoaded?.Invoke();
        }
        return error;
    }

    //Game
    public string GetBoardSize()
    {
        return presenter.GetBoardSize();
    }

    public (int, int) GetPoints()
    {
        return presenter.GetPoints();
    }

    public (string, string) GetTime()
    {
        return presenter.GetTime();
    }

    public bool GetTurn()
    {
        return presenter.GetTurn();
    }

    public bool TimeControlEnable()
    {
        return presenter.TimeControlEnable();
    }

    public string MakeMove(string word, string letter, (int, int) cell)
    {
        return presenter.MakeMove(word, letter, cell);
    }

    public void SkipTurn()
    {
        presenter.SkipTurn();
    }

    private void GameResults(bool? player, int? points)
    {
        game.GameResults(player, points);
    }

    public void ToMenu()
    {
        game.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }

    public void ToRecords(int points)
    {
        Debug.Log(points);
    }
}

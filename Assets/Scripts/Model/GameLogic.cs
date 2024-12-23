using Codice.CM.Client.Differences;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameLogic
{
    private Settings settings;

    private char[,] board;
    private int size;

    private Player player1;
    private Player player2;
  
    private int addTime;
    private bool turn; //true - 1, false - 2

    public delegate void GameOverHandler(bool? player, int? points);
    public event GameOverHandler GameOver;

    public GameLogic(Settings settings)
    {
        this.settings = settings;

        size = settings.startWord.Length;
        board = new char[size, size];
        AddStartWord(settings.startWord);

        player1 = new Player(settings.timer);
        player2 = new Player(settings.timer);

        addTime = settings.addTime;

        turn = true;
    }

    private void AddStartWord(string startWord)
    {
        int y = startWord.Length / (int)2;
        for (int x = 0; x < size; x++)
        {
            board[x, y] = startWord[x];
        }
    }

    public (int, int) GetPoints()
    {
        return (player1.points, player2.points);
    }

    public (string, string) GetTime()
    {
        if (turn)
        {
            player1.TimeMove();
            if (player1.time <= 0)
            {
                GameOver.Invoke(!turn, player2.points);
            }
        }
        else
        {
            player2.TimeMove();
            if (player2.time <= 0)
            {
                GameOver.Invoke(!turn, player1.points);
            }
        }
        return (TimeToString(player1.points), TimeToString(player2.points));
    }

    private string TimeToString(int time)
    {
        int minute = 60;
        int m = time / minute;
        int s = time - m * minute;
        if (s >= 10)
        {
            return $"{m}:{s}";
        }
        else
        {
            return $"{m}:0{s}";
        }
        
    }

    public bool GetTurn() { return turn; }

    public bool TimeControlEnable()
    {
        return settings.isTimeControlEnabled;
    }

    private void ChangeTurn(bool skipped)
    {
        IsGameOver();
        if (!skipped)
        {
            AddTime();
        }
        turn = !turn;
    }

    public string GetBoardSize()
    {
        return settings.startWord;
    }

    private (int x, int y) placedLetterCords;

    public void PlaceLetter(char letter, int x, int y)
    {
        placedLetterCords = (x, y);
        board[x, y] = letter;
    }
    public bool ConnectWord(int x, int y)
    {
        if (IsAccessable(x, y))
        {
            placedLetterCords = (x, y);
            return true;
        }
        return false;
    }

    private bool IsAccessable(int x, int y)
    {
        if (IsEmpty(x, y))
        {
            if ((x - 1 == placedLetterCords.x || x + 1 == placedLetterCords.x) && y == placedLetterCords.y
                || (y - 1 == placedLetterCords.x || y + 1 == placedLetterCords.x) && x == placedLetterCords.x)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsEmpty(int x, int y)
    {
        if (string.IsNullOrEmpty(board[x,y].ToString()))
        {
            return true;
        }
        return false;
    }

    public string MakeMove(string word, char letter, (int, int) cell)
    {
        string rword = Reverse(word);
        if (settings.CheckWordDict(word))
        {
            AddWord(word, letter, cell);
            ChangeTurn(false);
            return $"{word} {word.Length}";
        }
        else if (settings.CheckWordDict(rword))
        {
            AddWord(rword, letter, cell);
            ChangeTurn(false);
            return $"{word} {word.Length}";
        }
        else
        {
            return string.Empty;
        }
    }

    private void AddWord(string word, char letter, (int, int) cell)
    {
        AddLetter(letter, cell);
        if (turn)
        {
            player1.words.Add(word);
            player1.points += word.Length;
            player1.skipped = 0;
        }
        else
        {
            player2.words.Add(word);
            player2.points += word.Length;
            player2.skipped = 0;
        }
    }

    public void SkipTurn()
    {
        if (turn)
        {
            player1.skipped += 1;
        }
        else
        {
            player2.skipped += 1; 
        }
        if (player1.skipped == 3 && player2.skipped == 3)
        {
            GameOver.Invoke(null, null);
            return;
        }
        ChangeTurn(true);
    }

    private void AddLetter(char letter, (int x, int y) cell)
    {
        board[cell.x, cell.y] = letter;
    }

    private void AddTime()
    {
        if (turn)
        {
            player1.time += addTime;
        }
        else
        {
            player2.time += addTime;
        }
    }

    private void IsGameOver()
    {
        bool gameover = true;
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(0); j++)
            {
                if (board[i,j] == null)
                {
                    gameover = false;
                }
            }
        }
        if (gameover)
        {
            if (player1.points > player2.points)
            {
                GameOver.Invoke(turn, player1.points);
            }
            else if (player2.points < player1.points)
            {
                GameOver.Invoke(turn, player2.points);
            }
            else
            {
                GameOver.Invoke(null, null);
            }
        }
    }

    public string SaveGame()
    {
        string b = string.Empty;
        for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board.Length; j++)
            {
                b += board[i,j] + " ";
            }
            b = b.Remove(b.Length - 1) + "\n";
        }
        return b;
    }

    static string Reverse(string str)
    {
        char[] arr = str.ToCharArray();
        Array.Reverse(arr);
        return new string(arr);

    }
}

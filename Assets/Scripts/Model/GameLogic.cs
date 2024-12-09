using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameLogic
{
    private Settings settings;

    private string[,] board;
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
        board = new string[size, size];
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
            board[x, y] = startWord[x].ToString();
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
            player1.time -= 1;
            if (player1.time <= 0)
            {
                GameOver.Invoke(!turn, player2.points);
            }
        }
        else
        {
            player2.points -= 1;
            if (player2.points <= 0)
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

    public string MakeMove(string word, string letter, (int x, int y) cell)
    {
        if (settings.CheckWordDict(word))
        {
            if (turn)
            {
                player1.words.Add(word);
                AddLetter(letter, cell);
                player1.points += word.Length;
            }
            else
            {
                player2.words.Add(word);
                AddLetter(letter, cell);
                player2.points += word.Length;
            }
            ChangeTurn(false);
            return $"{word} {word.Length}";
        }
        else
        {
            return string.Empty;
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
        if (player1.skipped + player2.skipped == 6)
        {
            GameOver.Invoke(null, null);
            return; //Конец игры
        }
        ChangeTurn(true);
    }

    private void AddLetter(string letter, (int x, int y) cell)
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
        Debug.Log(addTime);
    }

    private void IsGameOver()
    {
        bool gameover = true;
        for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board.Length; j++)
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
                //p1 win
                GameOver.Invoke(turn, player1.points);
            }
            else if (player2.points < player1.points)
            {
                //p2 win
                GameOver.Invoke(turn, player2.points);
            }
            else
            {
                //tie
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
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] private View view;
    [Space]
    [SerializeField] private GameObject board;
    [SerializeField] private GameObject sampleGridButton;
    [SerializeField] private TMP_Text points1;
    [SerializeField] private TMP_Text points2;
    [SerializeField] private TMP_Text time1;
    [SerializeField] private TMP_Text time2;
    [SerializeField] private Image words1;
    [SerializeField] private Image words2;
    [SerializeField] private UIMessage message;

    private GridButton[,] grid;
    private int boardSize;
    private string startWord;
    private bool isTimeControlEnabled;

    private void OnEnable()
    {
        message.gameObject.SetActive(false);
        GetBoardSize();
        GenerateBoard(boardSize);
        GetPoints();
        DisplayStartWord();
    }

    private void Start()
    {
        TimeControlEnable();
        if (isTimeControlEnabled)
        {
            InvokeRepeating("ChangeTime", 0, 1);
        }
        else
        {
            time1.gameObject.SetActive(false);
            time2.gameObject.SetActive(false);
        }
        GetTurn();
    }

    public void GetBoardSize()
    {
        startWord = view.GetBoardSize();
        boardSize = startWord.Length;
    }

    public void GenerateBoard(int size)
    {
        grid = new GridButton[size, size];
        Transform parent = board.transform;
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        float buttonSize = board.GetComponent<RectTransform>().sizeDelta.x / size;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                GameObject button = Instantiate(sampleGridButton, parent);
                button.name = $"Button_{y}_{x}";

                RectTransform buttonRect = button.GetComponent<RectTransform>();
                buttonRect.anchorMin = Vector2.zero;
                buttonRect.anchorMax = Vector2.zero;
                buttonRect.pivot = Vector2.zero;
                buttonRect.sizeDelta = new Vector2(buttonSize, buttonSize);
                buttonRect.anchoredPosition = new Vector2(x * buttonSize, y * buttonSize);
                grid[x, y] = button.GetComponent<GridButton>();
            }
        }
    }

    private void DisplayStartWord()
    {
        int x = startWord.Length / (int)2;
        for (int y = 0; y < startWord.Length; y++)
        {
            grid[y, x].SetLetter(startWord[y].ToString());
        }
    }

    private void GetPoints()
    {
        (int p1, int p2) points = view.GetPoints();
        points1.text = points.p1.ToString();
        points2.text = points.p2.ToString();
    }

    private void TimeControlEnable()
    {
        isTimeControlEnabled = view.TimeControlEnable();
    }

    private void ChangeTime()
    {
        (string t1, string t2) time = view.GetTime();
        time1.text = time.t1;
        time2.text = time.t2;
    }

    private void GetTurn()
    {
        if (view.GetTurn())
        {
            words1.color = Color.green;
            words2.color = Color.gray;
        }
        else
        {
            words2.color = Color.green;
            words1.color = Color.gray;
        }
    }

    public string MakeMove(string word, string letter, (int, int) cell)
    {
        return view.MakeMove(word, letter, cell);
    }

    public void SkipTurn()
    {
        view.SkipTurn();
        GetTurn();
    }

    public void GameResults(bool? player, int? points)
    {
        if (player == null && points == null)
        {
            Tie();
        }
        else if (player == true)
        {
            PWin("Игрок 1 победил.", (int)points);
        }
        else
        {
            PWin("Игрок 2 победил.", (int)points);
        }
    }

    private void Tie()
    {
        UnityAction act = () => view.ToMenu();
        message.Show("Ничья!", act);
    }

    private void PWin(string m, int points)
    {
        UnityAction act = () => view.ToRecords(points);
        message.Show(m, act);
    }
}

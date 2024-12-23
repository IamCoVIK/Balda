using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static log4net.Appender.ColoredConsoleAppender;

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
    [SerializeField] private Button cancel;
    [SerializeField] private SelectLetter selectLetter;
    [SerializeField] private Board boardControl;
    [SerializeField] private Button skipButton;
    [SerializeField] private Button makeMove;
    [SerializeField] private TMP_Text list1;
    [SerializeField] private TMP_Text list2;

    public GridButton[,] grid;
    public int boardSize;
    private string startWord;
    private bool isTimeControlEnabled;

    private GridButton currentCell;

    private void OnEnable()
    {
        message.gameObject.SetActive(false);
        cancel.gameObject.SetActive(false);
        makeMove.gameObject.SetActive(false);
        GetBoardSize();
        GenerateBoard(boardSize);
        GetPoints();
        DisplayStartWord();
        GetTurn();

        list1.text = string.Empty;
        list2.text = string.Empty;
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
                grid[x, y].coords = (x, y);
            }
        }
    }

    private void DisplayStartWord()
    {
        int x = startWord.Length / (int)2;
        for (int y = 0; y < startWord.Length; y++)
        {
            grid[y, x].SetLetter(startWord[y].ToString().ToUpper());
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

    private bool GetTurn()
    {
        if (view.GetTurn())
        {
            words1.color = Color.green;
            words2.color = Color.gray;
            return true;
        }
        else
        {
            words2.color = Color.green;
            words1.color = Color.gray;
            return false;
        }
    }

    public void StartMove()
    {
        cancel.gameObject.SetActive(true);
        makeMove.gameObject.SetActive(true);
        makeMove.interactable = false;
        skipButton.gameObject.SetActive(false);
    }

    public void GetCurrentCell()
    {
        currentCell = boardControl.currentButton;
        makeMove.interactable = true;
    }

    public void CancelMove()
    {
        if (currentCell != null)
        {
            currentCell.SetLetter(string.Empty);
            currentCell.InteractionOn();
            currentCell = null;
        }
        ClearAfter();
    }
    private void ClearAfter()
    {
        cancel.gameObject.SetActive(false);
        makeMove.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(true);
        selectLetter.ResetButtons();
        boardControl.wordSelection = false;
        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                grid[y, x].InteractionOn();
            }
        }
    }

    public void MakeMove()
    {
        string word = FindWord();
        Debug.Log(word);
        string letter = currentCell.letter;
        (int, int) cell = currentCell.coords;
        string result = view.MakeMove(word, letter, cell);
        if (result == string.Empty)
        {
            UnityAction act = () => CloseMessage();
            message.Show("Такого слова нет!", act);
        }
        else
        {
            GetPoints();
            if (!GetTurn())
            {
                list1.text += result + "\n";
            }
            else
            {
                list2.text += result + "\n";
            }
            currentCell = null;
            ClearAfter();
        }
    }

    public string FindWord()
    {
        GridButton current = FindStart();
        Debug.Log(current.letter);
        GridButton prev;
        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        for (int i = 0; i < 4; i++)
        {
            int neighborX = current.coords.x + dx[i];
            int neighborY = current.coords.y + dy[i];

            if (currentCell.IsNeighborNotInteractable(neighborX, neighborY))
            {
                prev = current;
                currentCell = grid[neighborX, neighborY];
            }
        }
        while (true)
        {

        }

        //return FindWordRecursive(startButton.coords.x, startButton.coords.y, "", new HashSet<(int, int)>());
    }

    /*private string FindWordRecursive(int x, int y, string currentWord, HashSet<(int, int)> visited)
    {
        if (x < 0 || x >= boardSize || y < 0 || y >= boardSize) return currentWord;

        // Проверяем, что кнопка интерактивна и имеет букву
        if (grid[y, x].Interactable() || grid[y, x].letter == string.Empty) return currentWord;

        if (visited.Contains((x, y)))
        {
            return currentWord;
        }

        visited.Add((x, y));
        string newWord = currentWord + grid[y, x].letter;
        int[,] offsets = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };

        string maxWord = newWord;
        for (int i = 0; i < offsets.GetLength(0); i++)
        {
            int neighborX = x + offsets[i, 0];
            int neighborY = y + offsets[i, 1];
            string foundWord = FindWordRecursive(neighborX, neighborY, newWord, visited);
            if (foundWord.Length > maxWord.Length)
            {
                maxWord = foundWord;
            }
        }
        return maxWord;
    }*/

    public GridButton FindStart()
    {
        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                if (!grid[y, x].Interactable() && grid[y, x].letter != string.Empty)
                {
                    if (HasNonInteractableNeighbor(x, y) && grid[y, x].CountNonInteractableNeighbors() == 1)
                    {
                        return grid[y, x];
                    }
                }
            }
        }
        return null;
    }

    private bool HasNonInteractableNeighbor(int x, int y)
    {
        int[,] offsets = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };

        for (int i = 0; i < offsets.GetLength(0); i++)
        {
            int neighborX = x + offsets[i, 0];
            int neighborY = y + offsets[i, 1];

            if (neighborX >= 0 && neighborX < boardSize && neighborY >= 0 && neighborY < boardSize)
            {
                if (!grid[neighborY, neighborX].Interactable())
                {
                    return true;
                }
            }
        }
        return false;
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

    private void CloseMessage()
    {
        message.gameObject.SetActive(false);
    }
}

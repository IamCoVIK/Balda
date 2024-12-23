using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GridButton : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    public string letter = string.Empty;

    private Button button;
    private Board board;

    public (int x, int y) coords;

    private void Start()
    {
        button = GetComponent<Button>();
        board = GetComponentInParent<Board>();

        UnityAction act = () => Click();
        button.onClick.AddListener(act);
    }

    public void SetLetter(string l)
    {
        letter = l;
        text.text = letter;
    }

    public void Click()
    {
        if (board.wordSelection && letter != string.Empty)
        {
            int[] dx = { 1, -1, 0, 0 };
            int[] dy = { 0, 0, 1, -1 };

            for (int i = 0; i < 4; i++)
            {
                int neighborX = coords.x + dx[i];
                int neighborY = coords.y + dy[i];

                if (IsNeighborNotInteractable(neighborX, neighborY) && CountNonInteractableNeighbors() == 1)
                {
                    if (board.game.grid[neighborX, neighborY].CountNonInteractableNeighbors() <= 1)
                    {
                        InteractionOff();
                        return;
                    }
                }
            }
        }
        else if (letter == string.Empty && board.GetLetter() != string.Empty && !board.wordSelection)
        {
            SetLetter(board.GetLetter());
            board.wordSelection = true;
            board.LetterPlaced(this);

            InteractionOff();
        }
    }

    public void InteractionOn()
    {
        button.interactable = true;
    }
    public void InteractionOff()
    {
        button.interactable = false;
    }
    public bool Interactable()
    {
        return button.interactable;
    }
    private bool IsValidCoordinate(int x, int y)
    {
        return x >= 0 && x < board.game.boardSize && y >= 0 && y < board.game.boardSize;
    }
    public bool IsNeighborNotInteractable(int x, int y)
    {
        if (!IsValidCoordinate(x, y))
        {
            return false;
        }
        return !board.game.grid[x, y].Interactable();
    }
    public int CountNonInteractableNeighbors()
    {
        int count = 0;
        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        for (int i = 0; i < 4; i++)
        {
            int neighborX = coords.x + dx[i];
            int neighborY = coords.y + dy[i];

            if (IsNeighborNotInteractable(neighborX, neighborY))
            {
                count++;
            }
        }
        return count;
    }
}

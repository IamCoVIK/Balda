using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Board : MonoBehaviour
{
    public bool wordSelection;
    public GridButton currentButton;

    [SerializeField] private SelectLetter selectLetter;
    public Game game;

    public UnityEvent onLetterPlaced;

    private void Start()
    {
        wordSelection = false;
    }

    public string GetLetter()
    {
        return selectLetter.selectedLetter;
    }

    public void LetterPlaced(GridButton gb)
    {
        currentButton = gb;
        onLetterPlaced.Invoke();
    }
}

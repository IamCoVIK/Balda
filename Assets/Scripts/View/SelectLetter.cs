using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectLetter : MonoBehaviour
{
    public string selectedLetter;

    private AlphabetButton[] alphabet;

    public UnityEvent letterSelected;

    private void Start()
    {
        alphabet = GetComponentsInChildren<AlphabetButton>();
    }

    public void SetLetter(string l)
    {
        selectedLetter = l;
        letterSelected.Invoke();
    }

    public void ResetButtons()
    {
        foreach (AlphabetButton alphabetButton in alphabet)
        {
            alphabetButton.Activate();
        }
        selectedLetter = string.Empty;
    }
}

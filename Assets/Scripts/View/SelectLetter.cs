using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLetter : MonoBehaviour
{
    private string selectedLetter;

    private AlphabetButton[] alphabet;

    private void Start()
    {
        alphabet = GetComponentsInChildren<AlphabetButton>();
    }

    public void SetLetter(string l)
    {
        selectedLetter = l;
        Debug.Log(selectedLetter);
    }

    public void ResetButtons()
    {
        foreach (AlphabetButton alphabetButton in alphabet)
        {
            alphabetButton.Activate();
        }
    }
}

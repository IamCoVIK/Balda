using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridButton : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    [SerializeField] private string letter = "";

    public void SetLetter(string l)
    {
        letter = l;
        text.text = letter;
    }
}

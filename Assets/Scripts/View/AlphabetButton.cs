using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AlphabetButton : MonoBehaviour
{
    public string letter;

    private SelectLetter selectLetter;
    private TMP_Text text;
    private Button button;

    private void Start()
    {
        selectLetter = GetComponentInParent<SelectLetter>();
        button = GetComponent<Button>();
        text = GetComponentInChildren<TMP_Text>();
        text.text = letter;

        UnityAction act = () => Press();
        button.onClick.AddListener(act);
    }

    private void Press()
    {
        selectLetter.ResetButtons();
        selectLetter.SetLetter(letter);
        button.interactable = false;
    }

    public void Activate()
    {
        button.interactable = true;
    }
}

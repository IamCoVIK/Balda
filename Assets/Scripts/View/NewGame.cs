using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class NewGame : MonoBehaviour
{
    [SerializeField] private View view;
    [Space]
    [SerializeField] private TMP_InputField startWord;
    [SerializeField] private Toggle toggleTimeControl;
    [SerializeField] private TMP_Dropdown minutes;
    [SerializeField] private TMP_Dropdown seconds;
    [SerializeField] private TMP_Dropdown addSeconds;

    private bool isTimeControlEnabled = true;
    private int timer = 0;
    private int addTime = 0;

    public void RandomWord()
    {
        startWord.text = view.RandomWord();
    }

    public void Back()
    {
        view.Back();
    }

    public void EnableTimeControl()
    {
        isTimeControlEnabled = !toggleTimeControl.isOn;
    }

    public void UpdateTimeControl()
    {
        timer = minutes.value * 60 + seconds.value * 15;
    }

    public void UpdateAddTime()
    {
        addTime = addSeconds.value * 15;
    }

    public void StartGame()
    {
        string error = view.StartGame(startWord.text, isTimeControlEnabled, timer, addTime);
        if (error != string.Empty)
        {
            ShowError(error);
        }
    }

    public void ShowError(string error)
    {
        Debug.Log(error);
    }
}

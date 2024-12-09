using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private View view;
    [Space]
    [SerializeField] private TMP_Text recordsList;

    public void NewGame()
    {
        view.NewGame();
    }

    public void LoadGame()
    {
        view.LoadGame();
    }

    public void ExitGame()
    {
        view.ExitGame();
    }

    public void GetRecords((string, string)[] records)
    {
        recordsList.text = "";
        foreach ((string name, string points) record in records)
        {
            recordsList.text += $"{record.name} - {record.points}\n";
        }
    }
}

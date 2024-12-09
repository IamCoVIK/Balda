using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Settings
{
    public string startWord;
    public bool isTimeControlEnabled;
    public int timer;
    public int addTime;

    private string dict_path = Application.persistentDataPath + "/dictionary.txt";
    private string[] dictionary;

    public Settings()
    { 
        startWord = string.Empty;
        isTimeControlEnabled = true;
        timer = 0;
        addTime = 0;

        LoadDictionary();
    }

    private void LoadDictionary()
    {
        dictionary = File.ReadAllLines(dict_path);
    }

    public bool CheckWordLength(string word)
    {
        if (word.Length < 5)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool CheckWordDict(string word)
    {
        if (Array.IndexOf(dictionary, word.ToLower()) == -1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public string RandomWord()
    {
        while (true) 
        {
            string rword = dictionary[UnityEngine.Random.Range(0, dictionary.Length)];
            if (rword.Length == 5)
            {
                return rword;
            }
        }
    }

    public string CheckStartConditions(string startWord, bool isTimeControlEnabled, int timer, int addTime)
    {
        string error = string.Empty;

        if (!CheckWordLength(startWord))
        {
            error += "Начальное слово меньше 5 букв.\n";
        }
        if (!CheckWordDict(startWord))
        {
            error += "Начальное слово не присутствует в словаре.\n";
        }
        if (timer == 0 && isTimeControlEnabled)
        {
            error += "Время на партию не может быть нулевым.\n";
        }
        if (error == string.Empty)
        {
            this.startWord = startWord.ToLower();
            this.isTimeControlEnabled = isTimeControlEnabled;
            this.timer = timer;
            this.addTime = addTime;
        }
        return error;
    }
}

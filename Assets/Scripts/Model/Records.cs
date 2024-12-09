using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System;

public class Records
{
    private List<Record> records = new List<Record>();
    private string file = Application.persistentDataPath + "/records.txt";

    public Records()
    {
        LoadRecords();
    }

    private void LoadRecords()
    {
        if (File.Exists(file))
        {
            string[] lines = File.ReadAllLines(file);
            Regex regex = new Regex(@"^[\w\s]+?\|[\d]+$");

            foreach (string line in lines)
            {
                if (regex.IsMatch(line))
                {
                    string[] split = line.Split('|');
                    records.Add(ToRecord(split[0], split[1]));
                }
            }
        }
        else
        {
            File.Create(file);
        }
    }

    public (string, string)[] GetRecords()
    {
        (string, string)[] output = new (string, string)[records.Count];
        for(int i = 0; i < records.Count; i++)
        {
            output[i] = (records[i].Name, records[i].Points);
        }
        return output;
    }

    public void AddRecord(string name, string points)
    {
        records.Add(ToRecord(name, points));
        SaveRecords();
    }

    private Record ToRecord(string name, string points)
    {
        return new Record(name, points);
    }

    private void SaveRecords()
    {
        string to_file = "";
        foreach (Record record in records)
        {
            to_file += record.ToString() + '\n';
        }
        to_file.Remove(to_file.Length - 1);
        File.WriteAllText(file, to_file);
    }
}

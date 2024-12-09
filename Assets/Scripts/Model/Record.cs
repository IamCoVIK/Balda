using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Record
{
    public string Name;
    public string Points;

    public Record(string name, string points)
    {
        Name = name;
        Points = points;
    }

    public override string ToString()
    {
        return Name + '|' + Points;
    }
}

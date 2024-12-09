using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public List<string> words;
    public int points;
    public int skipped;
    public int time;

    public Player(int time)
    {
        words = new List<string>();
        points = 0;
        skipped = 0;
        this.time = time;
    }
}


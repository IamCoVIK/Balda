using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class Test_Records
{
    Records records = new Records();

    [Test]
    public void Test1_GetRecords()
    {
        bool a = true;
        (string, string)[] r = records.GetRecords();
        foreach ((string name, string points) in r)
        {
            try
            {
                Convert.ToInt32(points);
            }
            catch
            {
                a = false;
            }
        }
        Assert.True(a);
    }
}

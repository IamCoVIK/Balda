using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Test_CheckWord
{
    Settings settings = new Settings();
    [Test]
    public void Test1_CheckWordLength()
    {
        Assert.False(settings.CheckWordLength("����"));
    }
    [Test]
    public void Test2_CheckWordLength()
    {
        Assert.True(settings.CheckWordLength("�����"));
    }
    [Test]
    public void Test3_CheckWordLength()
    {
        Assert.True(settings.CheckWordLength("���������"));
    }

    [Test]
    public void Test1_CheckWordDict()
    {
        Assert.False(settings.CheckWordDict("dvsvsvewge"));
    }
    [Test]
    public void Test2_CheckWordDict()
    {
        Assert.True(settings.CheckWordDict("�����"));
    }
}

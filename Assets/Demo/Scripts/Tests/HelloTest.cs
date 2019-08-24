using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class HelloTest 
{
    [Test]
    public void helloWorld(){
        Hello h = new Hello();
        Assert.AreNotSame(h, null);
    }
}

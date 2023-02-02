using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fairy : Defender
{
    
    void Start()
    {
        base.SetStartConditions();
    }

    
    void Update()
    {
        base.CheckMonstersOnTheLevel();
    }
}

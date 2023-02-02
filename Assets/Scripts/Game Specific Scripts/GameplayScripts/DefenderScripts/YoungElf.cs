using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoungElf : Defender
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

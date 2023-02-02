using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakOrc : MeleeMonsters
{
   
    void Start()
    {
        base.SetStartConditions();   
    }

    
    void Update()
    {
        base.MonsterStateMachine();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Goblin : MeleeMonsters
{
    Vector3[] Dests;

    void Start()
    {
        base.SetStartConditions();
        
    }

    
    void Update()
    {
        base.MonsterStateMachine();
        
    }

   



}

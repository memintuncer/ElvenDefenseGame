using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PoolData 
{
    public Dictionary<MonsterTypes.Type, int> MonstersAtPoolCountDictionary = new Dictionary<MonsterTypes.Type, int>();
}

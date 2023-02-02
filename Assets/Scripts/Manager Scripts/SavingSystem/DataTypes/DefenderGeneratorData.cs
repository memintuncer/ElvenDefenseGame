using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DefenderGeneratorData 
{
    public Dictionary<DefenderTypes.Type, int> DefenderCountsDict = new Dictionary<DefenderTypes.Type, int>();
    public List<int> DefenderPositionIndexes = new List<int>();
    public List<DefenderData> DefenderDatas = new List<DefenderData>();
    public List<int> DefenderIndexes = new List<int>();
    public int CurrentDefenderCount = 0, RequiredGold = 0;
    
    
}

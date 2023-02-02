using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameManagerData
{
    public int CurrentGold, CurrentMonsterIndex,KilledEnemyCount, WaveNumber;
    public float Timer;
    public int LoadMonsterIndex;
    public Dictionary<MonsterTypes.Type, int> AllMonsters = new Dictionary<MonsterTypes.Type, int>();
    public Dictionary<MonsterTypes.Type, int> TargetMonsters = new Dictionary<MonsterTypes.Type, int>();
    public List<MonsterData> MonsterDatas = new List<MonsterData>();
    public List<int> MonsterIndexes = new List<int>();



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalGameManager : MonoBehaviour
{
    PoolingSystem PoolingSystemInstance;

    [SerializeField] private int MonsterCounts;
    private Dictionary<MonsterTypes.Type, List<Monster>> AllMonstersDictionary = new Dictionary<MonsterTypes.Type, List<Monster>>();
    public List<MonsterTypes.Type> AllMonsterTypes = new List<MonsterTypes.Type>();
    public List<GameObject> AllMonsters = new List<GameObject>();

    void Start()
    {
        PoolingSystemInstance = PoolingSystem.Instance;
        GetAllMonsterTypes();
        GetMonstersFromPool();
    }

    
    void Update()
    {
        
    }


    void GetAllMonsterTypes()
    {
        Dictionary<MonsterTypes.Type, GameObject> monster_object_dictionary = PoolingSystemInstance.GetMonsterObjectDictionary();

        foreach(MonsterTypes.Type monster_type in monster_object_dictionary.Keys)
        {
            AllMonsterTypes.Add(monster_type);
            
        }

    }

    void GetMonstersFromPool()
    {
        foreach(MonsterTypes.Type monster_type in AllMonsterTypes)
        {
            PoolingSystemInstance.CheckMonsterPool(monster_type, MonsterCounts,transform);
            List<Monster> new_monsters = new List<Monster>();
            List<GameObject> monsters_in_pool = PoolingSystemInstance.GetSpecificMonsterPool(monster_type);
            for(int i = 0; i < MonsterCounts; i++)
            {
                Monster new_monster_script = monsters_in_pool[i].GetComponent<Monster>();
                new_monsters.Add(new_monster_script);
            }
            AllMonstersDictionary[monster_type] = new_monsters;
        }           

        

    }

    

}

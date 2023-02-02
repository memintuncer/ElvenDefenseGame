using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameLevel : MonoBehaviour
{
    [Serializable] public class LevelSpecificMonsters
    {
        [SerializeField] private MonsterTypes.Type MonsterType;
        [SerializeField] private int MonsterCount;
        

        public MonsterTypes.Type GetMonsterType()
        {
            return MonsterType;
        }

        public int GetMonstercount()
        {
            return MonsterCount;
        }

        public void SetMonsterCount(int monster_count)
        {
            MonsterCount = monster_count;
        }


    }

    [SerializeField] private List<LevelSpecificMonsters> LevelSpecificMonstersList = new List<LevelSpecificMonsters>();
    [SerializeField] private GameManager GameManager;
    public List<Monster> Monsters = new List<Monster>();
    PoolingSystem PoolingSystemInstance;
    [SerializeField] private Transform MonsterDestionationsTransform;
    [SerializeField] int StartingGold;
    private int LevelIndex;
    

    public int GetLevelIndex()
    {
        return LevelIndex;
    }
    
    public void SetLevelIndex(int level_index)
    {
        LevelIndex = level_index;
    }

    public Transform GetMonsterDestionationsTransform()
    {
        return MonsterDestionationsTransform;
    }
    void Start()
    {
        PoolingSystemInstance = PoolingSystem.Instance;

        if (!GameManager.GetIsLoaded())
        {
            GameManager.SetGameLevel(this);
            GameManager.SetMonsterDestinations(MonsterDestionationsTransform);
            
            GameManager.SetCurrentGold(StartingGold);


            ChekPoolingSystemForMonsters();
            GetMonstersFromPool();
        }
        
    }

    
    void Update()
    {
        
    }


    void ChekPoolingSystemForMonsters()
    {
       foreach(LevelSpecificMonsters monster in LevelSpecificMonstersList)
        {
            MonsterTypes.Type monster_type = monster.GetMonsterType();
            int monster_count = monster.GetMonstercount();
            
            PoolingSystem.Instance.CheckMonsterPool(monster_type,monster_count,transform);
        }
    }

    public void GetMonstersFromPool()
    {
        int count = GameManager.GetAllMonsters().Count;
        if (count == 0)
        {
            foreach (LevelSpecificMonsters monster in LevelSpecificMonstersList)
            {
                MonsterTypes.Type monster_type = monster.GetMonsterType();
                int monster_count = monster.GetMonstercount();
                List<GameObject> monsters_in_pool = PoolingSystem.Instance.GetSpecificMonsterPool(monster_type);
                for (int i = 0; i < monster_count; i++)
                {
                    Monster new_monster_script = monsters_in_pool[i].GetComponent<Monster>();
                    GameManager.AddMonsters(new_monster_script);

                }
            }
        }
        
        
    }


    public void UpdateMonstersForSurvivalMod(int survival_monster_increase)
    {
        
        foreach (LevelSpecificMonsters monster in LevelSpecificMonstersList)
        {
            int previous_monster_count = monster.GetMonstercount();
            int new_monster_count = previous_monster_count + survival_monster_increase;
            monster.SetMonsterCount(new_monster_count);
            
          
        }

        ChekPoolingSystemForMonsters();
        GetMonstersFromPool();
    }

    
}

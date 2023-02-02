using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public  class PoolingSystem : MonoBehaviour, DataPersistenceInterface
{
    [Serializable] private class MonsterPoolingObject
    {
        [SerializeField] private GameObject MonsterPrefab;
        [SerializeField] MonsterTypes.Type Type;

        public MonsterTypes.Type GetMonsterType()
        {
            return Type;
        }

        public GameObject GetMonsterPrefab()
        {
            return MonsterPrefab;
        }

    }
    
    [Serializable] private class BulletPoolingObject
    {
        [SerializeField] private GameObject BulletPrefab;
        [SerializeField] DefenderTypes.Type DefenderType;


        public GameObject GetBulletPrefab()
        {
            return BulletPrefab;
        }

        public DefenderTypes.Type GetDefenderType()
        {
            return DefenderType;
        }

    }

    private Dictionary<MonsterTypes.Type, GameObject> MonsterObjectDictionary = new Dictionary<MonsterTypes.Type, GameObject>();
    private Dictionary<MonsterTypes.Type, List<GameObject>> MonsterPoolDictionary = new Dictionary<MonsterTypes.Type, List<GameObject>>();
    private Dictionary<DefenderTypes.Type, GameObject> BulletObjectDictionary = new Dictionary<DefenderTypes.Type, GameObject>();
    private Dictionary<DefenderTypes.Type, List<GameObject>> BulletPoolDictionary = new Dictionary<DefenderTypes.Type, List<GameObject>>();

    [SerializeField] private List<MonsterPoolingObject> MonsterPoolingObjectList = new List<MonsterPoolingObject>();
    [SerializeField] private List<BulletPoolingObject> BulletPoolingObjectList = new List<BulletPoolingObject>();


    public static PoolingSystem Instance;

    Dictionary<MonsterTypes.Type, int> MonstersAtPoolCountData = new Dictionary<MonsterTypes.Type, int>();
    public int X = 0;

    public Dictionary<MonsterTypes.Type, GameObject> GetMonsterObjectDictionary()
    {
        return MonsterObjectDictionary;
    }



    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.SAVE_LOAD_EVENTS.SAVE_GAME_BUTTON_CLICKED, SaveGameData);
        EventManager.StartListening(GameConstants.SAVE_LOAD_EVENTS.LOAD_POOL_DATA, LoadPoolData);
    }

    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.SAVE_LOAD_EVENTS.SAVE_GAME_BUTTON_CLICKED, SaveGameData);
        EventManager.StopListening(GameConstants.SAVE_LOAD_EVENTS.LOAD_POOL_DATA, LoadPoolData);
    }


    void SaveGameData(EventParam param)
    {
        Debug.Log("StartSaving");
        Time.timeScale = 0;
        GameData game_data = param.GetGameData();
        SaveData(ref game_data);
        EventManager.TriggerEvent(GameConstants.SAVE_LOAD_EVENTS.MONSTERS_SAVED_TO_POOL, param);
    }


    void LoadPoolData(EventParam param)
    {
        GameData game_data = param.GetGameData();
        LoadData(game_data);
        EventManager.TriggerEvent(GameConstants.SAVE_LOAD_EVENTS.LOAD_SURVIVAL_LEVEL, param);
    }


    private void Awake()
    {
        Instance = this;
        CreateMonsterObjectDictionary();
        CreateBulletObjectDictionary();
        

        foreach(DefenderTypes.Type defender_type in BulletObjectDictionary.Keys)
        {
            CheckBulletPool(defender_type, 30);
        }

    }


    private void CreateMonsterObjectDictionary()
    {
        foreach(MonsterPoolingObject monster_pool_object in MonsterPoolingObjectList)
        {
            
            MonsterObjectDictionary.Add(monster_pool_object.GetMonsterType(),monster_pool_object.GetMonsterPrefab());
            
            List<GameObject> new_monster_pool = new List<GameObject>();
            
            MonsterPoolDictionary.Add(monster_pool_object.GetMonsterType(), new_monster_pool);

        }
    }


    public void CheckMonsterPool(MonsterTypes.Type monster_type,int monster_count, Transform parent)
    {
        List<GameObject> monster_pool = MonsterPoolDictionary[monster_type];

        if (monster_pool.Count == 0 || monster_pool.Count < monster_count)
        {
            CreateMonstersToPool(monster_type, monster_count,monster_pool,parent);
        }
        
    }

    public void CreateMonstersToPool(MonsterTypes.Type monster_type, int count, List<GameObject> monster_pool,Transform parent)
    {
        GameObject monster_object_prefab = MonsterObjectDictionary[monster_type];

        for(int i = 0; i < count * 2; i++)
        {
            GameObject new_monster_object = Instantiate(monster_object_prefab, Vector2.zero, Quaternion.identity);
           
            new_monster_object.SetActive(false);
            monster_pool.Add(new_monster_object);
        }

    }


    public List<GameObject> GetSpecificMonsterPool(MonsterTypes.Type monster_type)
    {
        return MonsterPoolDictionary[monster_type];
    }





    public void LoadData(GameData game_data)
    {
        PoolData pool_data = game_data.GetPoolData();
        foreach(MonsterTypes.Type monster_type in pool_data.MonstersAtPoolCountDictionary.Keys) 
        {
            int monster_count = pool_data.MonstersAtPoolCountDictionary[monster_type];
            CheckMonsterPool(monster_type, monster_count/2, transform);
        }
       

    }

    public void SaveData(ref GameData game_data) 
    {
        PoolData temp_pool_data = new PoolData();
        foreach (MonsterTypes.Type monster_type in MonsterPoolDictionary.Keys)
        {
            int monster_count = MonsterPoolDictionary[monster_type].Count;
            if (MonstersAtPoolCountData.ContainsKey(monster_type))
            {
                MonstersAtPoolCountData[monster_type] = monster_count;
            }
            else
            {
                MonstersAtPoolCountData.Add(monster_type, monster_count);
            }
            
            
        }
        temp_pool_data.MonstersAtPoolCountDictionary = MonstersAtPoolCountData;
        game_data.SetPoolData(temp_pool_data);
       
    }



    public void CreateBulletsForPool(DefenderTypes.Type defender_type,int  bullet_count,List<GameObject> bullet_pool)
    {
       

        GameObject bullet_prefab = BulletObjectDictionary[defender_type];
        for(int i = 0; i < bullet_count; i++)
        {
            GameObject new_bullet = Instantiate(bullet_prefab, Vector2.zero,Quaternion.identity);
            new_bullet.SetActive(false);
            bullet_pool.Add(new_bullet);
            X++;
        }
    }


    

    public void CheckBulletPool(DefenderTypes.Type defender_type, int bullet_count)
    {
        List<GameObject> bullets = BulletPoolDictionary[defender_type];
        int current_bullet_count = bullets.Count;
        if (current_bullet_count < bullet_count)
        {
            int new_bullet_count = bullet_count - current_bullet_count;
            CreateBulletsForPool(defender_type, new_bullet_count, bullets);
            
        }
    }


    private void CreateBulletObjectDictionary()
    {
        foreach (BulletPoolingObject bullet_pool_object in BulletPoolingObjectList)
        {

            BulletObjectDictionary.Add(bullet_pool_object.GetDefenderType(), bullet_pool_object.GetBulletPrefab());

            List<GameObject> new_bullet_pool = new List<GameObject>();

            BulletPoolDictionary.Add(bullet_pool_object.GetDefenderType(), new_bullet_pool);

        }
    }


    public void GetBulletFromPool(DefenderTypes.Type defender_type, int bullet_count, List<Bullet> defender_bullets,Defender defender)
    {
        
        List<GameObject> bullet_pool = BulletPoolDictionary[defender_type];
        
        CheckBulletPool(defender_type, bullet_count);
        
        for (int i =0; i < bullet_count; i++)
        {
            GameObject bullet = bullet_pool[0];
            Bullet bullet_script = bullet.GetComponent<Bullet>();
            bullet_script.transform.parent = defender.transform;
            bullet.transform.position = defender.transform.position;
            bullet_script.SetDefenderParent(defender);
            defender_bullets.Add(bullet_script);
            bullet_pool.Remove(bullet);

        }

        
    }
}

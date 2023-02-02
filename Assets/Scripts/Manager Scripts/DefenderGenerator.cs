using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderGenerator : MonoBehaviour, DataPersistenceInterface
{

    [System.Serializable]
    public class DefenderObject
    {
        [SerializeField] private DefenderTypes.Type Type;
        [SerializeField] private GameObject DefenderPrefab;

        public GameObject GetDefenderPrefab()
        {
            return DefenderPrefab;
        }

        public DefenderTypes.Type GetDefenderType()
        {
            return Type;
        }
    }
    

    [SerializeField] private List<DefenderObject> DefenderObjects = new List<DefenderObject>();  

    [SerializeField] private GameManager GameManager;
    [SerializeField] private List<GameObject> DefenderPrefabs;
    [SerializeField] private Transform DefensePositionsParent;
    public List<Transform> DefencePositionsList = new List<Transform>();
    [SerializeField] private int RequiredGold, RequiredGoldIncreaseAmount;
    public int MaxDefenderCount, CurrentDefenderCount = 0;
    Dictionary<DefenderTypes.Type, int> DefendersCountDict = new Dictionary<DefenderTypes.Type, int>();
    List<int> DefenderPositionIndexes = new List<int>();
    public List<int> DefenderIndexes = new List<int>();
    private List<Defender> AllDefenders = new List<Defender>();

    [SerializeField] GameObject ContinueButton;
    DefenderGeneratorData DefenderGeneratorData ;
    

    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.SAVE_LOAD_EVENTS.GAME_MANAGER_DATA_SAVED, SaveData);
        EventManager.StartListening(GameConstants.SAVE_LOAD_EVENTS.LOAD_DEFENDER_GENERATOR_DATA, LoadData);
    }

    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.SAVE_LOAD_EVENTS.GAME_MANAGER_DATA_SAVED, SaveData);
        EventManager.StopListening(GameConstants.SAVE_LOAD_EVENTS.LOAD_DEFENDER_GENERATOR_DATA, LoadData);
    }

    void SaveData(EventParam param)
    {
        GameData game_data = param.GetGameData();
        SaveData(ref game_data);
        EventManager.TriggerEvent(GameConstants.SAVE_LOAD_EVENTS.DEFENDER_GENERATOR_DATA_SAVED, param);
    }

    

    public void SaveData(ref GameData game_data)
    {
        DefenderGeneratorData defender_generator_data = new DefenderGeneratorData();
        defender_generator_data.DefenderCountsDict = DefendersCountDict;

        defender_generator_data.DefenderPositionIndexes = DefenderPositionIndexes;
        defender_generator_data.DefenderIndexes = DefenderIndexes;
        defender_generator_data.CurrentDefenderCount = CurrentDefenderCount;
        defender_generator_data.RequiredGold = RequiredGold;
        
        game_data.SetDefenderGeneratorData(defender_generator_data);
        List<DefenderData> defender_datas = defender_generator_data.DefenderDatas;
        SaveDefenderDatas(defender_generator_data,defender_datas);
    }

    void SaveDefenderDatas(DefenderGeneratorData defender_generator_data, List<DefenderData> defender_datas)
    {
        
        foreach (Defender defender in AllDefenders)
        {
            DefenderData defender_data = new DefenderData();
            defender_data.PosX = defender.transform.position.x;
            defender_data.PosY = defender.transform.position.y;
            defender_data.AttackPower = defender.GetAttackPower();
            defender_data.KillCount = defender.GetKillCount();
            defender_data.AttackTimer = defender.GetAttackTimer();
            defender_data.BulletIndex = defender.GetBulletIndex();
            defender_data.BulletData = defender.SaveBulletData();
            defender_datas.Add(defender_data);
            
        }
    }
    void LoadData(EventParam param)
    {
        GameData game_data = param.GetGameData();
        LoadData(game_data);
    }

    public void LoadData(GameData game_data)
    {
        GetDefensePositions();
        DefenderGeneratorData defender_generator_data = game_data.GetDefenderGeneratorData();
        DefenderGeneratorData = defender_generator_data;
        LoadDefenderGeneratorData(defender_generator_data);
        ContinueButton.SetActive(true);
        Time.timeScale = 0;
    }


    void LoadDefenderGeneratorData(DefenderGeneratorData defender_generator_data)
    {
        
        int total_count = 0;
        CurrentDefenderCount = defender_generator_data.CurrentDefenderCount;
        RequiredGold = defender_generator_data.RequiredGold;
       
        DefenderPositionIndexes = defender_generator_data.DefenderPositionIndexes;
        DefenderIndexes = defender_generator_data.DefenderIndexes;
        
        List<DefenderData> DefenderDatas = defender_generator_data.DefenderDatas;
        List<int> defender_position_indexes = defender_generator_data.DefenderPositionIndexes;
        

        Dictionary<DefenderTypes.Type, int> defender_counts = defender_generator_data.DefenderCountsDict;
        
        foreach(DefenderTypes.Type defender_type in defender_counts.Keys)
        {
            int defender_count = defender_counts[defender_type];
            for(int i = 0; i < defender_count; i++)
            {
                int position_index = DefenderPositionIndexes[total_count];
                int defender_index = DefenderIndexes[total_count];

                DefenderObject defender_object = DefenderObjects[defender_index];


                GameObject new_defender = Instantiate(defender_object.GetDefenderPrefab(), Vector2.zero, Quaternion.identity);
                new_defender.transform.parent = transform;
                Defender defender_script = new_defender.GetComponent<Defender>();
                defender_script.SetGameManager(GameManager);
                defender_script.SetTargetMonsters(GameManager.GetMonsters());

                DefenderData defender_data = DefenderDatas[total_count];
                AllDefenders.Add(defender_script);
                defender_script.LoadDefenderData(defender_data);
                AddDefenderToDictionary(defender_script, new_defender);
                
                total_count++;
            }
        }

    }


    public int GetRequiredGold()
    {
        return RequiredGold;
    }

    void Start()
    {
        MaxDefenderCount = DefensePositionsParent.childCount;
        GetDefensePositions();
       
    }

 

    void GetDefensePositions()
    {
        
        for(int i = 0; i < MaxDefenderCount; i++)
        {
            DefencePositionsList.Add(DefensePositionsParent.GetChild(i));
        }

        

        if (DefenderGeneratorData != null && DefenderPositionIndexes.Count>0)
        {
           
            for(int i =0;i< DefenderPositionIndexes.Count; i++)
            {
                int index = DefenderPositionIndexes[i];
                DefencePositionsList.RemoveAt(index);
            }
          
        }
    }



    public void GenerateDefender()
    {
        int current_gold = GameManager.GetCurrentGold();

        if(RequiredGold <= current_gold && CurrentDefenderCount<MaxDefenderCount)
        {
            CreateRandomDefender(current_gold);
        }
    }


    void CreateRandomDefender(int current_gold)
    {
        int random_defender_index = Random.Range(0, DefenderObjects.Count);
        int random_position_index = Random.Range(0, DefencePositionsList.Count);

        DefenderObject defender_object = DefenderObjects[random_defender_index];
        GameObject new_defender = Instantiate(defender_object.GetDefenderPrefab(), Vector2.zero, Quaternion.identity);
        new_defender.transform.parent = transform;
        Defender defender_script = new_defender.GetComponent<Defender>();
        defender_script.SetGameManager(GameManager);
        defender_script.SetTargetMonsters(GameManager.GetMonsters());
        new_defender.transform.position = DefencePositionsList[random_position_index].position + new Vector3(0,0.25f,0);
        
        DefencePositionsList.RemoveAt(random_position_index);
        int new_gold = current_gold - RequiredGold;
        RequiredGold += RequiredGoldIncreaseAmount;
        GameManager.SetCurrentGold(new_gold);
        CurrentDefenderCount++;
        DefenderIndexes.Add(random_defender_index);
        DefenderPositionIndexes.Add(random_position_index);
        
        AddDefenderToDictionary(defender_script,new_defender);
        AllDefenders.Add(defender_script);

    }


    

    void AddDefenderToDictionary(Defender defender,GameObject defender_object)
    {
        DefenderTypes.Type defender_type = defender.GetDefenderType();
        if (DefendersCountDict.ContainsKey(defender_type))
        {
            DefendersCountDict[defender_type] += 1;
        }

        else
        {
            DefendersCountDict[defender_type] = 1;
            
        }
    }

   
}

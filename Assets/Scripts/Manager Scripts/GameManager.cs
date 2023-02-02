using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class GameManager : MonoBehaviour, DataPersistenceInterface
{
    private List<Monster> AllMonsters = new List<Monster>();
    private List<Monster> TargetMonsters = new List<Monster>();
    [SerializeField] private float EnemySpawnTimer,Timer,EnemyMovementTimer;
    private Transform MonsterDestionationsTransform;
    private List<Transform> MonsterDestionationList = new List<Transform>();

    private  int CurrentGold, MonsterIndex;
    private int KilledEnemyCount = 0, EscapedEnemyCount=0, SurvivalModMonsterCountIncrease=5, WaveNumber=0;
    [SerializeField] private GameObject LevelFailedScreen,LevelSuccessedScreen;

    private bool CanPlay=false,Failed = false;
    [SerializeField] Castle GameCastle;

    [SerializeField] bool IsSurvivalMode=false;
    [SerializeField] private GameLevel GameLevel;
    [SerializeField] float WaitForStartDuration;

    private bool IsLoadedGame = false;

    private int LoadMonsterIndex = 0;

    private List<int> MonsterIndexes = new List<int>();

    public Vector3[] Destinations;
    [SerializeField] PathMode PathMode;
    [SerializeField] PathType PathType;
    
    public int GetWaveCount()
    {
        return WaveNumber;
    }

    public bool GetIsLoaded()
    {
        return IsLoadedGame;
    }

    public void SetGameLevel(GameLevel game_level)
    {
        GameLevel = game_level;
    }

    public bool GetIsSurvivalMode()
    {
        return IsSurvivalMode;
    }

    public int GetSurvivalModMonsterCountIncrease()
    {
        return SurvivalModMonsterCountIncrease;
    }

    public void SetMonsterDestinations(Transform monster_destinations_transform)
    {
        
        MonsterDestionationsTransform = monster_destinations_transform;
        Vector3[] temp = new Vector3[MonsterDestionationsTransform.childCount];
        for (int i = 0; i < MonsterDestionationsTransform.childCount; i++)
        {
            Transform monster_destionation = MonsterDestionationsTransform.GetChild(i);
            MonsterDestionationList.Add(monster_destionation);
            temp[i] = MonsterDestionationList[i].position;
            
        }
        Destinations = temp;
    }

    public List<Monster> GetAllMonsters()
    {
        return AllMonsters;
    }

    public void AddMonsters(Monster monster)
    {
        AllMonsters.Add(monster);
        
    }

    public List<Monster> GetMonsters()
    {
        return AllMonsters;
    }

    public int GetCurrentGold()
    {
        return CurrentGold;
    }

    public List<Monster> GetTargetMonsters()
    {
        return TargetMonsters;
    }

    public void SetCurrentGold(int gold)
    {
        CurrentGold = gold;
    }


    public int GetKilledEnemyCount()
    {
        return KilledEnemyCount;
    }

    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.GameEvents.GAME_STARTED, StartGame);
        EventManager.StartListening(GameConstants.GameEvents.MONSTER_REACHED_TO_CASTLE, EnemyReachedToCastle);
        EventManager.StartListening(GameConstants.GameEvents.KILLED_MONSTER, RemoveKilledMonster);
        //EventManager.StartListening(GameConstants.GameEvents.CASTLE_DESTROYED, CastleDestroyed);
        EventManager.StartListening(GameConstants.LEVEL_EVENTS.LEVEL_SUCCESSED, AllEnemiesDied);
        EventManager.StartListening(GameConstants.GameEvents.MONSTER_REACHED_TO_CASTLE, CastleDamaged);
        EventManager.StartListening(GameConstants.GameEvents.RETURN_TO_TITLE_SCREEN, ReturnToMainMenu);
        EventManager.StartListening(GameConstants.SAVE_LOAD_EVENTS.MONSTERS_SAVED_TO_POOL,SaveData);
        
        EventManager.StartListening(GameConstants.SAVE_LOAD_EVENTS.LOAD_GAME_MANAGER_DATA,LoadData);

        
    }

    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.GameEvents.GAME_STARTED, StartGame);
        EventManager.StopListening(GameConstants.GameEvents.MONSTER_REACHED_TO_CASTLE, EnemyReachedToCastle);
        EventManager.StopListening(GameConstants.GameEvents.KILLED_MONSTER, RemoveKilledMonster);
        //EventManager.StopListening(GameConstants.GameEvents.CASTLE_DESTROYED, CastleDestroyed);
        EventManager.StopListening(GameConstants.LEVEL_EVENTS.LEVEL_SUCCESSED, AllEnemiesDied);
        EventManager.StopListening(GameConstants.GameEvents.MONSTER_REACHED_TO_CASTLE, CastleDamaged);
        EventManager.StopListening(GameConstants.GameEvents.RETURN_TO_TITLE_SCREEN, ReturnToMainMenu);
        EventManager.StopListening(GameConstants.SAVE_LOAD_EVENTS.MONSTERS_SAVED_TO_POOL, SaveData);

        EventManager.StopListening(GameConstants.SAVE_LOAD_EVENTS.LOAD_GAME_MANAGER_DATA, LoadData);
    }



    void SaveData(EventParam param)
    {
        GameData game_data = param.GetGameData();
        SaveData(ref game_data);
        EventManager.TriggerEvent(GameConstants.SAVE_LOAD_EVENTS.GAME_MANAGER_DATA_SAVED, param);
    }
    public void SaveData(ref GameData game_data)
    {
        GameManagerData game_manager_data = new GameManagerData();
        SaveGamaManagerData(game_manager_data);
        game_data.SetGameManagerData(game_manager_data);
       
    }


    void SaveGamaManagerData(GameManagerData game_manager_data)
    {


       
        game_manager_data.CurrentMonsterIndex = MonsterIndex;
        game_manager_data.CurrentGold = CurrentGold;
        game_manager_data.Timer = Timer;
       
        game_manager_data.KilledEnemyCount = KilledEnemyCount;
        game_manager_data.LoadMonsterIndex = LoadMonsterIndex;
        game_manager_data.WaveNumber = WaveNumber;
        game_manager_data.MonsterIndexes = MonsterIndexes;

        Dictionary<MonsterTypes.Type, int> all_monsters = game_manager_data.AllMonsters;
        Dictionary<MonsterTypes.Type, int> target_monsters = game_manager_data.TargetMonsters;
        List<MonsterData> monster_datas = game_manager_data.MonsterDatas;
        foreach(Monster monster in AllMonsters)
        {

            MonsterTypes.Type monster_type = monster.GetMonsterType();
            if (all_monsters.ContainsKey(monster_type))
            {
                all_monsters[monster_type] += 1;
            }
            else
            {
                all_monsters.Add(monster_type, 1);
            }



        }

        foreach (Monster monster in TargetMonsters)
        {

            MonsterTypes.Type monster_type = monster.GetMonsterType();
            if (target_monsters.ContainsKey(monster_type))
            {
                target_monsters[monster_type] += 1;
            }
            else
            {
                target_monsters.Add(monster_type, 1);
            }
            SaveMonstersData(monster, monster_datas);
            

        }
    }


    

    void SaveMonstersData(Monster monster, List<MonsterData> monster_datas)
    {
        MonsterData monster_data = new MonsterData();
        monster_data.Health = monster.GetHealth();
        monster_data.PosX = monster.transform.position.x;
        monster_data.PosY = monster.transform.position.y;
        monster_data.HealthImageAmount = monster.GetHealthBarImageAmount();
        monster_data.DestinationIndex = monster.GetDestinationIndex();
        monster_data.MovementTimer = monster.GetMovementTimer();
        monster.SaveDestionationPoints(monster_data);
        monster_datas.Add(monster_data);

    }
   
    void LoadData(EventParam param)
    {
        IsLoadedGame = true;
        GameData game_data = param.GetGameData();
        LoadData(game_data);
        EventManager.TriggerEvent(GameConstants.SAVE_LOAD_EVENTS.LOAD_DEFENDER_GENERATOR_DATA, param);
        
        
       
    }

    public void LoadData(GameData game_data)
    {
        GameManagerData game_manager_data = game_data.GetGameManagerData();
        LoadGameManagerData(game_manager_data);
    }

    void LoadGameManagerData(GameManagerData game_manager_data)
    {
        WaveNumber = game_manager_data.WaveNumber;
        GameLevel.UpdateMonstersForSurvivalMod((WaveNumber)*SurvivalModMonsterCountIncrease);
        GameLevel.GetMonstersFromPool();
        MonsterDestionationsTransform = GameLevel.GetMonsterDestionationsTransform();
        SetMonsterDestinations(MonsterDestionationsTransform);
        CurrentGold = game_manager_data.CurrentGold;
        KilledEnemyCount = game_manager_data.KilledEnemyCount;
        
        WaitForStartDuration = game_manager_data.Timer;
        MonsterIndex = game_manager_data.CurrentMonsterIndex;
        LoadTargetMonsters(game_manager_data);
        
    }

    void LoadTargetMonsters(GameManagerData game_manager_data)
    {
        int monster_data_index = 0;
        int temp_monster_index = 0;
        Dictionary<MonsterTypes.Type, int> target_monsters_dict = game_manager_data.TargetMonsters;
        List<MonsterData> monster_datas = game_manager_data.MonsterDatas;
        MonsterIndexes = game_manager_data.MonsterIndexes;
        foreach (MonsterTypes.Type monster_type in target_monsters_dict.Keys)
        {
            int count = target_monsters_dict[monster_type];
            for(int i = 0; i < count; i++)
            {
                int index = MonsterIndexes[temp_monster_index];
                Monster target_monster = AllMonsters[index];
                if (target_monster.GetMonsterType() != monster_type)
                {
                    for(int j= index; j < AllMonsters.Count; j++)
                    {
                        if (AllMonsters[j].GetMonsterType() == monster_type && !TargetMonsters.Contains(AllMonsters[j]))
                        {
                            
                            index = j;
                            target_monster = AllMonsters[index];


                            break;
                        }
                        
                    }
                }
                target_monster.gameObject.SetActive(true);
                MonsterData monster_data = monster_datas[monster_data_index];
                target_monster.LoadMonsterData(monster_data, MonsterDestionationList);
                target_monster.HealthBarSize = target_monster.GetHealth();
                Vector3[] destinations = LoadMonsterDestinations(monster_data);
                float timer = monster_data.MovementTimer;
                Sequence seq = DOTween.Sequence();
                target_monster.SetDestination(destinations[1]);
                
                seq.Append(target_monster.transform.DOPath(destinations, EnemyMovementTimer - timer, PathType, PathMode,gizmoColor: Color.red));
                target_monster.SetSequence(seq);
                temp_monster_index++;
                monster_data_index++;
                TargetMonsters.Add(target_monster);
            }
        }
    }



    Vector3[] LoadMonsterDestinations(MonsterData monster_data)
    {
        List<MonsterData.MonsterDestionationPoint> destinations_list = monster_data.MonsterDestionationPoints;
        int count = destinations_list.Count;
        Vector3[] destination_array = new Vector3[count];

        for(int i =0; i < count; i++)
        {
            float point_x = destinations_list[i].PointX;
            float point_y = destinations_list[i].PointY;
            Vector3 dest = new Vector3(point_x, point_y,0);
           
            destination_array[i] =dest;
        }

        return destination_array;
    }


    void StartGame(EventParam param)
    {
        CanPlay = true;
        
        
    }

    void CastleDamaged(EventParam param)
    {
        Monster monster = param.GetMonsterParam();

        EscapedEnemyCount++;
        if ((KilledEnemyCount + EscapedEnemyCount) == AllMonsters.Count && GameCastle.GetCastleHealth()>0)
        {
            if (!IsSurvivalMode)
            {
                EventManager.TriggerEvent(GameConstants.LEVEL_EVENTS.LEVEL_SUCCESSED, new EventParam());
            }
            
            
            
        }
    }

    void ReturnToMainMenu(EventParam param)
    {
        SendMonstersToPool();
        CanPlay = false;
    }

    void SendEnemies()
    {

        if (Timer == EnemySpawnTimer)
        {
            if (MonsterIndex < AllMonsters.Count)
            {
                MonsterIndexes.Add(MonsterIndex);
                Monster monster = AllMonsters[MonsterIndex];

                TargetMonsters.Add(monster);
                monster.NewHealth(WaveNumber);
                monster.transform.position = MonsterDestionationList[0].position;
                monster.gameObject.SetActive(true);
                monster.HealthBarSize = monster.GetHealth();
                monster.SetDestionations(MonsterDestionationList);
                Sequence seq = DOTween.Sequence();
                seq.Append(monster.transform.DOPath(Destinations, EnemyMovementTimer, PathType,PathMode));
                monster.SetSequence(seq);             
                MonsterIndex++;
                
                
            }
            
            if(MonsterIndex == AllMonsters.Count && IsSurvivalMode)
            {
                RefreshEnemiesForSurvivalMode();

            }

        }


        Timer -= Time.deltaTime;

        if (Timer < 0)
        {
            Timer = EnemySpawnTimer;
        }
    }
    void RemoveKilledMonster(EventParam param)
    {
        Monster killed_monster = param.GetKilledMonster();
        KilledEnemyCount++;
        LoadMonsterIndex++;
        if ((KilledEnemyCount + EscapedEnemyCount) == AllMonsters.Count && GameCastle.GetCastleHealth() > 0)
        {
            if (!IsSurvivalMode)
            {
                EventManager.TriggerEvent(GameConstants.LEVEL_EVENTS.LEVEL_SUCCESSED, new EventParam());
            }
            

        }
        else
        {
            CurrentGold += 10;
            killed_monster = param.GetKilledMonster();


        }
        RemoveMonsterFromList(killed_monster);

    }

    void AllEnemiesDied(EventParam param)
    {
        StartCoroutine(LevelSuccessCRT());
    }

    void CastleDestroyed(EventParam Param)
    {
        CanPlay = false;
        SendMonstersToPool();
        StartCoroutine(LevelFailedCRT());
    }


    IEnumerator LevelFailedCRT()
    {
        yield return new WaitForSeconds(.25f);
        LevelFailedScreen.SetActive(true);
        
    }

    IEnumerator LevelSuccessCRT()
    {
        yield return new WaitForSeconds(.25f);
        LevelSuccessedScreen.SetActive(true);
    }

    void RemoveMonsterFromList(Monster monster)
    {
        StartCoroutine(ResetMonsterCRT(monster));
        TargetMonsters.Remove(monster);
        MonsterIndexes.RemoveAt(0);
       
    }
    IEnumerator ResetMonsterCRT(Monster killed_monster)
    {
        yield return new WaitForSeconds(1f);
        killed_monster.SendMonsterToPool();
    }

    void EnemyReachedToCastle(EventParam param)
    {
        

        CanPlay = false;
        Failed = true;
        SendMonstersToPool();
        StartCoroutine(LevelFailedCRT());

    }



    void Start()
    {

        StartCoroutine(StartGameCRT());
        Timer = EnemySpawnTimer;
    }

    
    void Update()
    {
        if (CanPlay)
        {
            SendEnemies();
        }
    }

    IEnumerator StartGameCRT()
    {
        yield return new WaitForSeconds(WaitForStartDuration);
        if (!Failed)
        {
            EventManager.TriggerEvent(GameConstants.GameEvents.GAME_STARTED, new EventParam());
        }
        
    }


   


    void RefreshEnemiesForSurvivalMode()
    {

        MonsterIndex = 0;
        LoadMonsterIndex = MonsterIndexes[0];
      
        AllMonsters.Clear();
        
        
        if (EnemySpawnTimer > 0.75)
        {
            EnemySpawnTimer -= 0.25f;
        }
        
        Timer = EnemySpawnTimer;
        WaveNumber++;
        GameLevel.UpdateMonstersForSurvivalMod(WaveNumber* SurvivalModMonsterCountIncrease);
        EventManager.TriggerEvent(GameConstants.GameEvents.NEW_WAVE, new EventParam());
    }


    void SendMonstersToPool()
    {
        foreach (Monster monster in AllMonsters)
        {

            monster.SendMonsterToPool();

        }
    }


    
}

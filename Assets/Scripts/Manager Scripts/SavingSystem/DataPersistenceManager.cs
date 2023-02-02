using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using UnityEngine.UI;

public class DataPersistenceManager : MonoBehaviour
{
    private GameData GameData;
    public static DataPersistenceManager Instance { get; private set; }


    [Header("File Storage Config")]
    [SerializeField] private string SaveFileName;
    FileDataHandler FileDataHandler;

    [SerializeField] Button LoadSurvivalLevelButton;
    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.SAVE_LOAD_EVENTS.DEFENDER_GENERATOR_DATA_SAVED, SaveGame);
        EventManager.StartListening(GameConstants.SAVE_LOAD_EVENTS.START_LOADING_SAVED_GAME, StartLoadingGameData);
    }

    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.SAVE_LOAD_EVENTS.DEFENDER_GENERATOR_DATA_SAVED, SaveGame);
        EventManager.StopListening(GameConstants.SAVE_LOAD_EVENTS.START_LOADING_SAVED_GAME, StartLoadingGameData);
    }


    void StartLoadingGameData(EventParam param)
    {
        
        EventParam new_param = new EventParam(GameData);
        EventManager.TriggerEvent(GameConstants.SAVE_LOAD_EVENTS.LOAD_POOL_DATA, new_param);
    }

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("Found more than one Data Persistance Manager");
        }
        Instance = this;
    }

    private void Start()
    {
        FileDataHandler = new FileDataHandler(Application.persistentDataPath, SaveFileName);
        LoadGame();
    }


    public void NewGame()
    {
        GameData = new GameData();
    }

    public void LoadGame()
    {
        this.GameData = FileDataHandler.LoadSaveData();
        if(GameData == null)
        {
            NewGame();
        }

        else
        {
           
            GameManagerData gameManagerData = GameData.GetGameManagerData();
            
            LoadSurvivalLevelButton.interactable = true;
            DefenderGeneratorData defenderGeneratorData = GameData.GetDefenderGeneratorData();
            Debug.Log("SaveData found");
            
        }
    }

    public void SaveGame(EventParam param)
    {
        GameData = param.GetGameData();
        
        FileDataHandler.SaveGameData(GameData);
        Debug.Log("SavingCompleted");
        Time.timeScale = 1;

        
    }
}

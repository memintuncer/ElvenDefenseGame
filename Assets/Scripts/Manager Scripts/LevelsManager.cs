using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{

    [SerializeField] private List<GameLevel> AllGameLevels = new List<GameLevel>();
    [SerializeField] private List<LevelButton> AllLevelButtons = new List<LevelButton>();
    [SerializeField] GameObject LevelScreen,GameScreen;
    [SerializeField] private Transform LevelButtonsParent;
    [SerializeField] GameObject SurvivalLevelPrefab;

    public int CurrentLevelIndex;
    private GameObject CurrentLevel = null;
    bool IsSurvivalMod;

    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.LEVEL_EVENTS.SAME_LEVEL, RestartLevel);
        EventManager.StartListening(GameConstants.LEVEL_EVENTS.NEXT_LEVEL, NextLevel);
        EventManager.StartListening(GameConstants.LEVEL_EVENTS.SPESIFIC_LEVEL, LoadSpecificLevel);
        EventManager.StartListening(GameConstants.GameEvents.RETURN_TO_TITLE_SCREEN, ReturnToMainMenu);
        EventManager.StartListening(GameConstants.SAVE_LOAD_EVENTS.LOAD_SURVIVAL_LEVEL, LoadSurvivalLevel);

        
    }

    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.LEVEL_EVENTS.SAME_LEVEL, RestartLevel);
        EventManager.StopListening(GameConstants.LEVEL_EVENTS.NEXT_LEVEL, NextLevel);
        EventManager.StopListening(GameConstants.LEVEL_EVENTS.SPESIFIC_LEVEL, LoadSpecificLevel);
        EventManager.StopListening(GameConstants.GameEvents.RETURN_TO_TITLE_SCREEN, ReturnToMainMenu);
        EventManager.StopListening(GameConstants.SAVE_LOAD_EVENTS.LOAD_SURVIVAL_LEVEL, LoadSurvivalLevel);
    }

    void ReturnToMainMenu(EventParam param)
    {
        
        
        LevelScreen.SetActive(true);
        StartCoroutine(DestroyCurrentLevelCRT());
    }


    IEnumerator DestroyCurrentLevelCRT()
    {
        yield return new WaitForSeconds(1f);
        Destroy(CurrentLevel);
    }

    void LoadSpecificLevel(EventParam param)
    {
        int level_index = param.GetSpecificLevelIndex();
        
        LoadLevel(level_index);
    }

    void RestartLevel(EventParam param)
    {
        RestartLevel();
    }


    void LoadSurvivalLevel(EventParam param)
    {
        LoadSurvivalLevel();
        EventManager.TriggerEvent(GameConstants.SAVE_LOAD_EVENTS.LOAD_GAME_MANAGER_DATA, param);
    }

    void NextLevel(EventParam param)
    {
        
        Destroy(CurrentLevel);
        CurrentLevelIndex++;
        if (CurrentLevelIndex >= AllGameLevels.Count)
        {
            CurrentLevelIndex = AllGameLevels.Count - 1;
        }
       
            
        PlayerPrefs.SetInt("LevelNumber", CurrentLevelIndex);
      
        LoadLevel();
    }

    public void LoadLevel()
    {
        IsSurvivalMod = false;
        CurrentLevelIndex = PlayerPrefs.GetInt("LevelNumber");
        GameObject game_level = Instantiate(AllGameLevels[CurrentLevelIndex].gameObject, Vector2.zero, Quaternion.identity);
        GameLevel game_level_script = game_level.GetComponent<GameLevel>();
        game_level_script.SetLevelIndex(CurrentLevelIndex+1);
        CurrentLevel = game_level;
        game_level.transform.parent = GameScreen.transform;
        LevelScreen.SetActive(false);
    }
    public void LoadLevel(int index)
    {
        GameObject game_level = Instantiate(AllGameLevels[index].gameObject, Vector2.zero, Quaternion.identity);
        GameLevel game_level_script = game_level.GetComponent<GameLevel>();
        game_level_script.SetLevelIndex(index + 1);
        CurrentLevel = game_level;
        game_level.transform.parent = GameScreen.transform;
        LevelScreen.SetActive(false);
    }

    public void RestartLevel()
    {
        Destroy(CurrentLevel);
        
       

        if (IsSurvivalMod)
        {
            LoadSurvivalLevel();
        }

        else
        {
            LoadLevel();
        }
        Time.timeScale = 1;
    }

    private void Awake()
    {
       
        CheckFirstGame();
        GetLevelButtons();
    }

    void CheckFirstGame()
    {
        
        int first_time_play = PlayerPrefs.GetInt("FirstTimePlay");
        if (first_time_play == 0)
        {
            PlayerPrefs.SetInt("LevelNumber", 0);
            CurrentLevelIndex = 0;
            first_time_play = 1;
            PlayerPrefs.SetInt("FirstTimePlay", 1);
            
        }

        else
        {
            
            CurrentLevelIndex = PlayerPrefs.GetInt("LevelNumber");
            
        }
    }

    void GetLevelButtons()
    {
        for(int i = 0; i < LevelButtonsParent.childCount; i++)
        {
            LevelButton level_button = LevelButtonsParent.GetChild(i).GetComponent<LevelButton>();
            AllLevelButtons.Add(level_button);
        }
        SetLevelButtons();
    }
    void SetLevelButtons()
    {
        for(int i =0; i < CurrentLevelIndex + 1; i++)
        {
            LevelButton level_button = AllLevelButtons[i];
            level_button.SetInteractable(true);
        }
    }

    public void NextLevel()
    {
        Destroy(CurrentLevel);
        CurrentLevelIndex = PlayerPrefs.GetInt("LevelNumber");
        if (CurrentLevelIndex > AllGameLevels.Count)
        {
            PlayerPrefs.SetInt("LevelNumber", AllGameLevels.Count-1);
        }

        LoadLevel();
    }



    public void LoadSurvivalLevel()
    {
        IsSurvivalMod = true;
        GameObject survival_level = Instantiate(SurvivalLevelPrefab, Vector2.zero, Quaternion.identity);
        survival_level.transform.parent = GameScreen.transform;
        CurrentLevel = survival_level;
    }
}

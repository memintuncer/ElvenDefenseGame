using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface DataPersistenceInterface

{

    

    public void LoadData(GameData game_data);

    public void SaveData(ref GameData game_data);


}

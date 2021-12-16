using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public static class SaveSystem 
{
    private static string _saveFilepath = "Saves";
    private static string _saveFileExtension = ".json";
    private static string _saveFileName = "save";

    //Check data path, and return the SaveData type object from parsing
    public static async Task<SaveData> LoadData()
    {
        string directoryPath = Path.Combine(Application.persistentDataPath, _saveFilepath);
        string filePath = Path.Combine(directoryPath, _saveFileName + _saveFileExtension);

        SaveData dataToLoad;
            
        if (File.Exists(filePath))
        {
            dataToLoad = await ReadData(filePath);
        }
        else
        {
            dataToLoad = new SaveData();
        }

        return dataToLoad;
    }

    //Get Bytes array from file, make a string from it, and then parse it as a json int a SaveData type
    public static async Task<SaveData> ReadData(string filePath)
    {
        SaveData newSaveData;
        byte[] bytesArray;
        
        using (FileStream filestream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            bytesArray= new byte[filestream.Length];
            
            await filestream.ReadAsync(bytesArray, 0, bytesArray.Length);
            
        }

        newSaveData = await Task.Run(() =>
        {
            string fileString = Encoding.Unicode.GetString(bytesArray);
            return JsonUtility.FromJson<SaveData>(fileString);
        });
        
        return newSaveData;
    }

    //Check if there's a file correponding to the path
    public static bool CheckSaveFile()
    {
        string directoryPath = Path.Combine(Application.persistentDataPath, _saveFilepath);
        string filePath = Path.Combine(directoryPath, _saveFileName + _saveFileExtension);

        return File.Exists(filePath);
    }
    
    
    public static async void SaveData()
    {
        SaveData data = GetData();
        await WriteData(data);
    }
    
    //Get all the necessary infos from Managers singleton and store them in the SaveData struct
    public static SaveData GetData()
    {
        List<PlayerData> tempPlayersDatas = new List<PlayerData>();
        List<TileData> tempTilesDatas = new List<TileData>();
        
        //We set the tempPlayersData with all the actives players in PlayerManager
        foreach (Player player in PlayerManager.Instance._listPlayers)
        {
            PlayerData tempPlayerData;
            
            tempPlayerData._playerName = player._name;
            tempPlayerData._playerHealth = player._currentHealth;
            tempPlayerData._durationOfActiveBurningDebuff = new List<int>();
            tempPlayerData._durationOfActiveFreezeDebuff = new List<int>();

            if (player._debuffList.Count > 0)
            {
                foreach (var debuff in player._debuffList)
                {
                    switch (debuff)
                    {
                        case BurningDebuff burningDebuff:
                            tempPlayerData._durationOfActiveBurningDebuff.Add(burningDebuff._duration);
                            break;
                        
                        case FreezeDebuff freezeDebuff:
                            tempPlayerData._durationOfActiveFreezeDebuff.Add(freezeDebuff._duration);
                            break;
                    }
                }
            }

            tempPlayerData._playerRotation = player._playerMovementComponent.RotationOfPlayer;
            tempPlayerData._playerTile = player._currentTile.GetCoord();
            tempPlayerData._playerChoices = player._choicesMade;
            
            tempPlayerData._materialIndex =
                PlayerManager.Instance._skinSystem.GetMaterialIndex(player._skinnedMeshComponent.material);
            tempPlayerData._skinnedMeshIndex =
                PlayerManager.Instance._skinSystem.GetSkinIndex(player._skinnedMeshComponent.sharedMesh);
            
            tempPlayersDatas.Add(tempPlayerData);
        }

        //We set the tempTilesDatas with all the actives tiles in BoardManager
        foreach (Tile tile in BoardManager.Instance._listTiles)
        {
            TileData tempTileData;

            tempTileData._tileTransform = new TransformData(tile.transform);
            tempTileData._tileCoords = tile.GetCoord();
            tempTileData._hasObstacle = tile.hasObstacle;
            tempTileData._tileTrapCount = tile.trapList.Count;
            
            tempTilesDatas.Add(tempTileData);
        }
        
        SaveData data = new SaveData()
        {
            _currentTurn = GlobalManager.Instance.GetCurrentTurn(),
            _playersDatas = tempPlayersDatas,
            _tilesDatas = tempTilesDatas
        };

        return data;
    }

    //Collect data from active in-game objects, store them in a new object and write it as a json
    private static async Task WriteData(SaveData data)
    {
        string directoryPath = Path.Combine(Application.persistentDataPath, _saveFilepath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string filePath = Path.Combine(directoryPath, _saveFileName + _saveFileExtension);

        byte[] bytes = await Task.Run(() =>
        {
            string jsonData = JsonUtility.ToJson(data);
            return Encoding.Unicode.GetBytes(jsonData);
        });

        using (FileStream filestream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write))
        {
            await filestream.WriteAsync(bytes, 0, bytes.Length);
        }

    }

    

}
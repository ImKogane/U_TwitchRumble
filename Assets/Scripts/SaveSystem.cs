using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

public static class SaveSystem 
{

    [SerializeField] private static string _saveFilepath = "Saves";
    [SerializeField] private static string _saveFileExtension = ".json";
    [SerializeField] private static string _saveFileName = "save";


    public static void LoadData(SaveData data)
    {
        
    }
    
    public static void SaveData()
    {
        SaveData data = GetData();
        
        WriteData(data);
    }
    
    public static SaveData GetData()
    {
        List<PlayerData> tempPlayersDatas = new List<PlayerData>();
        List<TileData> tempTilesDatas = new List<TileData>();
        
        
        foreach (Player player in PlayerManager.Instance.PlayerList)
        {
            PlayerData tempPlayerData;
            
            tempPlayerData._playerName = player.namePlayer;
            tempPlayerData._playerHealth = player._playerLife;
            tempPlayerData._playerDebuffs = player.debuffList;
            tempPlayerData._playerTile = player.CurrentTile.GetCoord();
            if (player.playerWeapon != null)
            {
                tempPlayerData._playerWeapon = player.playerWeapon;
            }
            else
            {
                tempPlayerData._playerWeapon = null;
            }

            if (player.playerMoveBuff != null)
            {
                tempPlayerData._playerMovementBuff = player.playerMoveBuff;
            }
            else
            {
                tempPlayerData._playerMovementBuff = null;
            }

            if (player.playerWeaponBuff != null)
            {
                tempPlayerData._playerWeaponBuff = player.playerWeaponBuff;
            }
            else
            {
                tempPlayerData._playerWeaponBuff = null;
            }
            
            tempPlayerData._materialIndex =
                PlayerManager.Instance.SkinSystem.GetMaterialIndex(player.playerModel.material);
            tempPlayerData._skinnedMeshIndex =
                PlayerManager.Instance.SkinSystem.GetSkinIndex(player.playerModel.sharedMesh);
            
            tempPlayersDatas.Add(tempPlayerData);
        }

        foreach (Tile tile in BoardManager.Instance.tilesList)
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

    private static void WriteData(SaveData data)
    {
        string directoryPath = Path.Combine(Application.persistentDataPath, _saveFilepath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string filePath = Path.Combine(directoryPath, _saveFileName + _saveFileExtension);

        string jsonData = JsonUtility.ToJson(data);
        
        File.WriteAllText(filePath, jsonData);
        
        
        /*byte[] bytes = await Task.Run(() =>
        {
            string jsonData = JsonUtility.ToJson(data);
            return Encoding.Unicode.GetBytes(jsonData);
        });*/
        
        
        
        
        //File.WriteAllText(filePath, jsonData);

        /*using (FileStream filestream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write))
        {
            await filestream.WriteAsync(bytes, 0, bytes.Length);
        }*/

    }

}
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

public class SaveSystem 
{
    /*OLD SYSTEM
    public static void SaveDatas(object item, string dest)
    {
        string path = Path.Combine(Application.persistentDataPath, dest + ".xml");
        XmlSerializer serializer = new XmlSerializer(item.GetType());
        StreamWriter writer = new StreamWriter(path);
        serializer.Serialize(writer.BaseStream, item);
        writer.Close();
        Debug.Log("New save at :" + Application.persistentDataPath + "/" + dest + ".xml");
    }

    public static T LoadDatas<T>(string dest)
    {
        string path = Path.Combine(Application.persistentDataPath, dest);
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        StreamReader reader = new StreamReader(path);
        T deserialized = (T)serializer.Deserialize(reader.BaseStream);
        reader.Close();
        return deserialized;
    }
    */

    [SerializeField] private string _saveFilepath;
    [SerializeField] private string _saveFileExtension;
    [SerializeField] private string _saveFileName;


    public async void LoadData(SaveData data)
    {
        
    }
    
    public async void SaveData()
    {
        //SaveData data = StartCoroutine(GetDataCoroutine());
        
        //await WriteData(data);
        
    }
    
    public IEnumerator<SaveData> GetDataCoroutine()
    {
        
        
        
        
        SaveData data = new SaveData()
        {
            _currentGameState = GlobalManager.Instance.GetCurrentGameState(),
            _currentTurn = GlobalManager.Instance.GetCurrentTurn(),
            _tilesCoords = BoardManager.Instance.GetAllTilesCoords(),
            
        };

        return null;

    }

    private async Task WriteData(SaveData data)
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
        
        
        //File.WriteAllText(filePath, jsonData);

        using (FileStream filestream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write))
        {
            await filestream.WriteAsync(bytes, 0, bytes.Length);
        }

    }
    
    /*
    public void BuildNewSave()
    {
        SaveData newSave = new SaveData();
        newSave._currentTurn = turnCount;
        newSave._currentGameState = currentGameState;

        for(int i = 0; i < PlayerManager.Instance.PlayerList.Count; i++)
        {
            Player currentPlayer = PlayerManager.Instance.PlayerList[i];
            newSave._playerNames.Add(currentPlayer.namePlayer);
            newSave._playerHealth.Add(currentPlayer._playerLife);
            
            if(currentPlayer.playerWeapon != null) newSave._playerWeapons.Add(currentPlayer.playerWeapon.weaponType);
            if(currentPlayer.playerWeaponBuff != null) newSave._playerWeaponBuffs.Add(currentPlayer.playerWeaponBuff.weaponBuffType);
            if(currentPlayer.playerMoveBuff != null) newSave._playerMovementBuffs.Add(currentPlayer.playerMoveBuff.movementBuffType);
            newSave._playerTiles.Add(new Vector2Int(currentPlayer.CurrentTile.tileRow, currentPlayer.CurrentTile.tileColumn));
        }

        for(int i = 0; i < BoardManager.Instance.tilesList.Count; i++)
        {
            Tile currentTile = BoardManager.Instance.tilesList[i];
            newSave._tilesCoords.Add(new Vector2Int(currentTile.tileRow, currentTile.tileColumn));
            newSave._tilesPositions.Add(currentTile.transform.position);
        }
        
    }
    
    //Game values
    public int _currentTurn;
    public EnumClass.GameState _currentGameState;
    
    //Board values
    public List<Vector2Int> _tilesCoords = new List<Vector2Int>();
    public List<Vector3> _tilesPositions = new List<Vector3>();

    //Players values
    public List<string> _playerNames = new List<string>();
    public List<Vector2Int> _playerTiles = new List<Vector2Int>();
    public List<int> _playerHealth = new List<int>();
    public List<EnumClass.WeaponType> _playerWeapons = new List<EnumClass.WeaponType>();
    public List<EnumClass.WeaponBuffType> _playerWeaponBuffs = new List<EnumClass.WeaponBuffType>();
    public List<EnumClass.MovementBuffType> _playerMovementBuffs = new List<EnumClass.MovementBuffType>();
    */
    
}
using System.Collections.Generic;
using UnityEngine;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using System;
using System.IO;
using System.Threading.Tasks;

public class GoogleSheetManager : SingletonMonobehaviour<GoogleSheetManager>
{
    public override bool DestroyOnLoad => false;

    private string _spreadSheetID = "1stc2jb4ra-qVqIeG2AlY8yNqKGgHYvGI3ImMrKeryic";
    private string _jsonPath = "twitchrumble-abb198867ed9.json";

    private List<string> _allLettersList = new List<string>() {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
    
    private int _currentColumn = 0;
    private int _currentLine = 0;

    private SheetsService _service;

    public List<int> _variablesGetFromSheet = new List<int>();

    #region Connect To Google
    private async void Start()
    {
        ConnectToGoogleAPI();
        var cellsFull = await GetSheetRange("Stats!A:A");
        GetValidCurrentLine(cellsFull);
        GetVariablesOfGame();
    }

    public void ConnectToGoogleAPI()
    {
        String fullJsonPath = Application.streamingAssetsPath + "/" + _jsonPath;

        Stream jsonCreds = (Stream)File.Open(fullJsonPath, FileMode.Open);

        ServiceAccountCredential credential = ServiceAccountCredential.FromServiceAccountData(jsonCreds);

        _service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
        });
    }

    #endregion

    #region Set Datas In Google Sheet
    public async void StartGoogleSheetSaving()
    {
        await SetDateTimeInfosInSheet();
        await SetPlayersListToSheet();
    }

    public async Task SetDateTimeInfosInSheet()
    {
        List<string> datasToStore = new List<string>();
        datasToStore.Add(DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString());
        datasToStore.Add(DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString());
        datasToStore.Add(DateTime.Now.Year.ToString());

        await SetACell("Datas of the game :", 0, _currentLine);


        foreach (string info in datasToStore)
        {
            await SetACell(info, _currentColumn, _currentLine);
        }
    }
    public async Task SetPlayersListToSheet()
    {
        List<string> playersName = PlayerManager.Instance._listPlayersNames;

        Debug.Log(_currentColumn);
        await SetACell(playersName.Count.ToString(), _currentColumn, _currentLine);


        await SetACell("Noms des joueurs :", 0, _currentLine + 1);

        for (int i = 0; i < playersName.Count; i++)
        {
            await SetACell(playersName[i], i + 1, _currentLine + 1);
        }
    }

    #endregion

    #region Get Datas Of Google Sheet
    public async void GetVariablesOfGame()
    {
        IList<IList<object>> listOfDatas = await GetSheetRange("Params!2:2");

        foreach (var item in listOfDatas)
        {
            for (int i = 0; i < item.Count; i++)
            {
                //VariablesGetFromSheet.Add((int)item[i]);
                Debug.Log("Datas get from sheet : " + item[i]);
                _variablesGetFromSheet.Add(Convert.ToInt32(item[i]));
            }
        }
    }

    public async Task<IList<IList<object>>> GetSheetRange(String sheetNameAndRange)
    {
        SpreadsheetsResource.ValuesResource.GetRequest request = _service.Spreadsheets.Values.Get(_spreadSheetID, sheetNameAndRange);

        ValueRange response = await request.ExecuteAsync();

        IList<IList<object>> values = response.Values;

        if (values != null && values.Count > 0)
        {
            return values;
        }
        else
        {
            Debug.Log("No data found.");
            return null;
        }
    }

    public void GetValidCurrentLine(IList<IList<object>> values)
    {
        foreach (var item in values)
        {
            for (int i = 0; i < item.Count; i++)
            {
                _currentLine ++;
                if (i%2 == 0)
                {
                    _currentLine++;
                }
                Debug.Log(_currentLine);
            }
        }
        _currentLine ++;
        Debug.Log(_currentLine);
    }

    public async Task SetACell(string DataToSave, int column, int line)
    {
        string nameOfPage = "Stats!"+_allLettersList[column] + line.ToString();
        Debug.Log("CellToStoreDatas :" + nameOfPage);

        ValueRange values = new ValueRange();

        List<object> oblist = new List<object>() { DataToSave };
        values.Values = new List<IList<object>> { oblist };

        SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum valueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;

        SpreadsheetsResource.ValuesResource.UpdateRequest request = _service.Spreadsheets.Values.Update(values, _spreadSheetID, nameOfPage);
        request.ValueInputOption = valueInputOption;

        _currentColumn++;

        await request.ExecuteAsync();
    }

    #endregion
}

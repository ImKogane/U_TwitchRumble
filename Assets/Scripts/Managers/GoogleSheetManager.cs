using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;
using System.Linq;
using System.Dynamic;
using System.Threading.Tasks;

public class GoogleSheetManager : SingletonMonobehaviour<GoogleSheetManager>
{
    public override bool DestroyOnLoad => false;

    private string _spreadSheetID = "1stc2jb4ra-qVqIeG2AlY8yNqKGgHYvGI3ImMrKeryic";
    private string _jsonPath = "twitchrumble-abb198867ed9.json";

    private List<string> AllLetters= new List<string>() {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
    
    private int currentColumn = 0;
    private int currentLine = 0;

    private SheetsService service;

    public List<int> VariablesGetFromSheet = new List<int>();

    private async void Start()
    {
        ConnectToGoogleAPI();
        var cellsFull = await GetSheetRange("Stats!A:A");
        SetValidCurrentLine(cellsFull);
        GetVariablesOfGame();
    }

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

        await SetACell("Datas of the game :", 0, currentLine);


        foreach (string info in datasToStore)
        {
            await SetACell(info, currentColumn, currentLine);
        }
    }

    public async void GetVariablesOfGame()
    {
        IList<IList<object>> listOfDatas = await GetSheetRange("Params!2:2");

        foreach (var item in listOfDatas)
        {
            for (int i = 0; i < item.Count; i++)
            {
                //VariablesGetFromSheet.Add((int)item[i]);
                Debug.Log("Datas get from sheet : " + item[i]);
                VariablesGetFromSheet.Add(Convert.ToInt32(item[i]));
            }
        }
    }

    public async Task SetPlayersListToSheet()
    {
        List<string> playersName = PlayerManager.Instance.AllPlayersName;

        Debug.Log(currentColumn);
        await SetACell(playersName.Count.ToString(), currentColumn, currentLine);


        await SetACell ("Noms des joueurs :", 0, currentLine + 1);

        for (int i = 0; i < playersName.Count; i++)
        {
            await SetACell(playersName[i], i + 1, currentLine + 1);
        }
    }

    public void ConnectToGoogleAPI()
    {
        String fullJsonPath = Application.streamingAssetsPath + "/" + _jsonPath;

        Stream jsonCreds = (Stream)File.Open(fullJsonPath, FileMode.Open);

        ServiceAccountCredential credential = ServiceAccountCredential.FromServiceAccountData(jsonCreds);

        service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
        });
    }

    #region OwnFunctions

    public async Task<IList<IList<object>>> GetSheetRange(String sheetNameAndRange)
    {
        SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(_spreadSheetID, sheetNameAndRange);

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

    public void SetValidCurrentLine(IList<IList<object>> values)
    {
        foreach (var item in values)
        {
            for (int i = 0; i < item.Count; i++)
            {
                currentLine ++;
                if (i%2 == 0)
                {
                    currentLine++;
                }
                Debug.Log(currentLine);
            }
        }
        currentLine ++;
        Debug.Log(currentLine);
    }

    public async Task SetACell(string DataToSave, int column, int line)
    {
        string nameOfPage = "Stats!"+AllLetters[column] + line.ToString();
        Debug.Log("CellToStoreDatas :" + nameOfPage);

        ValueRange values = new ValueRange();

        List<object> oblist = new List<object>() { DataToSave };
        values.Values = new List<IList<object>> { oblist };

        SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum valueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;

        SpreadsheetsResource.ValuesResource.UpdateRequest request = service.Spreadsheets.Values.Update(values, _spreadSheetID, nameOfPage);
        request.ValueInputOption = valueInputOption;

        currentColumn++;

        await request.ExecuteAsync();
    }

    #endregion

    
}

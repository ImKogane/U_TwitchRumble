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


public class GoogleSheetManager : SingletonMonobehaviour<GoogleSheetManager>
{
    public override bool DestroyOnLoad => false;

    private string _spreadSheetID = "1stc2jb4ra-qVqIeG2AlY8yNqKGgHYvGI3ImMrKeryic";
    private string _jsonPath = "/StreamingAssetsCredentials/twitchrumble-abb198867ed9.json";

    private List<string> AllLetters= new List<string>() {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
    
    private int currentColumn = 0;
    private int currentLine = 0;

    private SheetsService service;

    private void Start()
    {
        ConnectToGoogleAPI();
        SetValidCurrentLine(GetSheetRange("Stats!A:A"));
        SetDateTimeInfosInSheet();
    }

    public void LaunchConnexionToGoogle()
    {

    }

    public void SetDateTimeInfosInSheet()
    {
        List<string> datasToStore = new List<string>();
        datasToStore.Add(DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString());
        datasToStore.Add(DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString());
        datasToStore.Add(DateTime.Now.Year.ToString());

        foreach (string info in datasToStore)
        {
            SetACell(info);
        }
    }

    public void ConnectToGoogleAPI()
    {
        String fullJsonPath = Application.dataPath + _jsonPath;

        Stream jsonCreds = (Stream)File.Open(fullJsonPath, FileMode.Open);

        ServiceAccountCredential credential = ServiceAccountCredential.FromServiceAccountData(jsonCreds);

        service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
        });
    }

    #region OwnFunctions

    public IList<IList<object>> GetSheetRange(String sheetNameAndRange)
    {
        SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(_spreadSheetID, sheetNameAndRange);

        ValueRange response = request.Execute();

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
                Debug.Log(currentLine);
            }
        }
        currentLine++;
        Debug.Log(currentLine);
    }

    public async void SetACell(string DataToSave)
    {
        //string nameOfPage = "A1";
        string nameOfPage = "Stats!"+AllLetters[currentColumn] + currentLine.ToString();
        Debug.Log("CellToStoreDatas :" + nameOfPage);

        ValueRange values = new ValueRange();
        //values.MajorDimension = "COLUMNS";

        List<object> oblist = new List<object>() { DataToSave };
        values.Values = new List<IList<object>> { oblist };

        SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum valueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;

        SpreadsheetsResource.ValuesResource.UpdateRequest request = service.Spreadsheets.Values.Update(values, _spreadSheetID, nameOfPage);
        request.ValueInputOption = valueInputOption;

        request.Execute();

        currentColumn++;
    }

    public void SetSheetRange()
    {
        IList<IList<object>> values = new List<IList<object>> { new List<object> { "Coucou 1 !", "Coucou 2 !" } };

        var valuesRange = new ValueRange();

        valuesRange.Range = "A1A2";
        valuesRange.Values = values;

        var batchUpdateRequest = new BatchUpdateValuesRequest();

        IList<ValueRange> listValueRange = new List<ValueRange> { valuesRange };
        batchUpdateRequest.Data = listValueRange;
        batchUpdateRequest.ValueInputOption = "RAW";

        SpreadsheetsResource.ValuesResource.BatchUpdateRequest request = service.Spreadsheets.Values.BatchUpdate(batchUpdateRequest, _spreadSheetID);

        request.Execute();
    }

    #endregion

    
}

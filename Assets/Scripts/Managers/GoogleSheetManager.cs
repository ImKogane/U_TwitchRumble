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


public class GoogleSheetManager : SingletonMonobehaviour<GoogleSheetManager>
{
    public override bool DestroyOnLoad => false;

    private string _spreadSheetID = "1stc2jb4ra-qVqIeG2AlY8yNqKGgHYvGI3ImMrKeryic";
    private string _jsonPath = "/StreamingAssetsCredentials/twitchrumble-abb198867ed9.json";

    private SheetsService service;

    private void Start()
    {
        SheetReader();
        printAllValues(GetSheetRange("A1:J10"));
        SetSheetRange();
    }

    public void SheetReader()
    {
        String fullJsonPath = Application.dataPath + _jsonPath;

        Stream jsonCreds = (Stream)File.Open(fullJsonPath, FileMode.Open);

        ServiceAccountCredential credential = ServiceAccountCredential.FromServiceAccountData(jsonCreds);

        service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
        });
    }

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

    public void printAllValues(IList<IList<object>> values)
    {
        foreach (var item in values)
        {
            foreach (var x in item)
            {
                Debug.Log(x);
            }
        }
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
    }
}

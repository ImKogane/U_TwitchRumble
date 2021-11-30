using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Settings : MonoBehaviour
{
    [SerializeField] private GameObject SettingsCanvas;
    [SerializeField] private GameObject ConnectionCanvas;

    public void Back()
    {
        SettingsCanvas.SetActive(false);
    }
    
    public void OpenConnection()
    {
        ConnectionCanvas.SetActive(true);
        SettingsCanvas.SetActive(false);
    } }

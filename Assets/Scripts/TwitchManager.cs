using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using UnityEngine.UI;
using TMPro;

public class TwitchManager : MonoBehaviour
{
    public TMP_InputField PasswordInput;
    public TMP_InputField UserNameInput;
    public TMP_InputField ChannelNameInput;

    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writter;

    bool makeUpdateTest = false;

    public void Connect() //A lancer avec le boutton connect.
    {
        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        reader = new StreamReader(twitchClient.GetStream());
        writter = new StreamWriter(twitchClient.GetStream());

        writter.WriteLine("PASS " + PasswordInput.text);
        writter.WriteLine("NICK " + UserNameInput.text);
        writter.WriteLine("USER " + UserNameInput.text + " 8 * :" + UserNameInput.text);
        writter.WriteLine("JOIN #" + ChannelNameInput.text);
        writter.Flush();

        makeUpdateTest = true;
    }

    private void ReadChat()
    {
        if (twitchClient.Available > 0)
        {
            var message = reader.ReadLine();
            Debug.Log(message);
        }
    }

    private void Update()
    {
        if (makeUpdateTest && (!twitchClient.Connected))
        {
            Connect();
        }

        if (makeUpdateTest)
        {
            ReadChat();
        }
    }
}

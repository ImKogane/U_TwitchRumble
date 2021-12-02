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
    public static TwitchManager Instance;

    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writter;

    [Header("Reference UI Connexion")]
    public TMP_InputField PasswordInput;
    public TMP_InputField ChannelNameInput;
    public GameObject PanelConnexion;

    

    [Header("Commande of the game")]
    public string AttackCommande;
    public string JoinCommande;
    public string QuitCommande;
    public string LeftCommande;
    public string RightCommande;
    public string UpCommande;
    public string DownCommande;
    public string ChoiceCommande = "!card";
    
    


    public int maxCharacterInNames;

    bool bConnexionIsDone = false;
    [System.NonSerialized] public bool canJoinedGame = false;
    [System.NonSerialized] public bool playersCanMakeActions = false;
    [System.NonSerialized] public bool playersCanMakeChoices = true;
    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start() // Use datas store in PlayerPrefs
    {
        //PanelConnexion.SetActive(true);
        if (PlayerPrefs.HasKey("PasswordInput")) { PasswordInput.text = PlayerPrefs.GetString("PasswordInput"); }
        if (PlayerPrefs.HasKey("ChannelNameInput")) { ChannelNameInput.text = PlayerPrefs.GetString("ChannelNameInput"); }
    }

    public void Connect() //A lancer avec le boutton connect.
    {
        //Store datas of connexion in PlayerPrefs
        PlayerPrefs.SetString("PasswordInput", PasswordInput.text);
        PlayerPrefs.SetString("ChannelNameInput", ChannelNameInput.text.ToLower());

        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        reader = new StreamReader(twitchClient.GetStream());
        writter = new StreamWriter(twitchClient.GetStream());

        writter.WriteLine("PASS " + PasswordInput.text);
        writter.WriteLine("NICK " + ChannelNameInput.text.ToLower());
        writter.WriteLine("USER " + ChannelNameInput.text.ToLower() + " 8 * :" + ChannelNameInput.text.ToLower());
        writter.WriteLine("JOIN #" + ChannelNameInput.text.ToLower());
        writter.Flush();

        if (twitchClient.Connected)
        {
            bConnexionIsDone = true;
        }
    }

    public bool GetConnexionIsDone()
    {
        return bConnexionIsDone;
    }

    private void ReadChat()
    {
        if (twitchClient.Available > 0)
        {
            var message = reader.ReadLine();

            if (message.Contains("Your host is"))
            {
                Debug.Log("CONNEXION ? " + message);
                PanelConnexion.SetActive(false);
                //PanelLobby.SetActive(true);
            }

            if (message.Contains("PRIVMSG"))
            {
                //Get the USER of the message.
                int splitPointU = message.IndexOf("!", 1);
                string NameOfUser = message.Substring(0, splitPointU);
                NameOfUser = NameOfUser.Substring(1);

                //Get the MESSAGE.
                int splitPointM = message.IndexOf(":", 1);
                string messageOfUser = message.Substring(splitPointM + 1);

                Debug.Log("MESSAGE of " + NameOfUser + " : " + messageOfUser);

                AnalyseChatCommand(NameOfUser, messageOfUser);
            }
            else
            {
                Debug.Log("DEBUG : " + message);
            }
        }
    }

    private void Update()
    {
        if (bConnexionIsDone && (!twitchClient.Connected))
        {
            Connect();
        }

        if (bConnexionIsDone)
        {
            ReadChat();
        }
        
    }

    public void AnalyseChatCommand(string nameOfPlayer, string messageOfPlayer)
    {
        nameOfPlayer = CutPlayerName(nameOfPlayer);

        // Tcheck if game didn't start. 
        if (messageOfPlayer == JoinCommande && canJoinedGame) //Connection du joueur twitch dans le jeu 
        {
            if (!PlayerManager.Instance.AllPlayersName.Contains(nameOfPlayer))
            {
                PlayerManager.Instance.AllPlayersName.Add(nameOfPlayer);
                Debug.Log("COMMAND : " + nameOfPlayer + " join the game !");
                ShowAllPlayersInGame();
            }
        }

        if (PlayerManager.Instance.AllPlayersName.Contains(nameOfPlayer)) // S'assurer que le joueur est dans la liste des joueurs pour faire ces commandes. 
        {
            if (messageOfPlayer == QuitCommande) //Deconnection du joueur twitch du jeu. 
            {
                PlayerManager.Instance.AllPlayersName.Remove(nameOfPlayer);
                Debug.Log("COMMAND : " + nameOfPlayer + " quit the game !");
                ShowAllPlayersInGame();
            }

            if (playersCanMakeActions)
            {
                //Need to be in ActionState in the GlobalManager.

                Player currentplayer = PlayerManager.Instance.ReturnPlayerWithName(nameOfPlayer);

                if (messageOfPlayer == AttackCommande) //Attack
                {
                    Debug.Log("COMMAND : " + nameOfPlayer + " attack !");
                    InputManager.Instance.AttackCommand(currentplayer);
                }
                if (messageOfPlayer == LeftCommande) //MoveLeft
                {
                    Debug.Log("COMMAND : " + nameOfPlayer + " move to the left !");
                    InputManager.Instance.MoveCommand(currentplayer, EnumClass.Direction.Left);
                }
                if (messageOfPlayer == RightCommande) //MoveRight
                {
                    Debug.Log("COMMAND : " + nameOfPlayer + " move to the right !");
                    InputManager.Instance.MoveCommand(currentplayer, EnumClass.Direction.Right);
                }
                if (messageOfPlayer == UpCommande) //MoveTop
                {
                    Debug.Log("COMMAND : " + nameOfPlayer + " move to the top !");
                    InputManager.Instance.MoveCommand(currentplayer, EnumClass.Direction.Up);
                }
                if (messageOfPlayer == DownCommande) //MoveDown
                {
                    Debug.Log("COMMAND : " + nameOfPlayer + " move to the down !");
                    InputManager.Instance.MoveCommand(currentplayer, EnumClass.Direction.Down);
                }
            }
            
            if (playersCanMakeChoices)
            {
                //Need to be in ActionState in the GlobalManager.

                Player currentplayer = PlayerManager.Instance.ReturnPlayerWithName(nameOfPlayer);

                if (messageOfPlayer.Contains(ChoiceCommande))
                {
                    //Enlever le text de la commande
                    string numberOfCommandTxt = messageOfPlayer.Substring(ChoiceCommande.Length);

                    string officialsNumber = string.Empty;

                    //Chercher si nous possedons un/des chiffre dans notre nouveau string
                    foreach (char cara in numberOfCommandTxt)
                    {
                        if (char.IsDigit(cara))
                        {
                            officialsNumber += cara;
                        }
                    }

                    //Si le string n'est pas null on le transform en int
                    if (officialsNumber.Length > 0)
                    {
                        int numberOfCommand = int.Parse(officialsNumber);

                        Debug.Log("Number of command : " + numberOfCommand);
                    }
                    else 
                    {
                        Debug.Log("Number of command is not valid !");
                        return; 
                    }
                }

               /* if (messageOfPlayer == ChoiceCommande) //Weapon 1
                {
                    Debug.Log("COMMAND : " + nameOfPlayer + " choose Weapon 1 !");
                    InputManager.Instance.ChoiceCommand(currentplayer, 0);
                }*/

            }
        }        
    }

    public string CutPlayerName(string namePlayer)
    {
        
        if (namePlayer.Length > maxCharacterInNames)
        {
            string nameCut = "";
            for (int i = 0; i < maxCharacterInNames; i++)
            {
                nameCut += namePlayer[i];
            }
            return nameCut;
        }
        else
        {
            return namePlayer;
        }
    }

    public void ShowAllPlayersInGame()
    {
        string List = LobbyManager.Instance.PlayerList.text = "";
        

        foreach (string item in PlayerManager.Instance.AllPlayersName)
        {
            Debug.Log("Try");
            LobbyManager.Instance.PlayerList.text += item + "\n";
        }
    }

}

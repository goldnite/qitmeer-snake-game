using System.Collections;
using System.Collections.Generic;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.HostWallet;
using Nethereum.Util;
using Nethereum.Web3;
using System;
using System.Numerics;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Web3Unity;
using SnakeGame;
using Cysharp.Threading.Tasks;
using Michsky.UI.ModernUIPack;
using UnityEngine.UI;
using TMPro;

public class Landing : MonoBehaviour
{

  [SerializeField] private WindowManager windowManager;
  [SerializeField] private NotificationStacking notifications;
  [SerializeField] private NotificationManager success;
  [SerializeField] private NotificationManager failure;

  [SerializeField] private ButtonManagerBasicWithIcon btnWallet;
  [SerializeField] private ButtonManagerBasicWithIcon btnStart;
  [SerializeField] private ButtonManagerBasicWithIcon btnParticipants;
  [SerializeField] private ButtonManagerBasicWithIcon btnPlayers;
  [SerializeField] private ButtonManagerBasicWithIcon btnAwards;

  [SerializeField] private RawImage gameImage;

  [SerializeField] private GameObject participantsList;
  [SerializeField] private GameObject participantRowItem;
  [SerializeField] private GameObject playersList;
  [SerializeField] private GameObject playerRowItem;
  [SerializeField] private GameObject awardsList;
  [SerializeField] private GameObject awardRowItem;

  // Contract variables
  public SnakeGameService snakeGameService;
  // public const string contractAddress = "0x7f1cf46659b54dab4f0ae4a2157284f3ac38ef54";
  public const string contractAddress = "0xfC43b80A26f30bAA0131fd0ebD5789aB91Ba6785";

  private bool connected = false;
  private bool flag_fetchData = false;

  public Player[] players;
  public Participant[] participants;
  public AwardRecord[] awardRecords;
  // Start is called before the first frame update
  void Start()
  {
#if !(DEVELOPMENT_BUILD || UNITY_EDITOR)
    Debug.unityLogger.filterLogType = LogType.Exception;
#endif
    EnableButton(btnStart, false);
    EnableButton(btnParticipants, false);
    EnableButton(btnPlayers, false);
    EnableButton(btnAwards, false);
    Web3Connect.Instance.OnConnected += Instance_OnConnected;
  }

  private void EnableButton(ButtonManagerBasicWithIcon button, bool active)
  {
    button.buttonVar.interactable = active;
    button.enabled = active;
  }


  private void Instance_OnConnected(object sender, string e)
  {
    Initialize();
  }

  public void ConnectWallet()
  {
    if (connected == false)
      SceneManager.LoadScene("Web3Modal", LoadSceneMode.Additive);
    else
    {
      Web3Connect.Instance.Disconnect();
      connected = false;
      btnWallet.buttonText = "Connect Wallet";
      btnWallet.UpdateUI();
      EnableButton(btnStart, false);
      EnableButton(btnParticipants, false);
      EnableButton(btnPlayers, false);
      EnableButton(btnAwards, false);
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (!connected)
    {
      if (Web3Connect.Instance.Connected)
      {
        Initialize();
      }
    }
  }
  private void Initialize()
  {
    connected = true;
    string address = Web3Connect.Instance.AccountAddress;
    btnWallet.buttonText = address.Substring(0, 6) + "..." + address.Substring(address.Length - 4, 4);
    btnWallet.UpdateUI();
    snakeGameService = new SnakeGameService(contractAddress);
    if (!flag_fetchData)
    {
      // prevent from multiple invoke repeating
      flag_fetchData = true;
      InvokeRepeating("FetchData", 0, 20);
    }
    EnableButton(btnStart, true);
    SwitchNetwork();
  }
  private async void SwitchNetwork()
  {
    // exemple of call add and switch
    // AddEthereumChainParameter data = new AddEthereumChainParameter()
    // {
    //   ChainId = new BigInteger(80001).ToHexBigInteger(),
    //   BlockExplorerUrls = new List<string> { "https://mumbai.polygonscan.com/" },
    //   ChainName = "Polygon Mumbai Testnet",
    //   IconUrls = new List<string> { "https://polygon.technology/favicon.ico" },
    //   NativeCurrency = new NativeCurrency() { Decimals = 18, Name = "Matic", Symbol = "MATIC" },
    //   RpcUrls = new List<string> { "https://rpc-mumbai.maticvigil.com/" }
    // };
    // AddEthereumChainParameter data = new AddEthereumChainParameter()
    // {
    //   ChainId = new BigInteger(5).ToHexBigInteger(),
    //   BlockExplorerUrls = new List<string> { "https://goerli.etherscan.io/" },
    //   ChainName = "Goerli Testnet",
    //   IconUrls = new List<string> { "https://github.com/ethereum/ethereum-org/blob/master/dist/favicon.ico" },
    //   NativeCurrency = new NativeCurrency() { Decimals = 18, Name = "GoerliETH", Symbol = "GoerliETH" },
    //   RpcUrls = new List<string> { "https://goerli.infura.io/v3/" }
    // };
    AddEthereumChainParameter data = new AddEthereumChainParameter()
    {
      ChainId = new BigInteger(223).ToHexBigInteger(),
      BlockExplorerUrls = new List<string> { "https://testnet.qng.meerscan.io/" },
      ChainName = "Qitmeer Testnet",
      IconUrls = new List<string> { "https://testnet.qng.meerscan.io/images/favicon-32x32-de0f59b7ad593d6b99e463c3bbe4f5b3.png?vsn=d1" },
      NativeCurrency = new NativeCurrency() { Decimals = 18, Name = "MEER", Symbol = "MEER" },
      RpcUrls = new List<string> { "https://meer.testnet.meerfans.club", "https://evm-testnet-node.qitmeer.io" }
    };
    await Web3Connect.Instance.AddAndSwitchChain(data);
    print("switch end");
  }


  private async void FetchData()
  {
    if (Web3Connect.Instance.Web3 != null)
    {
      // request user balance, we can use classic nethereum function
      var meerBalanceInGWei = await Web3Connect.Instance.Web3.Eth.GetBalance.SendRequestAsync(Web3Connect.Instance.AccountAddress);
      var meerBalance = UnitConversion.Convert.FromWei(meerBalanceInGWei.Value);
      // lblLabel.text = $"{meerBalance.ToString("F3")} MEER";
      //   lblAccount.text = $"{Web3Connect.Instance.AccountAddress} {amount.ToString("F3")} MEER";
      Debug.Log("Data fetch");

      try
      {
        var owner = await snakeGameService.OwnerQueryAsync();
        var price = await snakeGameService.PriceQueryAsync();
        var withdrawAddress = await snakeGameService.WithdrawAddressQueryAsync();
        var awardShare = await snakeGameService.GetAwardShareQueryAsync();
        awardRecords = (await snakeGameService.GetAwardRecordsQueryAsync()).ReturnValue1.ToArray();
        var playerAddresses = await snakeGameService.GetPlayersQueryAsync();
        var newPlayers = new Player[playerAddresses.Count];
        for (int i = 0; i < (int)playerAddresses.Count; i++)
        {
          newPlayers[i] = new Player();
          newPlayers[i].address = playerAddresses[i];
          newPlayers[i].accAward = await snakeGameService.AccAwardsQueryAsync(playerAddresses[i]);
          newPlayers[i].accPoint = await snakeGameService.AccPointsQueryAsync(playerAddresses[i]);
        }
        players = newPlayers.OrderBy(pl => pl.accPoint).ToArray();
        var participantAddresses = await snakeGameService.GetParticipantsQueryAsync();
        var newParticipants = new Participant[participantAddresses.Count];
        for (int i = 0; i < participantAddresses.Count; i++)
        {
          newParticipants[i] = new Participant();
          newParticipants[i].address = participantAddresses[i];
          newParticipants[i].totalPoint = await snakeGameService.TotalPointsQueryAsync(participantAddresses[i]);
        }
        participants = newParticipants.OrderBy(pl => pl.totalPoint).ToArray();

        // Update participants list
        for (int i = 0; i < participantsList.transform.childCount; i++)
        {
          Destroy(participantsList.transform.GetChild(i).gameObject);
        }
        GameObject row = GameObject.Instantiate(participantRowItem, participantsList.transform);
        row.transform.SetParent(participantsList.transform);
        for (int i = 0; i < participants.Length; i++)
        {
          row = GameObject.Instantiate(participantRowItem, participantsList.transform);
          row.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = (i + 1).ToString();
          row.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = participants[i].address;
          row.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = participants[i].totalPoint.ToString();
        }

        // Update players list
        for (int i = 0; i < playersList.transform.childCount; i++)
        {
          Destroy(playersList.transform.GetChild(i).gameObject);
        }
        row = GameObject.Instantiate(playerRowItem, playersList.transform);
        row.transform.SetParent(playersList.transform);
        for (int i = 0; i < players.Length; i++)
        {
          row = GameObject.Instantiate(playerRowItem, playersList.transform);
          row.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = (i + 1).ToString();
          row.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = players[i].address;
          row.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = UnitConversion.Convert.FromWei(players[i].accAward).ToString();
          row.transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = players[i].accPoint.ToString("D");
          row.transform.GetChild(4).gameObject.GetComponent<TMP_Text>().text = (new DateTime(1970, 1, 1, 0, 0, 0, 0) + TimeSpan.FromSeconds((long)players[i].lastPlayedTime)).ToString();
        }

        // Update players list
        for (int i = awardsList.transform.childCount; i < awardRecords.Length; i++)
        {
          row = GameObject.Instantiate(awardRowItem, awardsList.transform);
          row.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = (new DateTime(1970, 1, 1, 0, 0, 0, 0) + TimeSpan.FromSeconds((long)awardRecords[i].Timestamp)).ToString();
          int j;
          for (j = 0; awardRecords[i].Scores[j] > 0; j++)
          {
            GameObject r = GameObject.Instantiate(row.transform.GetChild(1).gameObject, row.transform);
            r.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = (j + 1).ToString();
            r.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = awardRecords[i].Awardees[j];
            r.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = awardRecords[i].Scores[j].ToString("D");
            r.transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = UnitConversion.Convert.FromWei(awardRecords[i].Amounts[j]).ToString();
          }
          row.GetComponent<RectTransform>().sizeDelta = new UnityEngine.Vector2(row.GetComponent<RectTransform>().sizeDelta.x, row.GetComponent<RectTransform>().sizeDelta.y + 105 * j);
        }

        EnableButton(btnParticipants, true);
        EnableButton(btnPlayers, true);
        EnableButton(btnAwards, true);
      }
      catch (System.Exception e)
      {
        Debug.LogException(e);
      }
    }
  }
  public async void StartGame()
  {
    try
    {
      windowManager.OpenPanel("Loading");
      StartGameFunction startGame = new StartGameFunction();
      startGame.FromAddress = Web3Connect.Instance.AccountAddress;
      startGame.AmountToSend = Web3.Convert.ToWei(0.5);
      var txReceipt = await snakeGameService.StartGameRequestAndWaitForReceiptAsync(startGame);
      Debug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(txReceipt));
      windowManager.OpenPanel("Blank");
      gameImage.enabled = true;
      GameObject.Find("Snake").SendMessage("StartGame");
    }
    catch (System.Exception e)
    {
      Debug.LogException(e);
      NotificationManager s = GameObject.Instantiate(failure, notifications.gameObject.transform);
      s.description = $"User denied transaction.";
      windowManager.OpenPanel("Blank");
    }
  }

  private async void EndGame(int score)
  {
    try
    {
      windowManager.OpenPanel("Loading");
      Debug.Log($"score:{score}");
      gameImage.enabled = false;
      EndGameFunction endGame = new EndGameFunction();
      endGame.FromAddress = Web3Connect.Instance.AccountAddress;
      endGame.Point = score;
      NotificationManager s = GameObject.Instantiate(success, notifications.gameObject.transform);
      s.description = $"You have earned {score} points.";
      var txReceipt = await snakeGameService.EndGameRequestAndWaitForReceiptAsync(endGame);
      // BigInteger score = new BigInteger(txReceipt.DecodeAllEvents<EndGameEventDTO>()[0].Point);
      Debug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(txReceipt));
      if (txReceipt.Status.Value == 1)
      {
        s = GameObject.Instantiate(success, notifications.gameObject.transform);
        s.description = $"Point saved on-chain.";
      }
      else
      {
        s = GameObject.Instantiate(failure, notifications.gameObject.transform);
        s.description = $"Something went wrong.";
      }
      FetchData();
      windowManager.OpenPanel("Blank");
    }
    catch (System.Exception e)
    {
      Debug.LogException(e);
      NotificationManager s = GameObject.Instantiate(failure, notifications.gameObject.transform);
      s.description = $"User denied transaction.";
      windowManager.OpenPanel("Blank");
    }
  }

}

using System.Collections;
using System.Collections.Generic;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.HostWallet;
using Nethereum.Util;
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


public class Dashboard : MonoBehaviour
{
  // UI Elements
  protected VisualElement root;
  protected VisualElement dashboard;
  protected ScrollView scrollBoard;
  protected Button btnWallet;
  protected Button btnStart;
  protected Label lblLabel;
  public VisualTreeAsset scoreRowTemplate;

  // Contract variables
  public SnakeGameService snakeGameService;
  public const string contractAddress = "0xa66bac7b62249c79eaca335aa5f9524783c6e670";
  // public const string contractAddress = "0x0Ff3AEFb87b0cD7163d13e45db1a916632ef41Eb";

  private bool connected = false;
  private bool flag_fetchData = false;

  // Start is called before the first frame update
  void Start()
  {
    root = GetComponent<UIDocument>().rootVisualElement;
    dashboard = root.Q<VisualElement>("dashboard");
    scrollBoard = root.Q<ScrollView>("scrollBoard");
    btnWallet = root.Q<Button>("btnWallet");
    btnStart = root.Q<Button>("btnStart");
    lblLabel = root.Q<Label>("lblLabel");

    btnWallet.clicked += BtnWallet_clicked;
    btnStart.clicked += BtnStart_clicked;
    btnStart.SetEnabled(false);

    Web3Connect.Instance.OnConnected += Instance_OnConnected;
  }


  private void Instance_OnConnected(object sender, string e)
  {
    Initialize();
  }


  private async void FetchData()
  {
    if (Web3Connect.Instance.Web3 != null)
    {
      // request user balance, we can use classic nethereum function
      var meerBalanceInGWei = await Web3Connect.Instance.Web3.Eth.GetBalance.SendRequestAsync(Web3Connect.Instance.AccountAddress);
      var meerBalance = UnitConversion.Convert.FromWei(meerBalanceInGWei.Value);
      lblLabel.text = $"{meerBalance.ToString("F3")} MEER";
      //   lblAccount.text = $"{Web3Connect.Instance.AccountAddress} {amount.ToString("F3")} MEER";
      Debug.Log("Data fetch");

      try
      {
        var owner = await snakeGameService.OwnerQueryAsync();
        var price = await snakeGameService.PriceQueryAsync();
        var withdrawAddress = await snakeGameService.WithdrawAddressQueryAsync();
        var awardShare = await snakeGameService.GetAwardShareQueryAsync();
        var awardRecords = await snakeGameService.GetAwardRecordsQueryAsync();
        var playerAddresses = await snakeGameService.GetPlayersQueryAsync();
        var players = new Player[playerAddresses.Count];
        for (int i = 0; i < (int)playerAddresses.Count; i++)
        {
          players[i] = new Player();
          players[i].address = playerAddresses[i];
          players[i].accAward = await snakeGameService.AccAwardsQueryAsync(playerAddresses[i]);
          players[i].accPoint = await snakeGameService.AccPointsQueryAsync(playerAddresses[i]);
        }
        var participantAddresses = await snakeGameService.GetParticipantsQueryAsync();
        var participants = new Participant[participantAddresses.Count];
        for (int i = 0; i < participantAddresses.Count; i++)
        {
          participants[i] = new Participant();
          participants[i].address = participantAddresses[i];
          participants[i].totalPoint = await snakeGameService.TotalPointsQueryAsync(participantAddresses[i]);
        }
        Debug.Log($"Owner: {owner}");
        Debug.Log($"Price: {price}");
        Debug.Log($"withdrawAddress: {withdrawAddress}");
        Debug.Log($"awardShare: {awardShare}");
        Debug.Log($"awardRecords: {awardRecords}");
        Debug.Log($"playerAddresses: {playerAddresses}");
        Debug.Log($"players: {players}");
        Debug.Log($"participantAddresses: {participantAddresses}");
        Debug.Log($"participants: {participants}");
      }
      catch (System.Exception e)
      {
        Debug.LogException(e);
      }
    }
  }

  private async void BtnStart_clicked()
  {
    try
    {
      var txReceipt = await snakeGameService.StartGameRequestAndWaitForReceiptAsync();
      Debug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(txReceipt));
      root.style.display = DisplayStyle.None;
      GameObject.Find("Snake").SendMessage("StartGame");
    }
    catch (System.Exception e)
    {
      Debug.LogException(e);
    }
  }

  private async void EndGame(int score)
  {
    root.style.display = DisplayStyle.Flex;
    var txReceipt = await snakeGameService.EndGameRequestAndWaitForReceiptAsync(score);
    Debug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(txReceipt));
    FetchData();
  }

  private async void BtnSwitch_clicked()
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
    AddEthereumChainParameter data = new AddEthereumChainParameter()
    {
      ChainId = new BigInteger(5).ToHexBigInteger(),
      BlockExplorerUrls = new List<string> { "https://goerli.etherscan.io/" },
      ChainName = "Goerli Testnet",
      IconUrls = new List<string> { "https://github.com/ethereum/ethereum-org/blob/master/dist/favicon.ico" },
      NativeCurrency = new NativeCurrency() { Decimals = 18, Name = "GoerliETH", Symbol = "GoerliETH" },
      RpcUrls = new List<string> { "https://goerli.infura.io/v3/" }
    };
    // AddEthereumChainParameter data = new AddEthereumChainParameter()
    // {
    //   ChainId = new BigInteger(223).ToHexBigInteger(),
    //   BlockExplorerUrls = new List<string> { "https://testnet.qng.meerscan.io/" },
    //   ChainName = "Qitmeer Testnet",
    //   IconUrls = new List<string> { "https://testnet.qng.meerscan.io/images/favicon-32x32-de0f59b7ad593d6b99e463c3bbe4f5b3.png?vsn=d1" },
    //   NativeCurrency = new NativeCurrency() { Decimals = 18, Name = "MEER", Symbol = "MEER" },
    //   RpcUrls = new List<string> { "https://meer.testnet.meerfans.club", "https://evm-testnet-node.qitmeer.io" }
    // };
    await Web3Connect.Instance.AddAndSwitchChain(data);
    print("switch end");
  }

  private void BtnWallet_clicked()
  {
    if (connected == false)
      SceneManager.LoadScene("Web3Modal", LoadSceneMode.Additive);
    else
    {
      Web3Connect.Instance.Disconnect();
      connected = false;
      btnWallet.text = "Connect Wallet";
    }
  }
  // Update is called once per frame
  void Update()
  {
    scrollBoard.Clear();
    var scoreRow = scoreRowTemplate.CloneTree().ElementAt(0);
    scrollBoard.Add(scoreRow);
    for (int i = 0; i < 100; i++)
    {
      scoreRow = scoreRowTemplate.CloneTree().ElementAt(0);
      (scoreRow.ElementAt(0) as Label).text = (i + 1).ToString();
      (scoreRow.ElementAt(1) as Label).text = "0x000000000000";
      (scoreRow.ElementAt(2) as Label).text = (100 - i).ToString();
      scrollBoard.Add(scoreRow);
    }

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
    btnWallet.text = address.Substring(0, 6) + "..." + address.Substring(address.Length - 4, 4);
    snakeGameService = new SnakeGameService(contractAddress);
    if (!flag_fetchData)
    {
      // prevent from multiple invoke repeating
      flag_fetchData = true;
      InvokeRepeating("FetchData", 0, 100000);
    }
    btnStart.SetEnabled(true);
  }
}

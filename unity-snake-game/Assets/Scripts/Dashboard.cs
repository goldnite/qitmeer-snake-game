using System.Collections;
using System.Collections.Generic;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.HostWallet;
using Nethereum.Util;
using System;
using System.Numerics;
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
  protected Button btnWallet;
  protected Label lblLabel;

  // Contract variables
  private SnakeGameService snakeGameService;
  public const string contractAddress = "0x5d4dc51a0f1c7ac8426aa28b78fff45369221851";

  private bool connected = false;
  private bool flag_fetchData = false;

  // Start is called before the first frame update
  void Start()
  {
    root = GetComponent<UIDocument>().rootVisualElement;
    dashboard = root.Q<VisualElement>("dashboard");
    btnWallet = root.Q<Button>("btnWallet");
    lblLabel = root.Q<Label>("lblLabel");

    btnWallet.clicked += BtnWallet_clicked;

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
        // var 
        // var decimals = await tokenService.DecimalsQueryAsync();
        // var symbol = await tokenService.SymbolQueryAsync();
        // emple of call balance
        // BigInteger result = await tokenService.BalanceOfQueryAsync(Web3Connect.Instance.AccountAddress);
        // var amount = UnitConversion.Convert.FromWei(result, decimals);
        // lblResult.text = $"My balance {amount} {symbol}";
        var owner = await snakeGameService.OwnerQueryAsync();
        Debug.Log($"owner: {owner}");
        var price = await snakeGameService.PriceQueryAsync();
        Debug.Log($"price: {price}");
        var withdrawAddress = await snakeGameService.WithdrawAddressQueryAsync();
        Debug.Log($"withdrawAddress: {withdrawAddress}");
        var playerCount = await snakeGameService.PlayerCountQueryAsync();
        Debug.Log($"playerCount: {playerCount}");
        var participantCount = await snakeGameService.ParticipantCountQueryAsync();
        Debug.Log($"participantCount: {participantCount}");
        var awardCount = await snakeGameService.AwardCountQueryAsync();
        Debug.Log($"awardCount: {awardCount}");
        var awardRecords = new BigInteger[(int)awardCount];
        for (int i = 0; i < (int)awardCount; i++)
        {
          awardRecords[i] = await snakeGameService.AwardRecordsQueryAsync(i);
          Debug.Log($"awardRecords[{i}]: {awardRecords[i]}");
        }
        var awardShare = new int[5];
        for (int i = 0; i < 5; i++)
        {
          awardShare[i] = (int)await snakeGameService.AwardShareQueryAsync(i);
          Debug.Log($"awardShare[{i}]: {awardShare[i]}");
        }
        Debug.Log("---Players---");
        var players = new Player[(int)playerCount];
        for (int i = 0; i < (int)playerCount; i++)
        {
          players[i] = new Player();
          players[i].address = await snakeGameService.PlayersQueryAsync(i);
          players[i].accAward = await snakeGameService.AccAwardsQueryAsync(players[i].address);
          players[i].accPoint = await snakeGameService.AccPointsQueryAsync(players[i].address);
          Debug.Log($"address {i}: {players[i].address}");
          Debug.Log($"address {i}: {players[i].accAward}");
          Debug.Log($"address {i}: {players[i].accPoint}");
        }
        Debug.Log("---Participants---");
        var participants = new Participant[(int)participantCount];
        for (int i = 0; i < participantCount; i++)
        {
          participants[i] = new Participant();
          participants[i].address = await snakeGameService.ParticipantsQueryAsync(i);
          participants[i].totalPoint = await snakeGameService.TotalPointsQueryAsync(participants[i].address);
          Debug.Log($"address {i}: {participants[i].address}");
          Debug.Log($"address {i}: {participants[i].totalPoint}");
        }

        await UniTask.Run(() => BtnSwitch_clicked());
        Debug.Log(Web3Connect.Instance.ChainId);
        // if (Web3Connect.Instance.ChainId == "0x5")
        // {
          var txReceipt = await snakeGameService.EndGameRequestAndWaitForReceiptAsync(new BigInteger(200));
          Debug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(txReceipt));
        // }
      }
      catch (System.Exception e)
      {
        Debug.LogException(e);
      }
    }
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
  }
}

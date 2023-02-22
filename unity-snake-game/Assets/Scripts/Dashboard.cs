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


public class Dashboard : MonoBehaviour
{
  // UI Elements
  protected VisualElement root;
  protected VisualElement dashboard;
  protected VisualElement scrollBoard;
  protected Button btnWallet, btnStart, btnParticipants, btnPlayers, btnAwards;

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
  async void Start()
  {
    root = GetComponent<UIDocument>().rootVisualElement;
    dashboard = root.Q<VisualElement>("dashboard");
    scrollBoard = root.Q<VisualElement>("scrollBoard");
    btnWallet = root.Q<Button>("btnWallet");
    btnStart = root.Q<Button>("btnStart");
    btnParticipants = root.Q<Button>("btnParticipants");
    btnPlayers = root.Q<Button>("btnPlayers");
    btnAwards = root.Q<Button>("btnAwards");

    btnWallet.clicked += BtnWallet_clicked;
    btnStart.clicked += BtnStart_clicked;
    btnParticipants.clicked += ShowParticipants;
    btnPlayers.clicked += ShowPlayers;
    btnAwards.clicked += ShowAwards;
    btnStart.SetEnabled(false);
    btnParticipants.SetEnabled(false);
    btnPlayers.SetEnabled(false);
    btnAwards.SetEnabled(false);

    Web3Connect.Instance.OnConnected += Instance_OnConnected;
    FetchData();
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

        btnParticipants.SetEnabled(true);
        btnPlayers.SetEnabled(true);
        btnAwards.SetEnabled(true);
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
      StartGameFunction startGame = new StartGameFunction();
      startGame.FromAddress = Web3Connect.Instance.AccountAddress;
      startGame.AmountToSend = Web3.Convert.ToWei(0.5);
      var txReceipt = await snakeGameService.StartGameRequestAndWaitForReceiptAsync(startGame);
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
    Debug.Log($"score:{score}");
    root.style.display = DisplayStyle.Flex;
    EndGameFunction endGame = new EndGameFunction();
    endGame.FromAddress = Web3Connect.Instance.AccountAddress;
    endGame.Point = score;
    var txReceipt = await snakeGameService.EndGameRequestAndWaitForReceiptAsync(endGame);
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

  private void ShowAwards()
  {
    VisualElement wrapper = new VisualElement();
    wrapper.style.width = new StyleLength(Length.Percent(100.0f));
    wrapper.style.flexDirection = FlexDirection.Column;
    wrapper.style.justifyContent = Justify.Center;
    wrapper.style.alignItems = Align.Center;
    if (awardRecords.Length == 0)
    {
      wrapper.Add(new Label("No data available."));
    }
    else
    {
      for (int i = 0; i < awardRecords.Length; i++)
      {
        VisualElement veRow = new VisualElement();
        veRow.style.width = new StyleLength(Length.Percent(100.0f));
        veRow.style.flexDirection = FlexDirection.Column;
        VisualElement veTime = new VisualElement();
        veTime.style.alignItems = Align.Center;
        veTime.style.justifyContent = Justify.Center;
        veTime.Add(new Label((new DateTime(1970, 1, 1, 0, 0, 0, 0) + TimeSpan.FromSeconds((long)awardRecords[i].Timestamp)).ToString()));
        VisualElement veMain = new VisualElement();
        veMain.style.flexDirection = FlexDirection.Row;
        veMain.style.justifyContent = Justify.SpaceBetween;
        veMain.style.width = new StyleLength(Length.Percent(100.0f));

        VisualElement colRank = new VisualElement();
        VisualElement colAddr = new VisualElement();
        VisualElement colScore = new VisualElement();
        VisualElement colAmount = new VisualElement();
        colRank.style.flexDirection = FlexDirection.Column;
        colAddr.style.flexDirection = FlexDirection.Column;
        colScore.style.flexDirection = FlexDirection.Column;
        colAmount.style.flexDirection = FlexDirection.Column;
        for (int j = 0; j < awardRecords[i].Amounts.Count; j++)
        {
          Debug.Log(awardRecords[i].Amounts[j]);
          colRank.Add(new Label((j + 1).ToString()));
          colAddr.Add(new Label(awardRecords[i].Awardees[j]));
          colScore.Add(new Label(awardRecords[i].Scores[j].ToString("D")));
          colAmount.Add(new Label(awardRecords[i].Amounts[j].ToString("D")));
        }
        veMain.Add(colRank);
        veMain.Add(colAddr);
        veMain.Add(colScore);
        veMain.Add(colAmount);
        veRow.Add(veTime);
        veRow.Add(veMain);
        wrapper.Add(veRow);
      }
    }
    scrollBoard.Clear();
    scrollBoard.Add(wrapper);
    flash();
  }
  private void ShowParticipants()
  {
    VisualElement wrapper = new VisualElement();
    wrapper.style.width = new StyleLength(Length.Percent(100.0f));
    wrapper.style.flexDirection = FlexDirection.Row;
    wrapper.style.justifyContent = Justify.SpaceBetween;
    wrapper.style.alignItems = Align.Center;
    if (participants.Length == 0)
    {
      wrapper.Add(new Label("No data available."));
    }
    else
    {
      VisualElement colRank = new VisualElement();
      VisualElement colAddr = new VisualElement();
      VisualElement colPoint = new VisualElement();
      colRank.style.unityTextAlign = TextAnchor.MiddleCenter;
      for (int i = 0; i < participants.Length; i++)
      {
        Label rank = new Label();
        Label addr = new Label();
        Label point = new Label();
        rank.text = (i + 1).ToString();
        addr.text = participants[i].address;
        point.text = participants[i].totalPoint.ToString("D");
        colRank.Add(rank);
        colAddr.Add(addr);
        colPoint.Add(point);
      }
      wrapper.Add(colRank);
      wrapper.Add(colAddr);
      wrapper.Add(colPoint);
      wrapper.style.flexDirection = FlexDirection.Row;
    }
    scrollBoard.Clear();
    scrollBoard.Add(wrapper);
    flash();
  }
  private void ShowPlayers()
  {
    VisualElement wrapper = new VisualElement();
    wrapper.style.width = new StyleLength(Length.Percent(100.0f));
    wrapper.style.flexDirection = FlexDirection.Row;
    wrapper.style.justifyContent = Justify.SpaceBetween;
    wrapper.style.alignItems = Align.Center;
    if (players.Length == 0)
    {
      wrapper.Add(new Label("No data available."));
    }
    else
    {
      VisualElement colRank = new VisualElement();
      VisualElement colAddr = new VisualElement();
      VisualElement colPoint = new VisualElement();
      VisualElement colAward = new VisualElement();
      VisualElement colTime = new VisualElement();
      colRank.style.unityTextAlign = TextAnchor.MiddleCenter;
      for (int i = 0; i < players.Length; i++)
      {
        Label rank = new Label();
        Label addr = new Label();
        Label point = new Label();
        Label award = new Label();
        Label time = new Label();
        rank.text = (i + 1).ToString();
        addr.text = players[i].address;
        point.text = players[i].accPoint.ToString("D");
        award.text = players[i].accAward.ToString("D");
        DateTime t = new DateTime(1970, 1, 1, 0, 0, 0, 0) + TimeSpan.FromSeconds((long)players[i].lastPlayedTime);
        time.text = t.ToString();
        colRank.Add(rank);
        colAddr.Add(addr);
        colPoint.Add(point);
        colAward.Add(award);
        colTime.Add(time);
      }
      wrapper.Add(colRank);
      wrapper.Add(colAddr);
      wrapper.Add(colPoint);
      wrapper.Add(colAward);
      // wrapper.Add(colTime);
      wrapper.style.flexDirection = FlexDirection.Row;
    }
    scrollBoard.Clear();
    scrollBoard.Add(wrapper);
    flash();
  }

  private async void flash()
  {
    scrollBoard.EnableInClassList("highlight", true);
    await UniTask.Delay(TimeSpan.FromSeconds(0.5));
    scrollBoard.EnableInClassList("highlight", false);
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
      btnStart.SetEnabled(false);
      btnParticipants.SetEnabled(false);
      btnPlayers.SetEnabled(false);
      btnAwards.SetEnabled(false);
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
      InvokeRepeating("FetchData", 0, 20);
    }
    btnStart.SetEnabled(true);
    BtnSwitch_clicked();
  }
}

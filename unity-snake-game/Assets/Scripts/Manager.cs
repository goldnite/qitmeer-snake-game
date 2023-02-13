using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WalletConnectSharp.Core;
using WalletConnectSharp.Core.Events;
using WalletConnectSharp.Core.Events.Request;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Core.Models.Ethereum;
using WalletConnectSharp.Core.Network;
using WalletConnectSharp.Core.Utils;
using WalletConnectSharp.Unity;

public class Manager : WalletConnectActions
{
  public WalletConnect walletConnect;
  public GameObject ConnectScreen;
  public GameObject ConnectButton;
  public string account;

  // Start is called before the first frame update
  void Start()
  {
    ConnectScreen.GetComponent<CanvasGroup>().alpha = 0;
    DontDestroyOnLoad(walletConnect);
    DontDestroyOnLoad(gameObject);
  }

  // Update is called once per frame
  void Update()
  {

  }

  void FixedUpdate()
  {
    if (WalletConnect.ActiveSession == null || WalletConnect.ActiveSession.Accounts == null)
      return;

    //accountText.text = "\nConnected to Chain " + WalletConnect.ActiveSession.ChainId + ":\n" + WalletConnect.ActiveSession.Accounts[0];
    account = WalletConnect.ActiveSession.Accounts[0];
  }
  public void ConnectWallet()
  {
    WalletConnect.Instance.Connect();
    ConnectScreen.GetComponent<CanvasGroup>().alpha = 1;
    ConnectButton.SetActive(false);
  }

  public async void OnClickSendTransaction()
  {
    var address = WalletConnect.ActiveSession.Accounts[0];
    var transaction = new TransactionData()
    {
      data = "0x",
      from = address,
      to = "0x637e10d84ca40B59250bb7758e45F2468fe7c4B7",
      gas = "21000",
      value = "1",
      chainId = 5,
    };

    var results = await SendTransaction(transaction);

    Debug.Log(results);
  }

  public async void OnClickSwitchChain()
  {
    Debug.Log("OnClickSwitchChain");
    try
    {
      var chainId = new EthChain();
      chainId.chainId = "0x5";
      //   chainId.chainId = "0xDF";
      var results = await WalletSwitchEthChain(chainId);
      Debug.Log(results);
    }
    catch
    {
      //If the client rejected or doesn't have that chain, try to add it.
      addEthChain();
    }
  }

  public async void addEthChain()
  {
    Debug.Log("addEthChain");
    List<string> list = new List<string>();
    list.Add("https://goerli.infura.io/v3/");
    var chainData = new EthChainData()
    {
      chainId = "0x5",
      chainName = "Goerli Testnet",
      rpcUrls = list.ToArray(),
      nativeCurrency = new NativeCurrency()
      {
        name = "GoerliETH",
        symbol = "GoerliETH",
        decimals = 18
      }
    };

    // list.Add("https://meer.testnet.meerfans.club");
    // list.Add("https://evm-testnet-node.qitmeer.io");
    // var chainData = new EthChainData()
    // {
    //   chainId = "0xDF",
    //   chainName = "Qitmeer Testnet",
    //   rpcUrls = list.ToArray(),
    //   nativeCurrency = new NativeCurrency()
    //   {
    //     name = "MEER",
    //     symbol = "MEER",
    //     decimals = 18
    //   }
    // };
    var results = await WalletAddEthChain(chainData);
    Debug.Log(results);
  }

  public async void OnClickDisconnectAndConnect()
  {
    bool shouldConnect = !WalletConnect.Instance.createNewSessionOnSessionDisconnect;
    CloseSession(shouldConnect);
    SceneManager.LoadScene("ConectWallet", LoadSceneMode.Single);
  }
  public void OnConnected()
  {
    Debug.Log("OnConnected");
  }

  public void OnConnectedSession(WCSessionData session)
  {
    ConnectScreen.GetComponent<CanvasGroup>().alpha = 0;
    SceneManager.LoadScene("Dashboard", LoadSceneMode.Single);
  }
  public void OnDisconnected(WalletConnectUnitySession session)
  {
    Debug.Log("OnDisconnected");
  }
  public void OnConnectionFailed(WalletConnectUnitySession session)
  {
    Debug.Log("OnConnectionFailed");
  }
  public void OnNewSessionConnected(WalletConnectUnitySession session)
  {
    Debug.Log("OnNewSessionConnected");
  }
  public void OnResumedSessionConnected(WalletConnectUnitySession session)
  {
    Debug.Log("OnResumedSessionConnected");
  }
}

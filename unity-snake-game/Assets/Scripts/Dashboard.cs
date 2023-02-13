using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WalletConnectSharp.Unity;

public class Dashboard : MonoBehaviour
{
  public Text address;
  // Start is called before the first frame update
  void Start()
  {
    address.text = WalletConnect.ActiveSession.Accounts[0];
  }

  // Update is called once per frame
  void Update()
  {

  }
}

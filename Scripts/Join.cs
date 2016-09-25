using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Join : MonoBehaviour
{
    public void OnClick()
    {
        NetworkManager.singleton.gameObject.GetComponent<NetworkDiscovery>().Initialize();
        NetworkManager.singleton.gameObject.GetComponent<NetworkDiscovery>().StartAsClient();
    }
}

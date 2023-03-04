using Mirror;
using UnityEngine;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SyncVar] [SerializeField] private string displayName = "Missing name";

    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }
}

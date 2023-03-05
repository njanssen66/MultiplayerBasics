using Mirror;
using TMPro;
using UnityEngine;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displayColorRenderer;

    [SyncVar(hook=nameof(HandleDisplayNameUpdated))] 
    [SerializeField] 
    private string displayName = "Missing name";

    [SyncVar(hook=nameof(HandleDisplayColorUpdated))] 
    [SerializeField] 
    private Color displayColor = Color.black;

#region Server

    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }

    [Server]
    public void SetDisplayColor(Color newDisplayColor)
    {
        displayColor = newDisplayColor;
    }

    [Command]
    private void CmdSetDisplayName(string newDisplayName)
    {
        //Can do server validation here
        SetDisplayName(newDisplayName);
        RpcLogName(newDisplayName);
    }

#endregion

#region Client

    private void HandleDisplayColorUpdated(Color oldColor, Color newColor)
    {
        displayColorRenderer.material.SetColor("_Color", newColor);
    }

    private void HandleDisplayNameUpdated(string oldName, string newName)
    {
        displayNameText.SetText(newName);
    }

    [ContextMenu("Set My Name")]
    private void SetMyName()
    {
        CmdSetDisplayName("My New Name");
    }

    [ClientRpc]
    private void RpcLogName(string newName)
    {
        Debug.Log($"A player had their name updated to {newName}");
    }

#endregion
}

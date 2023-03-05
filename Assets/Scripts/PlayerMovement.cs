using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent agent = null;

    private Camera mainCamera;

    #region Server

    [Command]
    private void CmdMove(Vector3 position)
    {
        bool isValidPosition = NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas);
        if (!isValidPosition) return;

        agent.SetDestination(position);
    }

    #endregion

    #region Client

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        mainCamera = Camera.main;
    }

    [ClientCallback]
    private void Update()
    {
        MoveToMouse();
    }

    private void MoveToMouse()
    {
        if(!isOwned) return;
        if(!Input.GetMouseButtonDown(0)) return;

        // Checks if the player clicked on movable area on an object with a navmesh in the scene
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        bool clickedOnMovableObject = Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity);
        if (!clickedOnMovableObject) return;

        //Moves the player to the point that was clicked on
        CmdMove(hit.point);
    }

    #endregion
}

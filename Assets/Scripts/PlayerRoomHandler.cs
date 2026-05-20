using UnityEngine;

public class PlayerRoomHandler : MonoBehaviour
{
    [SerializeField] private bool playerInsideRoom;

    public bool IsPlayerInsideRoom()
    {
        return playerInsideRoom;
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            playerInsideRoom = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            playerInsideRoom = false;
        }
        
    }

}

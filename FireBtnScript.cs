using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireBtnScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public MyPlayer player;
    // It sets the player
    public void SetPlayer(MyPlayer _player)
    {
        player = _player;
    }
    // This method is call when the Fire Button is press
    public void OnPointerDown(PointerEventData eventData)
    {
        // It makes Fire the player
        player.Fire();
    }
    // This method is call when the Fire Button is release
    public void OnPointerUp(PointerEventData eventData)
    {
        // It stops the player firing
        player.FireUp();
    }

}

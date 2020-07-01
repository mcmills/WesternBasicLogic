using UnityEngine;
using UnityEngine.EventSystems;

public class FixedButton : MonoBehaviour, IPointerClickHandler
{

    public MyPlayer player;
    // It sets the player
    public void SetPlayer(MyPlayer _player)
    {
        player = _player;
    }
    // This method is call when the Jump Button is press
    public void OnPointerClick(PointerEventData eventData)
    {
        // It makes Jump the player
        player.Jump();
    }



}

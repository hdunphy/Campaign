using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum PlayerColor { Red, Blue }

public class Manager : MonoBehaviour
{
    public static Manager Instance;
    public Tilemap HighlightTileMap;

    private PlayerController selectedPlayer;

    private void Awake()
    {
        Instance = this;
    }

    public void SetSelectedPlayer(PlayerController player)
    {
        if(selectedPlayer == null)
        {
            player.SetSelected(true);
            selectedPlayer = player;
        }
        else if(player == null)
        {
            selectedPlayer.SetSelected(false);
            selectedPlayer = null;
        }
        else if(selectedPlayer == player)
        {
            player.SetSelected(false);
            selectedPlayer = null;
        }
        else //selectedPlayer and player are not null but are different
        {
            selectedPlayer.SetSelected(false);
            player.SetSelected(true);
            selectedPlayer = player;
        }
    }

    public PlayerController GetPlayer()
    {
        return selectedPlayer;
    }


}

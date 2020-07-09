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
        if(selectedPlayer != null)
            selectedPlayer.SetSelected(false);

        selectedPlayer = player;

        if (selectedPlayer != null)
            selectedPlayer.SetSelected(true);

    }

    public PlayerController GetPlayer()
    {
        return selectedPlayer;
    }


}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInput : MonoBehaviour
{
    public Tilemap highlightTileMap;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit.collider != null)
            {

            }

            PlayerController selectedPlayer = Manager.Instance.GetPlayer();
            Vector3Int tilePos = highlightTileMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            TileBase highlightedTile = highlightTileMap.GetTile(tilePos);

            PlayerController playerOnTile = FindObjectsOfType<PlayerController>().FirstOrDefault(x => x.GetTilePosition() == tilePos);

            if (playerOnTile != null)
            {
                Manager.Instance.SetSelectedPlayer(playerOnTile);
            }
            else if (selectedPlayer != null && highlightedTile) //and playeronTile is null
            {

            }


            //return;
            //if (selectedPlayer == null)
            //{
            //    Manager.SelectPlayer(playerOnTile);
            //}
            //else
            //{
            //    if (playerOnTile != null && selectedPlayer != playerOnTile)
            //        playerOnTile.SelectUnit();
            //    //need to add a check for enemy players to attack
            //    else if (selectedPlayer != null && highlightedTile != null)
            //    {
            //        selectedPlayer.Move(tilePos);
            //    }
            //    else if (highlightedTile == null)
            //    {
            //        Manager.Instance.SetSelectedPlayer(null);
            //    }
            //}
        }
    }
}

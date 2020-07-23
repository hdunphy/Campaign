using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image PlayerIndicator;
    public List<Sprite> PlayerIndicatorSprites;

    private void Start()
    {
        EventManager.Instance.EndTurn += SetPlayerIndicatorImage;
        if(PlayerIndicatorSprites.Count > 0)
            PlayerIndicator.sprite = PlayerIndicatorSprites[0];
    }

    private void OnDestroy()
    {
        EventManager.Instance.EndTurn -= SetPlayerIndicatorImage;
    }

    public void SetPlayerIndicatorImage(PlayerColor color)
    {
        int playerColorIndex = (int)color;
        if (playerColorIndex < PlayerIndicatorSprites.Count)
            PlayerIndicator.sprite = PlayerIndicatorSprites[playerColorIndex];
    }
}

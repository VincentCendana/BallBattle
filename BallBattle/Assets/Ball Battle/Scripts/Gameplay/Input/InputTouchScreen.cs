using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputTouchScreen : MonoBehaviour, IPointerDownHandler
{
    [Header("Touch Input Components")]
    public RectTransform rect;                  // screen rect
    public LayerMask fieldLayer;                // game field's layer mask to be detected by the ray from player's touch screen input
    private RaycastHit hit;                     // raycast from touch input

    // event when player touched the screen
    public void OnPointerDown(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out hit, 100.0f, fieldLayer.value))
        {
            // spawn player at touch position if touch input hit the player's side of the field
            // spawn enemy at touch position if touch input hit the enemy's side of the field
            if (hit.transform.gameObject == GameManager.instance.soldierSpawner.playerSide)
            {
                GameManager.instance.soldierSpawner.SpawnPlayer(hit.point);
            }
            else if (hit.transform.gameObject == GameManager.instance.soldierSpawner.enemySide)
            {
                GameManager.instance.soldierSpawner.SpawnEnemy(hit.point);
            }
        }
    }
}

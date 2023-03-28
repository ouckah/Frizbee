using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayerNametag : NetworkBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Text nameText;
    [SerializeField] private float yOffset = 2f;
    [SerializeField] private Transform cameraTransform;

    private void Start()
    {
        if (IsLocalPlayer)
        {
            // Disable the nametag for the local player
            canvas.gameObject.SetActive(false);
        }
        else
        {
            // Set the name on the nametag to match the player's name
            nameText.text = EditPlayerName.Instance.GetPlayerName(); // TODO: fix this, it only sets text to current
                                                                     // TODO: player name (maybe setting name to PlayerObject instance)
        }
    }

    private void LateUpdate()
    {
        if (!IsLocalPlayer)
        {
            // Set the position of the nametag to hover above the player's head
            RectTransform rectTransform = canvas.GetComponent<RectTransform>();
            Vector3 playerPos = transform.position;
            Vector3 namePos = cameraTransform.GetComponent<Camera>().WorldToScreenPoint(new Vector3(playerPos.x, playerPos.y + yOffset, playerPos.z));
            rectTransform.position = namePos;
        }
    }
}
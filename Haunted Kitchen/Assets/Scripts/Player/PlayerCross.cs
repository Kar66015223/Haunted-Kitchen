using UnityEngine;

public class PlayerCross : MonoBehaviour
{
    private bool isHoldingCross;
    public bool IsHoldingCross => isHoldingCross;
    [SerializeField] private GameObject crossObj;

    private PlayerInputHandler inputHandler;
    [SerializeField] private PlayerAnimation playerAnim;
    private PlayerItem playerItem;
    private PlayerController_New controller;

    void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        playerAnim = GetComponent<PlayerAnimation>();
        playerItem = GetComponent<PlayerItem>();
        controller = GetComponent<PlayerController_New>();
    }

    void Update()
    {
        if (playerItem.currentHeldItemObj != null) 
            return;

        if (inputHandler != null)
            isHoldingCross = inputHandler.IsHoldingCross;

        if (isHoldingCross)
            controller.SetCanMove(false);

        if (!isHoldingCross)
            controller.SetCanMove(true);
    }

    public void HoldCross()
    {
        if (crossObj != null && playerAnim != null)
        {
            crossObj.SetActive(true);
            playerAnim.SetHoldOneHand(true);
        }

        Debug.Log("Player is holding cross");
    }

    public void PutCrossAway()
    {
        if (crossObj != null && playerAnim != null)
        {
            playerAnim.SetHoldOneHand(false);
            crossObj.SetActive(false);
        }
        
        Debug.Log("Player stopped holding cross");
    }
}

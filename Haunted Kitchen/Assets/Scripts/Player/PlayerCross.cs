using UnityEngine;

public class PlayerCross : MonoBehaviour
{
    private bool isHoldingCross;
    public bool IsHoldingCross => isHoldingCross;
    [SerializeField] private GameObject crossObj;

    private PlayerInputHandler inputHandler;
    [SerializeField] private PlayerAnimation playerAnim;
    private PlayerItem playerItem;

    void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        playerAnim = GetComponent<PlayerAnimation>();
        playerItem = GetComponent<PlayerItem>();
    }

    void Update()
    {
        if (playerItem.currentHeldItemObj != null) 
            return;
        
        if(inputHandler != null)
            isHoldingCross = inputHandler.IsHoldingCross;
    }

    public void HoldCross()
    {       
        if(crossObj != null && playerAnim != null)
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

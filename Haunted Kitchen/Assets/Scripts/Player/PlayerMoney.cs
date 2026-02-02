using Unity.VisualScripting;
using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    public int currentMoney;

    private void Start()
    {
        AddMoney(10000);
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
    }
}

using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject root;

    private void Awake()
    {
        root.SetActive(false);
    }

    public void Open()
    {
        root.SetActive(true);
    }

    public void Close()
    {
        root?.SetActive(false);
    }
}

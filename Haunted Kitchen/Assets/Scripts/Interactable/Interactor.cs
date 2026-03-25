using UnityEngine;

public class Interactor
{
    public GameObject source;
    public PlayerItem playerItem;
    public InteractionType interactionType;
    public Table currentTable;

    public Interactor(GameObject source, InteractionType type)
    {
        this.source = source;
        this.playerItem = source?.GetComponent<PlayerItem>();
        this.interactionType = type;
        this.currentTable = null;
    }

    public Interactor(GameObject source, InteractionType type, Table table) : this(source, type)
    {
        this.currentTable = table;
    }
}

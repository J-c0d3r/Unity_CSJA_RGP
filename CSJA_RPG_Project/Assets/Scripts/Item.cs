using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public Sprite icon;
    public string name;
    public float value;

    [System.Serializable]
    public enum Type
    {
        Potion, Elixir, Crystal
    }

    public Type ItemType;

    [System.Serializable]
    public enum SlotsType
    {
        helmet,
        shield,
        armor
    }

    public SlotsType SlotType;

    public Player player;

    public void GetAction()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        switch (ItemType)
        {
            case Type.Potion:
                //Debug.Log("Health +" + value);
                player.IncreaseStats(value, 0f);
                break;

            case Type.Elixir:
                Debug.Log("Elixir +" + value);
                break;

            case Type.Crystal:
                //Debug.Log("Crystal +" + value);
                player.IncreaseStats(0f, value);
                break;
        }
    }

    public void RemoveStats()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        switch (ItemType)
        {
            case Type.Potion:                
                player.DecreaseStats(value, 0f);
                break;

            case Type.Elixir:
                Debug.Log("Elixir +" + value);
                break;

            case Type.Crystal:                
                player.DecreaseStats(0f, value);
                break;
        }
    }

}

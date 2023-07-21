using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    public bool isOpen;

    public float colliderRadius;

    public List<Item> Items = new List<Item>();

    private Animator anim;


    void Start()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        GetPlayer();
    }

    void GetPlayer()
    {
        if (!isOpen)
        {
            foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward * colliderRadius), colliderRadius))
            {
                if (c.gameObject.CompareTag("Player"))
                {
                    if (Input.GetMouseButtonDown(0))
                        OpenChest();
                }
            }
        }
    }

    void OpenChest()
    {
        foreach (Item i in Items)
        {
            Inventory.instance.CreateItem(i); 
        }

        anim.SetTrigger("open");
        isOpen = true;
    }
}

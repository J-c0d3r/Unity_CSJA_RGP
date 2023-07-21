using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject inventoryBtn;
    public GameObject itemPrefab;

    public static GameController instance;

    void Awake()
    {
        instance = this;
    }

    public void ChangingStateGameObject(GameObject go)
    {
        var state = go.activeSelf;
        go.SetActive(!state);
    }

    public void ActiveGameObject(GameObject go)
    {
        go.SetActive(true);
        inventoryBtn.SetActive(false);
    }

    public void DisableGameObject(GameObject go)
    {
        go.SetActive(false);
        inventoryBtn.SetActive(true);
    }

}

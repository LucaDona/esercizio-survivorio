using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseMenu : MonoBehaviour
{

    [SerializeField]
    GameManager gameManager;

    public void restart()
    {
        gameManager.ReloadScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

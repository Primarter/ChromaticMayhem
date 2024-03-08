using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    [SerializeField] GameSceneManager gameSceneManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gameSceneManager.EndGame();
        }
    }
}

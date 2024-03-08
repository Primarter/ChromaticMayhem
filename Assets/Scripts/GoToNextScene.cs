using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToNextScene : MonoBehaviour
{
    private void Update() {
        if (Input.GetButtonDown("Dash"))
            SceneManager.LoadScene("Build Menu");
    }
}

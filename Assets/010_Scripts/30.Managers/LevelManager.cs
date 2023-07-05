using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("080_Scenes/Tutorial_Scene");
    }
}

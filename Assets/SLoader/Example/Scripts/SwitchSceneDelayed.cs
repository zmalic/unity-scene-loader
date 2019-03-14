using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLoader;

public class SwitchSceneDelayed : MonoBehaviour
{
    public int goToScene;
    public int delay = 5;

    void Start()
    {
        Invoke("SwitchScene", delay);
    }

    void SwitchScene()
    {
        SceneLoader.Instance?.LoadScene(goToScene);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrans : MonoBehaviour
{
    public int sceneNumber;
    public void TransitionScene(){
    SceneManager.LoadScene(sceneNumber);
}
}

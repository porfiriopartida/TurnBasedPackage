using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TransitionWithParameters 
{
    public static Dictionary<string, object> parameters;
    public static void Transition(string sceneName, Dictionary<string, object> parameters){
        TransitionWithParameters.parameters = parameters;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}

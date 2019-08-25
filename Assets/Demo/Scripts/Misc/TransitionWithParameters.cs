using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class TransitionWithParameters 
{
    public static bool isModeAdditive = true;
    public static Dictionary<string, object> EMPTY = new Dictionary<string, object>();
    public static Dictionary<string, object> parameters = new Dictionary<string, object>();
    public static void Transition(string sceneName, Dictionary<string, object> fromDictionary){
        if(fromDictionary == null){
            fromDictionary = EMPTY;
        }
        if(!isModeAdditive){
            //Not additive, just override the parameters.
            TransitionWithParameters.parameters = fromDictionary;   
        } else {
            //IF Additive, then we copy the values from the parameters to the current context, replace existing key-value pairs.
            Dictionary<string, object>.KeyCollection keys = fromDictionary.Keys;
            foreach(string k in keys){
                object dummyVal;
                fromDictionary.TryGetValue(k, out dummyVal);
                if(parameters.ContainsKey(k)){
                    parameters.Remove(k);
                }
                parameters.Add(k, dummyVal);
            }
        }

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}

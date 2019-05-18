using UnityEngine;
using UnityEngine.SceneManagement;

namespace TurnBasedPackage
{
    public class LoadSceneOnClick : MonoBehaviour
    {
        public string SceneName = "Main";
        public string context = "";

        void OnMouseDown()
        {
            SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
        }
    }

}
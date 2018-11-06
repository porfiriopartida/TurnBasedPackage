using UnityEngine;
using UnityEngine.SceneManagement;

namespace TurnBasedPackage
{
    public class LoadSceneOnClick : MonoBehaviour
    {
        public string SceneName = "Main";
        public string context = "";

        private void Start()
        {
            string gg = ContextManager.GetContextAttribute("SceneContext", "NA");
            Debug.Log(gg);
        }

        void OnMouseDown()
        {
            ContextManager.SetContextAttribute("SceneContext", context);
            SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
        }
    }

}
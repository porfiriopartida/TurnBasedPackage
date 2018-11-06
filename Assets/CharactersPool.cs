using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedPackage
{
    public class CharactersPool : MonoBehaviour
    {
        public CharacterPool[] characterPool;
        private Dictionary<string, GameObject> charactersMap;
        private GameObject dummy;
        public void InitMap()
        {
            charactersMap = new Dictionary<string, GameObject>();
            foreach (CharacterPool character in characterPool) {
                charactersMap.Add(character.key, character.prefab);
            }

        }
        public GameObject get(string key) {
            if (charactersMap.TryGetValue(key, out dummy))
            {
                return dummy;
            }
            return null;
        }
    }
}

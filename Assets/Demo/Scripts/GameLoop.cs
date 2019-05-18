using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TurnBasedPackage
{
    public class GameLoop : MonoBehaviour
    {
        public const string ZERO = "0";
        private ContextManager contextManager;
        public List<Character> Allies = new List<Character>();
        public List<Character> Enemies = new List<Character>();

        [System.NonSerialized]
        public Character EnemyTarget;
        [System.NonSerialized]
        public Character AllyTarget;
        [System.NonSerialized]
        public Character CharacterInTurn;

        //Prefabs
        public CharactersPool charactersPool;
        public GameObject _characterUIBarPrefab;

        //Post Events
        public List<EventNotifier> OnReadyEvents;
        public GameObject arrow;
        public GameObject target;

        // Use this for initialization
        void Start()
        {
            contextManager = ContextManager.GetInstance();

            //Mind the order of execution.
            contextManager.AddObserver(gameObject);

            charactersPool.InitMap();

            PrepareEnemies();

            PrepareAllies();

            PrepareBattle();

            DoneLoading();

            presetTargets();
        }
        private void presetTargets(){
            contextManager.SetEnemyTarget(contextManager.GetEnemyCharacters()[0]);
            contextManager.SetAllyTarget(contextManager.GetAllyCharacters()[0]);
        }
        private void DoneLoading() {
            foreach (EventNotifier en in OnReadyEvents) {
                Instantiate(en, transform);
            }
        }

        private void PrepareAllies()
        {
            string[] allies = new string[] { "blue", "cat", "blue" };

            Transform parent = GameObject.Find("allies").transform;
            CoordinatesController coordinates = parent.GetComponent<CoordinatesController>();
            
            for(int i = 0; i < allies.Length; i++)
            {
                GameObject newInstance = Instantiate(charactersPool.get(allies[i]), parent, false);
                newInstance.transform.localPosition = new Vector2(coordinates.points[i].x, coordinates.points[i].y);
                //Allies.Add(newInstance.GetComponent<Character>());
                Character newCharacter = newInstance.GetComponent<Character>();
                newCharacter.isAlly = true;
                Allies.Add(newCharacter);

                //Setup hp and energy bars.
                GameObject newUIInstance = Instantiate(_characterUIBarPrefab, newInstance.transform);
                newUIInstance.name = Character.STATUS_BAR_NAME;
                newCharacter.HierarchyUpdated();
            }
        }

        private void PrepareEnemies()
        {
            string[] enemies = new string[] { "cat", "red", "cat" };
            Transform parent = GameObject.Find("enemies").transform;
            CoordinatesController coordinates = parent.GetComponent<CoordinatesController>();

            for (int i = 0; i < enemies.Length; i++)
            {
                GameObject newInstance = Instantiate(charactersPool.get(enemies[i]), parent, false);
                newInstance.transform.localPosition = new Vector2(coordinates.points[i].x, coordinates.points[i].y);
                Character newCharacter = newInstance.GetComponent<Character>();
                newCharacter.isAlly = false;
                Enemies.Add(newCharacter);

                //Setup hp and energy bars.
                GameObject newUIInstance = Instantiate(_characterUIBarPrefab, newInstance.transform);
                newUIInstance.name = Character.STATUS_BAR_NAME;
                newCharacter.HierarchyUpdated();
            }
        }

        private void PrepareBattle() {

            string str = ContextManager.GetContextAttribute("SceneContext");
            Debug.Log(str + "IS STARTING.. ");

            contextManager.SET_CHARACTERS(Allies, Enemies);
            contextManager.SetAttributes(BaseCharacter.TURN_GAUGE, ZERO);
        }


        public void _GainTurnGauge()
        {
            contextManager.NotifyEvent("GainTurnGauge");
        }

        public void NextTurn()
        {
            while (!contextManager.HasCharacterInTurn() && contextManager.IsEnabled)
            {
                _GainTurnGauge();
            }
        }
        public void PrintTurnGauges()
        {
            List<Character> characters = contextManager.GetAllCharacters();
            foreach (Character c in characters)
            {
                Debug.Log(c.GetAttribute(BaseCharacter.TURN_GAUGE));
            }
        }
        public void TakeAction(int a) {
            if (!contextManager.HasCharacterInTurn())
            {
                Debug.Log("Character is required before taking action.");
                return;
            }

            Character c = contextManager.GetCharacterInTurn();
            //Debug.Log(string.Format("Character {0} is using their skill {1} ", c.getTag(), a));
            //contextManager.SetEnemyTarget(EnemyTarget);
            c.TakeAction(a);
        }

        public void TURN_STARTED(Character c)
        {
            System.Threading.Thread.Sleep(10);

            CharacterInTurn = c;
            if(c != null && c.isAlly){
                arrow.SetActive(false);
                arrow.GetComponent<TargetLookup>().movetoTarget(c.gameObject);
                arrow.SetActive(true);
                Debug.Log("TURN_STARTED : " + c.name);
            }
            else {
                arrow.SetActive(false);
                setNextTarget();
                if(contextManager.GetAllyTarget() != null){
                    TakeAction(1);
                    Debug.Log("TURN_STARTED AI Move : " + c.name);
                }
            }
        }

        public void setNextTarget(){
            Character randomAlly = contextManager.getRandomAlive(contextManager.GetAllyCharacters());
            if(randomAlly != null){
                contextManager.SetAllyTarget(randomAlly);
            }
        }

        protected void endTurn() {
            contextManager.TURN_ENDED();
        }
        
        public void TURN_ENDED(BaseCharacter character) {
            character.SetTurnGauge(0);
            character.SetAttribute(BaseCharacter.TURN_GAUGE, ZERO);
            List<Character> characters = contextManager.GetAllCharacters();
            foreach (Character c in characters) {
                if (c.IsReady()) {
                    //FIFO
                    contextManager.SetCharacterInTurn(c);
                    break;
                }
            }
            NextTurn();
        }

        // Update is called once per frame
        void Update()
        {
        }
        public void DefeatCharacter()
        {
            if (EnemyTarget != null && EnemyTarget.IsAlive)
            {

                EnemyTarget.Defeat();
            }
        }

        void ENCOUNTER_STARTED(ContextManager context)
        {
            List<Character> characters = context.GetAllCharacters();

            NextTurn();
        }
        void ENEMY_TARGET_CHANGED(Character character)
        {
            this.EnemyTarget = character;
            if(character != null && character.IsAlive){
                target.GetComponent<TargetLookup>().movetoTarget(EnemyTarget.gameObject);
                target.SetActive(true);
            } else {
                target.SetActive(false);
            }
        }
        void ALLY_TARGET_CHANGED(Character character)
        {
            this.AllyTarget = character;
        }
        public void CHARACTER_DEFEATED(Character character)
        {
            bool isAlly = character.isAlly;
            ContextManager manager = ContextManager.GetInstance();
            List<Character> targetsList = isAlly ? manager.GetAllyCharacters() : manager.GetEnemyCharacters();
            Character randomTarget = manager.getRandomAlive(targetsList);
            if(randomTarget == null){
                //All defeated
                Debug.Log( isAlly ? "DEFEAT!" : "VICTORY");
                return;
            }
            //Reset target for either side.
            if(isAlly){
                //if an ally was defeated, then an ally target is set.
                manager.SetAllyTarget(randomTarget);
            } else {
                manager.SetEnemyTarget(randomTarget);
            }
        }
    }
}

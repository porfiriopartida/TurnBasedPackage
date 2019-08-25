using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        private string ppreviousSceneName;

        // Use this for initialization
        void Start()
        {
            /** 
            * Register as observer to ContextManager
            * * List.add
            * Prepare Allies
            * Prepare Enemies
            * Send SET_CHARACTERS event to ContextManager
            * * Set Enemies, Set Allies
            * Notify Event ENCOUNTER_STARTED (sent to all listeners/allies/ through ContextManager)
            * * Notify Characters
            * * * Send message for each observer inside observerslist
            * * Notify Observers (like this GameLoop instance)
            * * Loop
            * * * Send message for each observer inside observerslist
            * 
            * * Context Manager uses the SendMessage unity method, which invokes the given method if it exists such as  "ENCOUNTER_STARTED()" for Characters that may
            * * do something special for such event.
            */
            contextManager = ContextManager.GetInstance();

            //Mind the order of execution.
            contextManager.AddObserver(gameObject);
            contextManager.FlipEnemies(true);

            charactersPool.InitMap();

            //Building allies and enemies from the characters pool.
            Dictionary<string, object> previousContext = TransitionWithParameters.parameters;
            object pEnemies = "NA";
            previousContext.TryGetValue("enemies", out pEnemies);
            object ppreviousSceneNameO;
            
            if(previousContext.TryGetValue("previousSceneName", out ppreviousSceneNameO)){
                ppreviousSceneName = (string)ppreviousSceneNameO;
            } else {
                ppreviousSceneName = SceneManager.GetActiveScene().name;
            }
            
            //context.Add("previousScene", SceneManager.GetActiveScene().name); //This context must be string to obj maybe?
            //context.Add("enemies", new string[]{"cat", "cat", "cat"}); //This context must be string to obj maybe?

            Debug.Log("previousScene: " + ppreviousSceneName);
            PrepareAllies();

            PrepareEnemies((string[])pEnemies);

            PrepareBattle();

            //Starts the Event Notifier.
            DoneLoading();

            presetTargets();
        }
        /* 
        <summary>
        Set the Target contexts, Enemies will focus the Ally target and Allies will focus the enemy target.
        </summary> 
        */
        private void presetTargets(){
            contextManager.SetEnemyTarget(contextManager.GetEnemyCharacters()[0]);
            contextManager.SetAllyTarget(contextManager.GetAllyCharacters()[0]);
        }
        /* 
        <summary>
        Sends to the ContextManager the EventName and then removes them from the Unity scene if Auto Remove is checked.
        This sends the ENCOUNTER_STARTED once after all the pre setup is done.
        </summary> 
        */
        private void DoneLoading() {
            foreach (EventNotifier en in OnReadyEvents) {
                Instantiate(en, transform);
            }
        }

        private void PrepareAllies(){
            Transform parent = GameObject.Find("allies").transform;
            
            Dictionary<string, object> previousContext = TransitionWithParameters.parameters;
            object pPreviousStates = "NA";
            State[] previousStates = null;
            if(previousContext.TryGetValue("allies", out pPreviousStates)){
                //passing the previous context of characters
                previousStates = (State[]) pPreviousStates;
                PrepareCharacters(parent, Allies, previousStates);
            } else {
                string[] alies = new string[] { "blue", "frosty", "blue" };
                PrepareCharacters(parent, Allies, alies);
            }

        }
        private void PrepareEnemies(string[] enemies){
            Transform parent = GameObject.Find("enemies").transform;

            //What if enemies is null/empty? should we go back to the previous screen if any?
            //What if I want to debug only the battle scene?
            if(enemies == null || enemies.Length == 0){
                enemies = new string[] { "cat", "red", "cat" };
            }
            
            PrepareCharacters(parent, Enemies, enemies);
        }
        /* 
            <summary>
            Given an array of string names, the context sets allies if they exist in the CharacterPool instance as prefabs.
            Creates a game object based on the previous lookup.
            Created characters are then marked as isAlly = true and added to the Allies List
            The UI status bars are also added here.
            </summary>
            <param name="parent">Transform to hold the new game object.</param>
            <param name="parentList">List<Character> the list for reference of type of character (allies or enemies)</param>
            <param name="characters">string[] Array of character names to lookup in the prefabs characters pool</param>
            <param name="isAlly">Boolean to set the character as Ally (true) or Enemey (false)</param>
        */
        private void PrepareCharacters(Transform parent, List<Character> parentList, string[] characters)
        {
            CoordinatesController coordinates = parent.GetComponent<CoordinatesController>();
            for(int i = 0; i < characters.Length; i++)
            {
                //TODO: What if the character does not exist in the pool?
                String characterKey = characters[i];
                GameObject characterReference = charactersPool.get(characterKey);
                //Character newCharacter = characterReference.GetComponent<Character>();
                //newCharacter.isAlly = false;//this is not being saved?
                GameObject newInstance = Instantiate(characterReference, parent, false);
                newInstance.transform.localPosition = new Vector2(coordinates.points[i].x, coordinates.points[i].y);
                Character newCharacter = newInstance.GetComponent<Character>();
                newCharacter.CurrentHealth = newCharacter.MaxHealth;
                newCharacter.IsAlive = true;
                parentList.Add(newCharacter);
                
                //Setup hp and energy bars.
                newInstance.transform.Find("STATUS_BAR");
                GameObject newUIInstance = Instantiate(_characterUIBarPrefab, newInstance.transform);
                Transform statusBarPosition =  newInstance.transform.Find("StatusBarPosition");
                newUIInstance.name = Character.STATUS_BAR_NAME;
                if(statusBarPosition != null){
                    newUIInstance.transform.position = statusBarPosition.transform.position;
                }
                newCharacter.HierarchyUpdated();
            }
        }
        private void PrepareCharacters(Transform parent, List<Character> parentList, State[] previousState)
        {
            CoordinatesController coordinates = parent.GetComponent<CoordinatesController>();
            for(int i = 0; i < previousState.Length; i++)
            {
                State state = previousState[i];
                //TODO: What if the character does not exist in the pool?
                String characterKey = state.Tag;
                GameObject characterReference = charactersPool.get(characterKey);
                //Character newCharacter = characterReference.GetComponent<Character>();
                //newCharacter.isAlly = false;//this is not being saved?
                GameObject newInstance = Instantiate(characterReference, parent, false);
                newInstance.transform.localPosition = new Vector2(coordinates.points[i].x, coordinates.points[i].y);
                Character newCharacter = newInstance.GetComponent<Character>();
                //Just storing our current GameObjects is not possible, we will need to setup a "State" object instead.
                // Just passing the GameObject/Controller will end up in MissingReferenceException: The object of type 'XXController' 
                //Testing with CurrentHealths.
                state.loadTo(newCharacter);

                parentList.Add(newCharacter);
                
                if(state.IsAlive){
                    //TODO: We still need the dead one reference.
                    //Setup hp and energy bars.
                    newInstance.transform.Find("STATUS_BAR");
                    GameObject newUIInstance = Instantiate(_characterUIBarPrefab, newInstance.transform);
                    newUIInstance.name = Character.STATUS_BAR_NAME;
                    newCharacter.HierarchyUpdated();
                    continue;
                }
            }
        }

        /* 
            <summary>
            Submits the SET_CHARACTERS event with Allies and Enemies in context
            </summary>
        */
        private void PrepareBattle() {

            string str = ContextManager.GetContextAttribute("SceneContext");
            Debug.Log(str + "IS STARTING.. ");

            contextManager.SET_CHARACTERS(Allies, Enemies);
            contextManager.SetAttributes(BaseCharacter.TURN_GAUGE, ZERO); //TODO: Add to doc. This sets the turn meter to 0.
        }


        public void _GainTurnGauge()
        {
            contextManager.NotifyEvent("GainTurnGauge");
        }

        public void NextTurn()
        {
            while (contextManager.CanTriggerNextTurn())
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

        //Listen to Allies turn started, enemies will be handled in AIController.
        public void TURN_STARTED(Character c)
        {
            if (c == null || !c.isAlly)
            {
                return;
            }

            //System.Threading.Thread.Sleep(10);

            CharacterInTurn = c;
            Debug.Log("TURN_STARTED : " + c.name + ". IsAlly: " + c.isAlly);
            if(c != null && c.isAlly){
                arrow.SetActive(false);
                arrow.GetComponent<TargetLookup>().movetoTarget(c.gameObject);
                arrow.SetActive(true);
            }
            /* 
            else {
                arrow.SetActive(false);
                setNextTarget();
                if(contextManager.GetAllyTarget() != null){
                    Debug.Log("TURN_STARTED AI Move : " + c.name);
                    TakeAction(1);
                }
            }*/
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
                    return;
                }
            }
            //Only call next in turn if no one was ready before.
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
                bool isVictory = !isAlly;
                Debug.Log( isAlly ? "DEFEAT!" : "VICTORY");
                //Stop all turn meter gauge gaining.
                this.gameObject.SetActive(false); 

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("allies", buildAlliesState());
                parameters.Add("isVictory", isVictory);
                TransitionWithParameters.Transition(ppreviousSceneName, parameters);
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
        private object buildAlliesState(){
            State[] states = new State[Allies.Count];
            for(int i=0;i<states.Length; i++){
                states[i] = State.buildFromCharacter(Allies[i]);
            }
            return states;
        }
    }
}

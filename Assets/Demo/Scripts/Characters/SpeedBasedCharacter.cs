/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedPackage
{
    public abstract class SpeedBasedCharacter : Character
    {
        public AudioClip attack;
        public const string TURN_GAUGE = "TURN_GAUGE";
        public int speed = 1;

        public const int MAX_TURN_GAUGE = 1000;
        private Dictionary<string, string> attributes = new Dictionary<string, string>();
        public Sprite thumbnail;

        void Awake()
        {
            CurrentHealth = MaxHealth;
            this.uuid = System.Guid.NewGuid();
            audioSource = GetComponent<AudioSource>();
        }
        
        public void AddDamage(int dmg)
        {
            SetHp(this.CurrentHealth - dmg);
        }
        public void AddHealth(int hp)
        {
            SetHp(this.CurrentHealth + hp);
        }
        public void GainTurnGauge()
        {
            int turnGauge = GetTurnGauge();
            turnGauge += getSpeed();
            SetTurnGauge(turnGauge);
            if (IsReady()) {
                ContextManager.GetInstance().SetCharacterInTurn(this);
            }

            //ContextManager.GetInstance().NotifyEvent("TURN_GAUGE_GAINED", this);
            //Debug.Log((!isAlly ? "Enemy " : "Ally") + "[" + getTag() + "] gained turn gauge with speed [" + speed + "] ... " + GetTurnGauge());
        }

        public override bool IsReady()
        {
            return GetTurnGauge() >= MAX_TURN_GAUGE && IsAlive;
        }

        public void TURN_GAUGE_GAINED(Character c) {
            //Debug.Log(c.getTag() + " gained turn gauge.");
        }


        public int getSpeed()
        {
            return speed;
        }

        public override void TakeAction(int a)
        {
            //Debug.Log(string.Format("{0} used {1}", getTag(), a));
            string action = "N/A";
            bool executed = false;
            switch (a)
            {
                case 1:
                    action = "Basic Attack";
                    executed = BasicAttack();
                    break;
            }

            Debug.Log(string.Format("{0} used {1} - {2}", getTag(), action, executed));

            //This must be called from the end of the animation.
            //EndTurn();
        }
        public int GetTurnGauge()
        {
            string turnGaugeStr = GetAttribute(TURN_GAUGE, "0");
            return int.Parse(turnGaugeStr);
        }
        public void SetTurnGauge(int turnGauge)
        {
            SetAttribute(TURN_GAUGE, turnGauge.ToString());
            updateTurnBar((float)turnGauge / MAX_TURN_GAUGE);
        }
        public void updateTurnBar(float val)
        {

            if (statusBar != null)
            {
                statusBar.updateTurnBar(val);
            }

        }

        public override string GetAttribute(string key) {
            return this.GetAttribute(key, "");
        }
        public override string GetAttribute(string key, string defaultValue)
        {
            string outStr;
            bool hasVal = attributes.TryGetValue(key, out outStr);
            return hasVal ? outStr : defaultValue;
        }
        public override void SetAttribute(string key, string val)
        {
            if (attributes.ContainsKey(key))
            {
                attributes[key] = val;
            }
            else
            {
                attributes.Add(key, val);
            }
        }
        
        public virtual bool BasicAttack()
        {
            ContextManager manager = ContextManager.GetInstance();

            Character target = (Character)( isAlly ? manager.GetEnemyTarget() : manager.GetAllyTarget() );

            if (target != null && target.IsAlive) {
                TriggerAnimation("BasicAttack"); //TODO: Make a constant?
                playSFX(attack);
                target.AddDamage(Atk);
                return true;
            }

            return false;
        }
        public void TriggerAnimation(string anim){
            Animator animator = GetComponent<Animator>();
            if(animator == null){
                animator = GetComponentInChildren<Animator>();
            }
            if(animator != null){
                animator.SetTrigger(anim);
            }
        }
    }

} */
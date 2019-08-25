
namespace TurnBasedPackage
{
    public class State 
    {
        public string Tag;
        public int MaxHealth;
        public int CurrentHealth ;
        public bool IsAlive;

        public void loadTo(Character character){
            character.IsAlive = IsAlive;
            character.CurrentHealth = CurrentHealth;
        }
        public static State buildFromCharacter(Character character){
            State s = new State();
            s.CurrentHealth = character.CurrentHealth;
            s.MaxHealth = character.MaxHealth;
            s.IsAlive = character.IsAlive;
            s.Tag = character.getTag();
            return s;
        }
    }
}
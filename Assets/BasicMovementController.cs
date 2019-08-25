using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TurnBasedPackage;
public class BasicMovementController : MonoBehaviour
{
    void Start(){
        //TODO: Check context position so we are rendered where we left.
    }
    public float speed = 1f;
    private bool inGrass = false;

    private Rigidbody2D rb;

    // Update is called once per frame
    void Update()
    {
        rb = GetComponent<Rigidbody2D>();

        if(Input.GetAxis("Horizontal") != 0){
            moveH(Input.GetAxis("Horizontal") > 0);
        } else if(Input.GetAxis("Vertical") != 0){
            moveV(Input.GetAxis("Vertical") > 0);
        }
    }
    public void moveH(bool positive){
        rb.MovePosition(transform.position + ( transform.right * (speed * ( positive ? 1:-1)) * Time.fixedDeltaTime));
        registerWalking();
    }
    public void moveV(bool positive){
        rb.MovePosition(transform.position + ( transform.up * (speed * ( positive ? 1:-1)) * Time.fixedDeltaTime));
        registerWalking();
    }
    public void setInGrass(bool inGrass){
        this.inGrass = inGrass;
    }
    private int grassWalking = 0;
    public void registerWalking(){
        if(this.inGrass){
            grassWalking++;
            if(grassWalking < 10) {
                return;
            }
            Debug.Log(this.name + " is walking in grass.");
            //After 10 or so steps we will find an enemy.
            //Once enemy is found, we will transition to the next scene with the given parameters.
            Dictionary<string, object> context = new Dictionary<string, object>();
            
            context.Add("previousSceneName", SceneManager.GetActiveScene().name); //This context must be string to obj maybe?
            int random = ContextManager.getRandom(5); //0,1,2,3,4
            string[] enemiesArray = null;
            Debug.Log("Case - " + random);
            switch(random){
                case 0:
                case 1:
                enemiesArray = new string[]{"cat"};
                break;
                case 2:
                enemiesArray = new string[]{"red", "cat"};
                break;
                case 3:
                enemiesArray = new string[]{"red", "red"};
                break;
                case 4:
                enemiesArray = new string[]{"cat", "cat", "cat"};
                break;

            }
            context.Add("enemies", enemiesArray); //This context must be string to obj maybe?

            TransitionWithParameters.Transition("BattleScene", context);
        } else {
            grassWalking = 0;
        }
    }
}

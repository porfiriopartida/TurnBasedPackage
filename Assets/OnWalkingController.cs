using UnityEngine;

public class OnWalkingController : MonoBehaviour
{
    public void OnTriggerEnter2D (Collider2D other){
        Debug.Log(other.name + " has entered the grass.");
        BasicMovementController bmc = other.GetComponent<BasicMovementController>();
        bmc.setInGrass(true);
    }
    public void OnTriggerExit2D(Collider2D other){
        Debug.Log(other.name + " has exited the grass.");
        BasicMovementController bmc = other.GetComponent<BasicMovementController>();
        bmc.setInGrass(false);
    }
}

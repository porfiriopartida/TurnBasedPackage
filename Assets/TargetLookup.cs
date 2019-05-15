using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLookup : MonoBehaviour
{
    public GameObject target;
    public float offsetX = 0f;
    public float offsetY = 0f;
    
    public void Start(){
        movetoTarget(target);
    }
    public void movetoTarget(GameObject target){
        if(target ==null || target.transform == null){
            gameObject.SetActive(false);
            return;
        }
        this.target = target;
        this.transform.parent = target.transform;

        float x = target.transform.position.x + offsetX;
        float y = target.transform.position.y + offsetY;
        float z = target.transform.position.z;

        this.transform.position = new Vector3(x, y, z);
    }
}

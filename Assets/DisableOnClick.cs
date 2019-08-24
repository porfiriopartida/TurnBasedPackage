using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnClick : MonoBehaviour
{
    public void onClick(){
        gameObject.SetActive(false);
    }
}

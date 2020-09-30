using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class PoolBehaviour : MonoBehaviour
{
    
    public System.Action onDestroy;

    void OnDestroy(){
        if(onDestroy != null){
            onDestroy();
        }
    }
}

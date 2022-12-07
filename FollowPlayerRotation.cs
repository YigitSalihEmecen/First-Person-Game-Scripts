using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerRotation : MonoBehaviour
{
    
    public Transform playerRotation;
    
    void Update()
    {
        gameObject.transform.eulerAngles = new Vector3(0, playerRotation.eulerAngles.y, 0);
    }
}

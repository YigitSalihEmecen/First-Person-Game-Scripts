using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SkyboxRotate : MonoBehaviour
{
    void Update()
    {
        gameObject.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y + 3 * Time.deltaTime, 0);
    }
}

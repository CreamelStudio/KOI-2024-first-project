using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyMove : MonoBehaviour
{

    public Transform Pmove;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2((Pmove.position.x - 2) / 4, 4.23f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YRotate : MonoBehaviour
{
    public float rotSpeed;
  
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(transform.up * rotSpeed);
    }
}

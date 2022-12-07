using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    // Start is called before the first frame update
    float rotation = 0;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
  {

    transform.Rotate(Vector3.up); 
  }
}

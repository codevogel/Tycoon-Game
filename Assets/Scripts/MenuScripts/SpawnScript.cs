using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{

  public GameObject cube;
  // Update is called once per frame
  void Update()
  {
    if (Input.GetMouseButton(1))
    {

      Instantiate(cube, new Vector3(Input.mousePosition.x,Input.mousePosition.y,0f), Quaternion.identity);
    };

  }
}

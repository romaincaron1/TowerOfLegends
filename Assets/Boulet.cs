using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulet : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Destroy(gameObject, 3);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Terrain")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHover : MonoBehaviour
{
    [SerializeField] GameObject BloodSprite;
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            //Instantiate(BloodSprite, new Vector3(i * 2.0F, 0, 0), Quaternion.identity);
        }
    }

    void Update()
    {
        
    }
}

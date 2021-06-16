using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundFill : MonoBehaviour
{
    [Header("Options")]     
    [SerializeField] float Speed;
    [SerializeField] bool FillingByLocal;
    [SerializeField] bool FillingByGlobal;

    HoverManager Manager;
    void Start()
    {
        Manager = FindObjectOfType<HoverManager>();
    }

    public void Fill(bool Overwrite)
    {
        if (Overwrite)
        {
            FillingByLocal = true;
        }
        else
        {
            FillingByGlobal = true;
        }
    }
    public void Clear(bool Overwrite)
    {
        if (Overwrite)
        {
            FillingByLocal = false;
        }
        else
        {
            FillingByGlobal = false;
        }
    }
    void Update()
    {
        if (FillingByLocal)
        {
            gameObject.GetComponent<Image>().fillAmount += Speed * 0.01f;
        }
        else if(!FillingByLocal && !FillingByGlobal)
        {
           gameObject.GetComponent<Image>().fillAmount -= Speed * 0.01f;
        }

        if (FillingByGlobal)
        {
            gameObject.GetComponent<Image>().fillAmount += Manager.HoverSpeed * 0.01f;
        }
        else if (!FillingByLocal && !FillingByGlobal)
        {
            gameObject.GetComponent<Image>().fillAmount -= Manager.HoverSpeed * 0.01f;
        }

    }
}

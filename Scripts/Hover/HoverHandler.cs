using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class HoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    [Header("Canvas")]
    [SerializeField] GameObject Canvas;
    [SerializeField] bool OverwriteGlobal;

    [Header("Background")]
    [SerializeField] GameObject BackGround;

    void Start()
    {
        Canvas = GameObject.FindGameObjectWithTag("Canvas");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayHoverSound();
        if(BackGround != null)
        {
            BackGround.GetComponent<BackGroundFill>().Fill(OverwriteGlobal);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(BackGround != null)
        {
            BackGround.GetComponent<BackGroundFill>().Clear(OverwriteGlobal);
        }
    }
    void PlayHoverSound()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(Canvas.GetComponent<AudioManager>().HoverSound());
    }

}

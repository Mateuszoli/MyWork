using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    [Header("SubPanels")]
    [SerializeField] GameObject[] Panels;
    void Start()
    {
        
    }
    public void UpdateVolume(float vol)
    {
        AudioListener.volume = vol;
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void DebLog()
    {
        Debug.Log("Button Works");
    }
    public void ActivatePanel(GameObject Panel)
    {
        Panel.SetActive(true);
    }
    public void DeactivatePanel(GameObject Panel)
    {
        Panel.SetActive(false);
    }
    public void DeactivateAll(GameObject PanelParent)
    {
        for(int i = 0; i < PanelParent.transform.childCount; i++)
        {
            PanelParent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void ActivateAll(GameObject PanelParent)
    {
        for (int i = 0; i < PanelParent.transform.childCount; i++)
        {
            PanelParent.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    void Update()
    {
        
    }
}

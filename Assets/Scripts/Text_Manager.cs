using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Text_Manager : MonoBehaviour 
{
    [SerializeField]
    GameObject pause_object;

    public void Set_Text(string pText)
    {
        this.gameObject.GetComponent<TMP_Text>().SetText(pText);
    }

    public void start_countdown(GameObject pGameObject)
    {
        StartCoroutine(do_countdown(pGameObject));
    }

    //TODO better link this countdown to the activation of the swords scripts
    IEnumerator do_countdown(GameObject pGameObject)
    {
        Set_Text("3");
        yield return new WaitForSecondsRealtime(1.0f);
        Set_Text("2");
        yield return new WaitForSecondsRealtime(1.0f);
        Set_Text("1");
        yield return new WaitForSecondsRealtime(1.0f);
        pGameObject.GetComponentInChildren<Rotate_Texture>().enabled = true;
        pGameObject.GetComponentInChildren<Mouse_Control_Rotation>().enabled = true;
        pGameObject.transform.GetChild(2).gameObject.SetActive(true);
        pause_object.SetActive(true);
    }
}

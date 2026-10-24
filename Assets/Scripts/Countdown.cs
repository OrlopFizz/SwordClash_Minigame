using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    //TODO bruh i hate this. 
    //this script will change the value of the main window, and when it ends, will enable the swords objects.
    [SerializeField]
    GameObject main_window;
    [SerializeField]
    GameObject swords;
    [SerializeField]
    GameObject loss_bar;
    [SerializeField]
    GameObject restart_button;
    [SerializeField]
    GameObject score_keeper;
    
    Difficulty_Manager difficulty_manager;

    // Start is called before the first frame update
    void Start()
    {
        reset_scene();
    }

    public void reset_scene()
    {
        difficulty_manager = GameObject.Find("Difficulty_Container").GetComponent<Difficulty_Manager>();
        //set default rotation for player sword
        swords.transform.GetChild(0).localRotation = Quaternion.Euler(0.0f, 0.0f, 135.0f);
        //set default rotation for ai sword
        swords.transform.GetChild(1).localRotation = Quaternion.Euler(0.0f, 0.0f, 45.0f);
        //set default scale for loss bar
        score_keeper.GetComponent<Score>().scale_current = 13.0f;
        loss_bar.transform.localScale = new Vector3(3.0f, 13.0f, 0.0f);

        main_window.SetActive(true);
        main_window.GetComponent<TMP_Text>().enabled = true;
        main_window.GetComponent<Text_Manager>().Set_Text("");
        restart_button.SetActive(false);
        main_window.GetComponent<Text_Manager>().start_countdown(swords);
        difficulty_manager.disable_buttons(false);
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    //Score bar components
    [SerializeField]
    GameObject Loss_Bar;
    [SerializeField]
    TMP_Text win_screen;
    [SerializeField]
    Button restart_button;
    [SerializeField]
    GameObject main_window;
    [SerializeField]
    GameObject countdown;
    [SerializeField]
    TMP_Text ai_name;

    float scale_100 = 26.0f;
    float scale_0 = 0.0f;
    public float scale_current = 13.0f;
    [SerializeField]
    float scale_speed;

    Text_Manager text_manager;
    Difficulty_Manager difficulty_manager;
    // Start is called before the first frame update
    void Start()
    {
        restart_button.onClick.AddListener(onClick);
        text_manager = main_window.GetComponent<Text_Manager>();
        difficulty_manager = GameObject.Find("Difficulty_Container").GetComponent<Difficulty_Manager>();
    }

    void onClick()
    {
        countdown.GetComponent<Countdown>().reset_scene();
    }

    // Update is called once per frame
    void Update()
    {
        //check if anyone has won
        if (scale_current >= scale_100) {
            text_manager.Set_Text("Player Won");
            //enable restart button TODO DRY violated
            restart_button.gameObject.SetActive(true);
            difficulty_manager.disable_buttons(true);
        }
        if (scale_current <= scale_0) {
            text_manager.Set_Text("Player Lost");
            //enable restart button TODO DRY violated lmao this comment also violates DRY technically
            restart_button.gameObject.SetActive(true);
            difficulty_manager.disable_buttons(true);
        }
        //disable all other scripts
    }


    public void increase_scale_bar()
    {
        if (scale_current < scale_100)
        {
            scale_current += scale_speed * Time.deltaTime;
            Loss_Bar.transform.localScale = new Vector3(3.0f, scale_current, 0.0f);
        }
    }
    
    public void decrease_scale_bar()
    {
        if (scale_current > scale_0)
        {
            scale_current -= scale_speed * Time.deltaTime;
            Loss_Bar.transform.localScale = new Vector3(3.0f, scale_current, 0.0f);
        }
    }

    public char check_won_status()
    {
        if (scale_current >= scale_100)
        {
            return 'w';
        }
        if (scale_current <= scale_0)
        {
            return 'l';
        }
        return 'o';
        
    }
}

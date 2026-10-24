using UnityEngine;
using UnityEngine.UI;

public class Start_Game : MonoBehaviour
{
    //this class will manage the button and enable the game.
    //when the game is enabled, we also disable the main menu, including the button
    [SerializeField]
    GameObject Main_Menu;
    [SerializeField]
    GameObject Main_Game_Objects;
    [SerializeField]
    GameObject main_music_gameobject;
    [SerializeField]
    GameObject difficulty_buttons_container;
    [SerializeField]
    GameObject back_object;

    Music_Handler main_music_manager;
    Difficulty_Manager difficulty_manager;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(start_game);
        main_music_manager = main_music_gameobject.GetComponent<Music_Handler>();
        difficulty_manager = difficulty_buttons_container.GetComponent<Difficulty_Manager>();

        //set configuration framerates
        QualitySettings.vSyncCount = 0; 
        Application.targetFrameRate = 60;
    }

    void start_game()
    {
        difficulty_manager.disable_buttons(false);
        Main_Menu.SetActive(false);
        Main_Game_Objects.SetActive(true);
        main_music_manager.switch_state();
        back_object.SetActive(true);
    }
}

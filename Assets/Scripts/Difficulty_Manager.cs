using UnityEngine;
using UnityEngine.UI;

public class Difficulty_Manager : MonoBehaviour
{
    GameObject Easy_Button;
    GameObject Medium_Button;
    GameObject Hard_Button;
    GameObject Mute_Button;
    GameObject Music_Player;

    [SerializeField]
    GameObject Effect_Player;

    public string chosen_difficulty;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chosen_difficulty = "easy";
        Easy_Button = GameObject.Find("Easy");
        Medium_Button = GameObject.Find("Medium");
        Hard_Button = GameObject.Find("Hard");
        Mute_Button = GameObject.Find("Mute");
        Music_Player = GameObject.Find("Music_Source");
        
        Easy_Button.GetComponent<Button>().onClick.AddListener(() => {
            chosen_difficulty = "easy";
        });
        Medium_Button.GetComponent<Button>().onClick.AddListener(() =>
        {
            chosen_difficulty = "medium";
        });
        Hard_Button.GetComponent<Button>().onClick.AddListener(() => {
            chosen_difficulty = "hard";
        });

        Mute_Button.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (Music_Player.activeSelf)
            {
                Music_Player.SetActive(false);
            }
            else
            {
                Music_Player.SetActive(true);
            }

            if (Effect_Player.activeSelf)
            {
                Effect_Player.SetActive(false);
            }
            else
            {
                Effect_Player.SetActive(true);
            }
        });
    }

    public void disable_buttons(bool value)
    {
        Easy_Button.SetActive(value);
        Medium_Button.SetActive(value);
        Hard_Button.SetActive(value);
    }
}

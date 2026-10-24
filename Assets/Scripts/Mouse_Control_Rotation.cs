using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.UI;

public class Mouse_Control_Rotation : MonoBehaviour
{
    private Camera cam;

    [SerializeField]
    GameObject object_to_battle;
    [SerializeField]
    ParticleSystem ps;
    [SerializeField]
    GameObject score_keeper;
    [SerializeField]
    TMP_Text diff_angle_text;
    [SerializeField]
    TMP_Text text_angle_player;
    [SerializeField]
    TMP_Text text_angle_ai;

    [SerializeField]
    TMP_Text player_heading_text;
    [SerializeField]
    TMP_Text ai_heading_text;
    [SerializeField]
    TMP_Text diff_angle_2_text;
    
    [SerializeField]
    AudioSource audio_system;

    [SerializeField]
    Slider volume;

    [SerializeField]
    GameObject pause_object;

    InputAction rotate_action;

    Score score_script;

    bool play_ps = false;
    // Start is called before the first frame update
    void Start()
    {
        ps.Clear();
        ps.Stop();
        cam = Camera.main;
        score_script = score_keeper.GetComponent<Score>();
        ps.gameObject.SetActive(true);
        audio_system.gameObject.SetActive(true);

        rotate_action = InputSystem.actions.FindAction("Move_Sword");

        volume.onValueChanged.AddListener(set_volume);
    }

    // Update is called once per frame
    void Update()
    {
        //rotate to always point to the mouse
        Vector3 mouse_pos = new Vector3(rotate_action.ReadValue<Vector2>().x, rotate_action.ReadValue<Vector2>().y);
        mouse_pos = cam.ScreenToWorldPoint(mouse_pos);
        Vector3 diff = mouse_pos - transform.position;
        float rotationz = (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg) + 315;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationz);

        //calculate heading angles of both objects
        double player_radians = transform.localEulerAngles.z * (Math.PI / 180);
        Vector2 player_heading = new Vector2((float)Math.Cos(player_radians), (float)Math.Sin(player_radians));
        double ai_radians = object_to_battle.transform.localEulerAngles.z * (Math.PI / 180);
        Vector2 ai_heading = new Vector2((float)Math.Cos(ai_radians), (float)Math.Sin(ai_radians));

        float diff_angle = Vector2.Angle(ai_heading, player_heading); //calculate the angle between vectors. Keep in mind this 

        if (65.0f <= diff_angle && diff_angle <= 115.0f)
        {
            if (!play_ps)
            { //no esta corriendo la animacion

                play_ps = true; //start playing
                ps.Play();
                audio_system.Play();
            }
            score_script.increase_scale_bar();
        }
        else
        { //esta afuera de la zona hecha para detectar 
            if (play_ps)
            { //is it playing?
                play_ps = false; //stop playing
                ps.Clear();
                ps.Stop();
                audio_system.Stop();
            }
            score_script.decrease_scale_bar();
        }

        //dibujar el angulo en los procesos
        diff_angle_text.text = "Diff Angle: " + diff_angle;
        text_angle_player.text = "Angle Player: " + transform.localEulerAngles.z;
        text_angle_ai.text = "Angle AI: " + object_to_battle.transform.localEulerAngles.z;

        //calculate heading angle and write it into the debug text
        player_heading_text.text =String.Format("({0:F2},{1:F2})", player_heading[0], player_heading[1]);
        ai_heading_text.text = String.Format("({0:F2}, {1:F2})", ai_heading[0], ai_heading[1]);

        float diff_angle_2 = Vector2.Angle(ai_heading, player_heading);
        diff_angle_2_text.text = String.Format("{0:F2}", diff_angle_2);

        //check if anyone has won
        char result = score_script.check_won_status();
        if (result != 'o') //someone won
        {
            //stop particle system
            ps.Clear();
            ps.Stop();
            play_ps = false;

            //stop audio system
            audio_system.Stop();

            //disable all battle scripts
            object_to_battle.GetComponent<Rotate_Texture>().generated_name = "";
            object_to_battle.GetComponent<Rotate_Texture>().enabled = false;
            this.enabled = false;

            //disable pausing script
            pause_object.SetActive(false);
        }
    }

    void set_volume(float value)
    {
        audio_system.volume = value;
    }
}

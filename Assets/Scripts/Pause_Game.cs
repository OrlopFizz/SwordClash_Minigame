using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

//When the player presses the ESC key, the game should pause. Disable texture rotations and audio.
public class Pause_Game : MonoBehaviour
{
    [SerializeField]
    GameObject main_text;
    [SerializeField]
    GameObject player_sword;
    [SerializeField]
    GameObject ai_sword;
    [SerializeField]
    AudioSource music_source;
    [SerializeField]
    AudioSource audio_source;
    [SerializeField]
    ParticleSystem clash_particle_system;

    bool paused;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            paused = !paused;
        }
        main_text.GetComponent<TMP_Text>().text = paused ? "Paused" : ""; 
        player_sword.GetComponent<Mouse_Control_Rotation>().enabled = !paused;
        ai_sword.GetComponent<Rotate_Texture>().enabled = !paused;
        music_source.gameObject.SetActive(!paused);
        audio_source.gameObject.SetActive(!paused);
        clash_particle_system.gameObject.SetActive(!paused);
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music_Handler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    AudioClip main_menu;
    [SerializeField]
    AudioClip action_1;
    [SerializeField]
    AudioClip action_2;
    [SerializeField]
    AudioClip action_3;
    [SerializeField]
    AudioClip action_4;
    [SerializeField]
    AudioClip action_5;

    [SerializeField]
    Slider volume;

    AudioSource audio_source;
    List<AudioClip> audio_list = new List<AudioClip>();

    //This element should manages two states, The main menu music and the gameplay music.
    //The main menu music is always the same, but gameplay music should be chosen at random at the start of EVERY GAME.

    public void switch_state()
    {
        audio_source.clip = choose_random_music();
        audio_source.Play();
        Debug.Log(audio_source.isPlaying);
    }

    AudioClip choose_random_music() //BEING CALLED EVERY FRAME OF THE 
    {
        AudioClip chosen_clip = null;
        int value = UnityEngine.Random.Range(0, 4);
        chosen_clip = audio_list[value];
        //delete value from its position, put in on the end
        audio_list.RemoveAt(value);
        audio_list.Add(chosen_clip);
        return chosen_clip;
    }

    //Start is called at the start of the game objects life
    void Start()
    {
        audio_source = GetComponent<AudioSource>();
        audio_source.clip = main_menu;
        audio_source.Play();

        //initialize RNG
        System.DateTime current_time = System.DateTime.UtcNow;
        UnityEngine.Random.InitState((int)((System.DateTimeOffset)current_time).ToUnixTimeSeconds());

        //initialize audio queue
        audio_list.Add(action_1);
        audio_list.Add(action_2);
        audio_list.Add(action_3);
        audio_list.Add(action_4);
        audio_list.Add(action_5);

        //set volume slider callback
        volume.onValueChanged.AddListener(set_volume);
    }

    // Update is called once per frame
    void Update()
    {
        if (audio_source.isPlaying == false)
        {
            Debug.Log("update running");
            switch_state();
        }
    }

    //function to set the volumen of the music with the slider
    void set_volume(float value)
    {
        audio_source.volume = value;
    }
}

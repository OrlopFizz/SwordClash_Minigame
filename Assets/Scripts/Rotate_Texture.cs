using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public struct ai_behaviors
{
    public float rotation_speed; //base speed the AI rotates
    public float max_speed; //max speed the AI rotates
    public float rotation_timer; //how long should the AI be losing for it to start chancing a direction and speed change.
    public float rotation_probability;//the probability it DOES decide to change its direction and speed;
    public int bin_size;
    public List<float> eligible_speeds;

    public ai_behaviors(float pRotation_speed, float pMax_speed, float pRotation_timer, float pRotation_probability, int pBin_size)
    {
        rotation_speed = pRotation_speed;
        max_speed = pMax_speed;
        rotation_timer = pRotation_timer;
        rotation_probability = pRotation_probability;
        bin_size = pBin_size;

        eligible_speeds = new List<float>();
        //eligible speeds are calculated by discretizing the domain (rotation_speed, max_speed). Every 5 values is an eligible speed. If the +5 speed goes past max_speed, then we add the max speed to the list
        for (float value = rotation_speed; value < max_speed; value = value + bin_size)
        {
            eligible_speeds.Add(value);
        }
        eligible_speeds.Add(max_speed);
    }
     
    public bool test_probability()
    {
        float result = UnityEngine.Random.Range(0.0f, 1.0f);
        return result <= rotation_probability;
    }

    public float get_random_speed()
    {
        int result = UnityEngine.Random.Range(0, eligible_speeds.Count);
        return eligible_speeds[result];
    }
}

public class Rotate_Texture : MonoBehaviour
{

    //TimeSpan last_time_check_rot = DateTime.Now.TimeOfDay;
    //float mult = 1; //internal speed mult to control direction

    [SerializeField]
    GameObject name_window;
    [SerializeField]
    GameObject names_object;
    [SerializeField]
    TMP_Text ai_speed_text;

    TMP_Text name_window_text;

    //GENERATING AI BEHAVIOR CONTAINERS
    Dictionary<string, ai_behaviors> difficulty_dict = new Dictionary<string, ai_behaviors>();
    public const string EASY_CONSTANT = "easy";
    public const string MEDIUM_CONSTANT = "medium";
    public const string HARD_CONSTANT = "hard";

    ai_behaviors current_behavior;

    Names names_container;

    //EXECUTION VALUES
    public string generated_name;
    int direction; //1 -> counter clock wise | 2 -> clock wise
    float current_speed;
    float time_sum;

    private void Awake()
    {
        //INITIALIZING RNG
        System.DateTime current_time = System.DateTime.UtcNow;
        UnityEngine.Random.InitState((int)((System.DateTimeOffset)current_time).ToUnixTimeSeconds());

        //initializing difficulties
        //reminder: the order of behavior parameters are: rotation_speed, Max_speed, rotation_timer, rotation_probability, and bin_size
        difficulty_dict.Add(EASY_CONSTANT, new ai_behaviors(75.0f, 75.0f * 3, 1.5f, 0.6f, 10));
        difficulty_dict.Add(MEDIUM_CONSTANT, new ai_behaviors(125.0f, 125.0f * 2, 1.0f, 0.7f, 20));
        difficulty_dict.Add(HARD_CONSTANT, new ai_behaviors(150.0f, 150.0f * 3, 1.0f, 0.8f, 75));
    }
    private void OnEnable()
    {
        name_window_text = name_window.GetComponent<TMP_Text>();
        names_container = names_object.GetComponent<Names>();

        //get chosen difficulty from difficulty manager
        GameObject difficulty_container = GameObject.Find("Difficulty_Container");
        if (generated_name == "")
        {        
            set_behavior(difficulty_container.GetComponent<Difficulty_Manager>().chosen_difficulty);
        }

        current_speed = current_behavior.rotation_speed;
        direction = 1;
        time_sum = 0;
        name_window.GetComponent<TMP_Text>().text = generated_name;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player_sword = GameObject.Find("player_sword");
        //player_sword.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 45.0f);
        if (player_sword != null)
        {
            //calculate heading angles of both objects
            double player_radians = player_sword.transform.localEulerAngles.z * (Math.PI / 180);
            Vector2 player_heading = new Vector2((float)Math.Cos(player_radians), (float)Math.Sin(player_radians));
            double ai_radians = transform.localEulerAngles.z * (Math.PI / 180);
            Vector2 ai_heading = new Vector2((float)Math.Cos(ai_radians), (float)Math.Sin(ai_radians));

            float diff_angle = Vector2.Angle(ai_heading, player_heading); //calculate the angle between vectors.
            if (65.0f <= diff_angle && diff_angle <= 115.0f)
            {
                if (time_sum >= current_behavior.rotation_timer ) //if the ai is still losing and the time to run a change has passed
                {
                    //check if probability procs
                    if (current_behavior.test_probability())
                    {
                        //change direction
                        direction = -1 * direction;
                        //chose a new speed
                        current_speed = current_behavior.get_random_speed();

                    }
                    time_sum = 0.0f; //reset to zero
                }
                time_sum += Time.deltaTime;
            }
            else
            {
                if (time_sum != 0.0f)
                {
                    time_sum = 0.0f;
                }
            }
            transform.Rotate(0, 0, direction * current_speed * Time.deltaTime);
        }
        //DEBUG show ai speed
        ai_speed_text.text = String.Format("CURRENT SPEED: {0}", current_speed);
    }

    public void set_behavior(string difficulty)
    {
        current_behavior = difficulty_dict[difficulty];

        name_window_text = name_window.GetComponent<TMP_Text>();
        string name;
        //if difficulty easy or medium, look in the commoner names, if hard, look in the noble names, add a "Ser" at the start.
        if (difficulty == HARD_CONSTANT)
        {
            name = String.Format("Ser {0} The Knight", names_container.get_noble_name());
        }
        else
        {
            name = names_container.get_commoner_names();
            if (difficulty == EASY_CONSTANT)
            {
                name = String.Format("{0} The Peasant", name);
            }
            else
            {
                name = String.Format("{0} The Footman", name);
            }
        }
        generated_name = "Fighting " + name;
    }
}

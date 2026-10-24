using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Go_Back : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(go_back);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void go_back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

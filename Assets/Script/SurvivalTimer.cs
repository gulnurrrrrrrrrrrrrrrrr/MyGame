using UnityEngine;

public class SurvivalTimer : MonoBehaviour
{
    public float goal = 60f;
    public GameObject gate;      
    float t;
    bool won;

    void Update()
    {
        t += Time.deltaTime;
        if (!won && t >= goal)
        {
            won = true;
            Debug.Log("YOU WIN!");
            if (gate != null) gate.SetActive(false);   
        }
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public int maxHp = 10;
    public Slider hpBar;
    public GameObject warningPrefab;
    public GameObject rainPrefab;
    public Fireball fireballPrefab;
    public float arenaLeft = -8f;
    public float arenaRight = 8f;
    public float rainY = 7f;
    public float baseAngle = 180f;     

    int hp;
    SpriteRenderer sr;

    bool Phase2 => hp <= maxHp / 2;

    void Start()
    {
        hp = maxHp;
        sr = GetComponent<SpriteRenderer>();
        hpBar.maxValue = maxHp;
        hpBar.value = hp;
        StartCoroutine(AttackLoop());
    }

    IEnumerator AttackLoop()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            yield return FireRain();
            yield return new WaitForSeconds(Phase2 ? 1f : 2.5f);
            FireFan();
            yield return new WaitForSeconds(Phase2 ? 1f : 2.5f);
        }
    }

    IEnumerator FireRain()
    {
        int count = Phase2 ? 4 : 2;
        float[] xs = new float[count];
        GameObject[] marks = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            xs[i] = Random.Range(arenaLeft, arenaRight);
            marks[i] = Instantiate(warningPrefab, new Vector3(xs[i], 0f, 0f), Quaternion.identity);
        }

        yield return new WaitForSeconds(1f);   

        for (int i = 0; i < count; i++)
        {
            Destroy(marks[i]);
            Instantiate(rainPrefab, new Vector3(xs[i], rainY, 0f), Quaternion.identity);
        }
    }

    void FireFan()
    {
        int count = Phase2 ? 5 : 3;
        float spread = Phase2 ? 50f : 30f;

        for (int i = 0; i < count; i++)
        {
            float t = i / (float)(count - 1);
            float angle = baseAngle + Mathf.Lerp(-spread, spread, t);
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            Fireball f = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
            f.velocity = dir * 5f;
        }
    }

    public void TakeHit()
    {
        hp--;
        hpBar.value = hp;
        StartCoroutine(Flash());
        if (hp <= 0) SceneManager.LoadScene("Win");
    }

    IEnumerator Flash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }
}
using System.Collections;
using UnityEngine;
using static DebugController;

public class Swordfish : MonoBehaviour
{
    [SerializeField] bool goesRight;
    Vector2 startPosition;
    [SerializeField] float speed;

    [SerializeField] AudioClip[] audioclips;
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        startPosition = transform.position;

        StartCoroutine(SoundCues());
    }

    private IEnumerator SoundCues()
    {
        while (true)
        {
            audioSource.clip = audioclips[Random.Range(0, audioclips.Length-1)];
            audioSource.Play();
            yield return new WaitForSeconds(5);
        }
    }

    private void Update()
    {
        if (goesRight)
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
            transform.position += new Vector3(speed*Time.deltaTime, 0, 0);

            if (transform.position.x > startPosition.x + 60) { transform.position = startPosition; }
        }
        else
        {
            transform.localScale = Vector3.one / 2;
            transform.position += new Vector3(-speed*Time.deltaTime, 0, 0);

            if (transform.position.x < startPosition.x - 60) { transform.position = startPosition; }
        }
    }
}

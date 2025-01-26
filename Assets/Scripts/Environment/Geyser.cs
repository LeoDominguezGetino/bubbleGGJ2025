using System.Collections;
using UnityEngine;

public class Geyser : MonoBehaviour
{
    public GameObject airCollectible;

    [SerializeField] int bubblesRate = 25;
    [SerializeField] float emittingTime = 2;
    [SerializeField] float pauseTime = 2;
    [SerializeField] float bubblesLifetime = 2;
    [SerializeField] float coneWith = 0.25f;

    [SerializeField] float airAmount;
    [SerializeField] float airInitialForce;
    [SerializeField] float airUpForce;

    [SerializeField] AudioClip[] bubbleSounds;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(SpawnBubbles());;
    }

    private IEnumerator SpawnBubbles()
    {
        while (true)
        {
            for (int i = 0; i < bubblesRate; i++)
            {
                yield return new WaitForSeconds(emittingTime / bubblesRate);
                AirCollectible airBubble = Instantiate(airCollectible).GetComponent<AirCollectible>();
                airBubble.transform.position = transform.position;
                airBubble.airAmount = airAmount;
                Rigidbody2D rb = airBubble.GetComponent<Rigidbody2D>();
                rb.linearVelocity = (transform.up + (transform.right * Random.Range(-coneWith, coneWith)) * airInitialForce);
                rb.gravityScale = -airUpForce;

                Destroy(airBubble.gameObject, bubblesLifetime);
            }

            audioSource.clip = bubbleSounds[Random.Range(0, bubbleSounds.Length-1)];
            audioSource.Play();

            yield return new WaitForSeconds(pauseTime);
        }
    }
}

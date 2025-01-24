using UnityEngine;

public class Geyser : MonoBehaviour
{
    public GameObject airCollectible;

    [SerializeField] int bubblesRate = 25;
    float rate;

    private void Update()
    {
        AirCollectible airBubble = Instantiate(airCollectible).GetComponent<AirCollectible>();
    }
}

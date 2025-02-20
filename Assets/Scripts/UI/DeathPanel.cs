using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DeathPanel : MonoBehaviour
{
    [SerializeField] GameObject orange;
    [SerializeField] GameObject green;
    [SerializeField] GameObject pink;

    [SerializeField] GameObject coral;
    [SerializeField] GameObject swordfish;
    [SerializeField] GameObject pufferfish;

    public void DeathScreen(int player, string hazard)
    {
        StartCoroutine(DeathAnimation(player, hazard));
    }

    IEnumerator DeathAnimation(int player, string hazard)
    {
        this.gameObject.SetActive(true);

        if (player == 0) { orange.SetActive(true); }
        else if (player == 1) { green.SetActive(true); }
        else { pink.SetActive(true); }

        if (hazard == "swordfish") { swordfish.SetActive(true); }
        else if (hazard == "pufferfish") { pufferfish.SetActive(true); }
        else { coral.SetActive(true); }

        yield return new WaitForSeconds(1f);

        orange.SetActive(false);
        green.SetActive(false);
        pink.SetActive(false);
        coral.SetActive(false);
        swordfish.SetActive(false);
        pufferfish.SetActive(false);

        this.gameObject.SetActive(false);
    }
}

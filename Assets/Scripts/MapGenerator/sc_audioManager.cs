using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_audioManager : MonoBehaviour
{
    public static AudioClip jump1, jump2, jump3, pichonAleteo, aguilaAleteo, spiderWalk, spiderAttack, hit;
    static AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        jump1 = Resources.Load<AudioClip>("SFX/Jump1");
        jump2 = Resources.Load<AudioClip>("SFX/Jump2");
        jump3 = Resources.Load<AudioClip>("SFX/Jump3");
        pichonAleteo = Resources.Load<AudioClip>("SFX/aleteoPichon");
        aguilaAleteo = Resources.Load<AudioClip>("SFX/aleteoAguila");
        spiderWalk = Resources.Load<AudioClip>("SFX/InsectWalk");
        spiderAttack = Resources.Load<AudioClip>("SFX/spiderAttack");
        hit = Resources.Load<AudioClip>("SFX/Hit");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "jump":
                int num = Random.Range(1, 3);
                switch (num)
                {
                    case 1:
                        audioSource.PlayOneShot(jump1, 0.5f);
                        break;
                    case 2:
                        audioSource.PlayOneShot(jump2, 0.5f);
                        break;
                    case 3:
                        audioSource.PlayOneShot(jump3, 0.5f);
                        break;
                }
                break;
            case "aleteoPichon":
                audioSource.PlayOneShot(pichonAleteo, 0.2f);
                break;
            case "aleteoAguila":
                audioSource.PlayOneShot(aguilaAleteo, 0.5f);
                break;
            case "spiderWalk":
                audioSource.PlayOneShot(spiderWalk, 0.5f);
                break;
            case "spiderAttack":
                audioSource.PlayOneShot(spiderAttack, 0.5f);
                break;
            case "hit":
                audioSource.PlayOneShot(hit, 0.5f);
                break;
        }
    }
}

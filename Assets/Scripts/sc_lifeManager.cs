using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_lifeManager : MonoBehaviour
{
    private const string FULL = "Anim_LifeFull";
    private const string ONEDAMAGE = "Anim_LifeOneDamage";
    private const string TWODAMAGE = "Anim_LifeTwoDamage";
    private const string THREEDAMAGE = "Anim_LifeThreeDamage";
    private const string EMPTY = "Anim_LifeEmpty";
    private Animator animator;
    private string currentAnim;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentAnim = "";
        changeAnimation(FULL);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject player = GameObject.FindWithTag("Player");
        int health = player.GetComponent<sc_player>().health;
        switch (health)
        {
            case 0:
                changeAnimation(EMPTY);
                break;
            case 1:
                changeAnimation(THREEDAMAGE);
                break;
            case 2:
                changeAnimation(TWODAMAGE);
                break;
            case 3:
                changeAnimation(ONEDAMAGE);
                break;
            case 4:
                changeAnimation(FULL);
                break;
        }
    }

    private void changeAnimation(string newAnim)
    {
        if(currentAnim == newAnim) return;
        animator.Play(newAnim);
        currentAnim = newAnim;
    }
}

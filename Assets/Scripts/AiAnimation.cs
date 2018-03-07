using UnityEngine;
using System.Collections;

public class AiAnimation : MonoBehaviour {


    /*

    This script controlls the IA character animations.

    */

    Animation anima;
    private bool isDead = false;
    public bool walking = false;
    CharacterController controller;
    private void Awake()
    {
        anima = GetComponent<Animation>();
        controller = GetComponent<CharacterController>();

    }
    void Start()
    {
        anima["jab"].layer = 1;
        anima["jab"].blendMode = AnimationBlendMode.Additive;

        anima["cross"].layer = 1;
        anima["cross"].blendMode = AnimationBlendMode.Additive;

        anima["uppercutleft"].layer = 1;
        anima["uppercutleft"].blendMode = AnimationBlendMode.Additive;

        anima["uppercutright"].layer = 1;
        anima["uppercutright"].blendMode = AnimationBlendMode.Additive;

        anima["cubrirse"].layer = 2;
        anima["cubrirse"].blendMode = AnimationBlendMode.Additive;

        anima["descubrirse"].layer = 2;
        anima["descubrirse"].blendMode = AnimationBlendMode.Additive;

        anima["impactoDerecho"].layer = 1;
        anima["impactoDerecho"].blendMode = AnimationBlendMode.Additive;

        anima["impactoIzquierdo"].layer = 1;
        anima["impactoIzquierdo"].blendMode = AnimationBlendMode.Additive;

        anima["impactoBajoDerecho"].layer = 1;
        anima["impactoBajoDerecho"].blendMode = AnimationBlendMode.Additive;

        anima["impactoBajoIzquierdo"].layer = 1;
        anima["impactoBajoIzquierdo"].blendMode = AnimationBlendMode.Additive;
    }

    void Update()
    {

        if (!isDead)
        {
            if (walking)
            {
                if (controller.velocity.z < 0)
                {
                    anima.CrossFade("avanzar", 0.1f);
                }
                else if (controller.velocity.z > 0)
                {
                    anima.CrossFade("caminar_atras", 0.1f);
                }
            }
            else
            {
                anima.CrossFade("idle", 0.1f);
            }
        }
        else
        {
            anima.CrossFade("muerte", 0.1f);
            this.enabled = false;
        }
    }

    public void Hit(string attackType)
    {
        anima.CrossFadeQueued(attackType, 0.1f, QueueMode.PlayNow);
    }

    void Impact(string hitType)
    {
        if (!isDead)
        {
            if (hitType == "jab")
            {
                anima.CrossFadeQueued("impactoIzquierdo", 0.1f, QueueMode.PlayNow);
            }
            if (hitType == "cross")
            {
                anima.CrossFadeQueued("impactoDerecho", 0.1f, QueueMode.PlayNow);
            }
            if (hitType == "uppercutleft")
            {
                anima.CrossFadeQueued("impactoBajoIzquierdo", 0.1f, QueueMode.PlayNow);
            }
            if (hitType == "uppercutright")
            {
                anima.CrossFadeQueued("impactoBajoDerecho", 0.1f, QueueMode.PlayNow);
            }
        }
    }

    void Covered()
    {
        anima.CrossFade("cubrirse", 0.1f);
    }

    void Uncovered()
    {
        anima.CrossFade("descubrirse", 0.1f);
    }

    void Dead()
    {
        isDead = true;
    }
}
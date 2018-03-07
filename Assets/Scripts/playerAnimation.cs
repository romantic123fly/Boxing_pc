

using UnityEngine;
using System.Collections;

public class playerAnimation : MonoBehaviour {


/*

This script controlls all the player animations.

*/

AnimationState cover;
AnimationState uncover;

private bool   isCovered= false;
private bool   isDead= false;
    Animation anima;
    void Start(){

        anima = GetComponent<Animation>();
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

    void Update(){

	CharacterController  controller= GetComponent<CharacterController>();

	    if(!isDead)
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                anima.Play("avanzar"); //("avanzar",0.1f);
            }
		    else if(Input.GetAxis("Vertical") < 0){
                    anima.Play("caminar_atras");
                    //anima.CrossFade("caminar_atras",0.1f);
		    }
		    else if(Input.GetAxis("Horizontal") > 0){
                    anima.Play("caminar_derecha");
                    //anima.CrossFade("caminar_derecha",0.1f);
		    }
		    else if(Input.GetAxis("Horizontal") < 0){
                    anima.Play("caminar_izquierda");
                    //anima.CrossFade("caminar_izquierda",0.1f);
		    }
		    else{
                    anima.CrossFade("idle",0.1f);
		    }
	    }
        else
        {
            anima.CrossFade("muerte", 0.2f);
            this.enabled = false;
        }
    }

    void Attack(string attackType)
    {
        anima.Stop();
        anima.Play(attackType);
    }

    void Impact( string  hitType)
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
        if (!isCovered)
        {
            anima.Blend("cubrirse", 1, 0.1f);
            isCovered = true;
        }
    }

    void Uncovered()
    {
        if (isCovered)
        {
            anima.CrossFade("descubrirse", 0.2f);
            isCovered = false;
        }
    }

    void Dead()
    {
        isDead = true;
    }
}
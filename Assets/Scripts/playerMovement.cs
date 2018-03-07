using UnityEngine;
using System.Collections;

public class playerMovement : MonoBehaviour {


    /*

    This script control the player movements.

    */

    // Player speeds
    public  float  movSpeed= 2.3f;
    public  int  rotSpeed= 5;

    // Some private variables.
    private bool isDead= false;
    private CharacterController controller;
    private Vector3  moveDirection= Vector3.zero;
    private Quaternion rotInitial;
    private GameObject enemy;

    void Start()
    {
	    // Set the enemy for the player.
	    enemy = transform.GetComponent<playerStatus>().enemy;	
    }

    void Update ()
    {
	    // If the player is not dead, can be controlled.
	    if(!isDead){
		    controller = GetComponent<CharacterController>();
		
		    if(enemy){
			    // Autorotate to the enemy.
			    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(enemy.transform.position - transform.position), rotSpeed * Time.deltaTime);			   
		    }
		
		    // Walk movement
		    moveDirection =new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		    moveDirection = transform.TransformDirection(moveDirection);
		    moveDirection *= movSpeed;
	   	    controller.Move(moveDirection * Time.deltaTime);
	    }
    }

    void Dead()
    {
        isDead = true;
    }
}
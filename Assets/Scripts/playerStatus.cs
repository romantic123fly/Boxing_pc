using UnityEngine;
using System.Collections;

public class playerStatus : MonoBehaviour {


/*

This script controlls the player status.

*/

// The enemy target for this player.
    public GameObject enemy;
    //生命值
    public  float  life= 100; // Max life.
    //精力值
    public  float  stamina= 100; // Max stamina.
    //精力值恢复速率
    public  int staminaRecuperationFactor = 5; // If you increment this, it will gain stamina faster.

    // Some private variables.
    private bool isDead= false;
    private bool canRegenerateStamina = true;

    // Variables to access to others scripts.
    private playerAnimation playerAnimationScript;
    private playerCombat playerCombatScript;
    private playerMovement playerMovementScript;
    private LevelManager levelManagerScript;

    public bool isCovered = false;

    void Start () {
	    // Set external scripts.
	    playerAnimationScript = transform.GetComponent<playerAnimation>();
	    playerCombatScript = transform.GetComponent<playerCombat>();
	    playerMovementScript = transform.GetComponent<playerMovement>();
	    levelManagerScript = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
	
	    if(!enemy){
		    print("WARNING: You must set one enemy (the PC character) for this script in the Inspector View!");
	    }
    }

    void Update () {
	    // if player is not dead, regenerate stamina.
	    if(!isDead){
		    RegenerateStamina();	
	    }
    }

    // This function apply damage for this player.
    void Damage(float amount)
        {
	
	    float totalAmount;
	
	    if(isCovered){
		    totalAmount = amount / 5;
	    }else{
		    totalAmount = amount;
		    // If the player was damaged, add one successful hit to the enemy in LevelManager.js.
		    levelManagerScript.AddSuccessfulHit("enemy");
	    }
		
	    life -= totalAmount;
	
	    if(life <= 0){
		    life = 0;
		    isDead = true;
		    gameObject.SendMessage("Dead");
	    }
    }

    // this function reduce the stamina.
    // Is called externally from PlayerCombat.js.
    public void LoseStamina( float cantidad)
    {
	    stamina -= cantidad;
	    if(stamina < 0)
        {
            stamina = 0;
            canRegenerateStamina = false;
            Invoke("SetStamina", 3);
        }
    }

    private void SetStamina()
    {
        canRegenerateStamina = true;
    }

    // this function regenerate stamina every frame.
    // Is called in Update function.
    void RegenerateStamina()
    {
	    if(canRegenerateStamina){
		    stamina += Time.deltaTime * staminaRecuperationFactor;
		    stamina = Mathf.Clamp(stamina,0,100);
	    }
    }

    void  Covered()
    {
	    isCovered = true;
    }

    void Uncovered()
    {
	    isCovered = false;
    }

        // Player Dead.
        // Informs to LevelManager.js that the player was Knockout.
    void Dead()
    {
	    isDead = true;
	    levelManagerScript.StartKO("player");
    }
}
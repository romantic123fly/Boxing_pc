using UnityEngine;
using System.Collections;

public class playerCombat : MonoBehaviour {

// Damage for each attack type 
    public  float damageJab;
    public float damageCross;
    public float damageUpperLeft;
    public float damageUpperRight;

    public float attackRatio; // If you increment this, the player can hit more faster.

    // Audio Vars
    public AudioClip attackMissed;
    public AudioClip attackLeft;
    public AudioClip attackRight;

    // Some private vars.
    private float timeForNextAttack;
    private bool   canAct= true;
    private bool   isDead= false;
    private float damageCaused;
    private GameObject enemy;
    private playerStatus playerStatusScript;
    private LevelManager levelManagerScript;
    private Animation anima;
    private new AudioSource audio;
    void  Start()
    {
	    playerStatusScript = transform.GetComponent<playerStatus>();
	    levelManagerScript = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
	    enemy = playerStatusScript.enemy;
        anima = GetComponent<Animation>();
        audio = GetComponent<AudioSource>();
    }

    void  Update ()
    {
	    if(!isDead){
		    if(timeForNextAttack > 0){
			    timeForNextAttack -= Time.deltaTime;
			    canAct = false;
		    }
		    if(timeForNextAttack < 0){
			    timeForNextAttack = 0;
			    canAct = true;
		    }
	
		    if(Input.GetKeyDown(KeyCode.R) && canAct){
			    gameObject.SendMessage("Attack","jab");
		    }
		    if(Input.GetKeyDown(KeyCode.T) && canAct){
			    gameObject.SendMessage("Attack","cross");
		    }
		    if(Input.GetKeyDown(KeyCode.F) && canAct){
			    gameObject.SendMessage("Attack","uppercutleft");
		    }
		    if(Input.GetKeyDown(KeyCode.G) && canAct){
			    gameObject.SendMessage("Attack","uppercutright");
		    }
		
		    if(Input.GetKey(KeyCode.E)){
			    gameObject.SendMessage("Covered");
		    }
		    else{
			    gameObject.SendMessage("Uncovered");
		    }
	    }
    }

    void  Attack (string attackType)
    {
	    timeForNextAttack = attackRatio;
	    playerStatusScript.LoseStamina(5);
        float dist=0;

        if (enemy){
                dist = Vector3.Distance(transform.position, enemy.transform.position);
	    }
	
	    if(attackType == "jab"){
		    if(dist < 1.95){
			    damageCaused = playerStatusScript.stamina * damageJab / 100;
			    enemy.SendMessage("LoseLife",damageCaused);
			    enemy.SendMessage("Impact",attackType);
			    playerStatusScript.LoseStamina(2);
                    audio.PlayOneShot(attackLeft);
		    }else{
                    audio.PlayOneShot(attackMissed);
		    }
	    }
	    if(attackType == "cross")
        {
		    if(dist < 1.95)
            {
			    damageCaused = playerStatusScript.stamina * damageCross / 100;
			    enemy.SendMessage("LoseLife",damageCaused);
			    enemy.SendMessage("Impact",attackType);
			    playerStatusScript.LoseStamina(2);
                audio.PlayOneShot(attackRight);
		    }
            else
            {
                audio.PlayOneShot(attackMissed);
		    }
	    }
	    if(attackType == "uppercutleft")
        {
		    if(dist < 1.85)
            {
			    damageCaused = playerStatusScript.stamina * damageUpperLeft / 100;
			    enemy.SendMessage("LoseLife",damageCaused);
			    enemy.SendMessage("Impact",attackType);
			    playerStatusScript.LoseStamina(5);
                audio.PlayOneShot(attackLeft);
		    }
            else
            {
                audio.PlayOneShot(attackMissed);
		    }
	    }
	    if(attackType == "uppercutright")
        {
		    if(dist < 1.85){
			    damageCaused = playerStatusScript.stamina * damageUpperRight / 100;
			    enemy.SendMessage("LoseLife",damageCaused);
			    enemy.SendMessage("Impact",attackType);
			    playerStatusScript.LoseStamina(5);
                audio.PlayOneShot(attackRight);
		    }
            else
            {
                audio.PlayOneShot(attackMissed);
		    }
	    }
	
	    levelManagerScript.AddHit("player");
    }

    void Dead()
    {
	    isDead = true;
    }

    // If the player was impacted, need to wait the attackRatio seconds for the next attack.
    void Impact()
    {
	    timeForNextAttack = attackRatio;
    }
}
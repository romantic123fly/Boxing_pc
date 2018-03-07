using UnityEngine;
using System.Collections;

public class AiScript : MonoBehaviour {


    /*

    This script controlls all the AI script.
    You should only change the exposed variables in the Inspector View.

    It's a combination of playerStatus, playerMovement and playerCombat scripts, with randoms
    decisions.

    */
    enum enemyState { offensive, defensive, idle }

    public GameObject player;
    public float life = 100;
    public float stamina = 100;
    public float movSpeed = 2.3f;
    public float rotSpeed = 5;
    public float damageJab;
    public float damageCross;
    public float damageUpperLeft;
    public float damageUpperRight;
    public float staminaRecuperationFactor;
    public float changeStateRatio;
    public float attackRatio;
    public float actionRatio;

    private bool canRegenerateStamina = true;
    private enemyState currentState;
    private AiAnimation aiAnimationScript;
   //攻击
    private bool   canAttack= true;
    private float timeForNextAttack;
    //改变状态
    private bool   canChangeState= true;
    //下次状态冷却
    private float timeForNextState;
    //改变动作
    private bool   canChangeAction= true;
    //下次攻击冷却
    private float timeForNextAction;    
    //攻击
    private bool isHit= false;
    //攻击类型
    private string hitType;
    //死亡
    private bool isDead= false;
    //伤害值
    private float damageCaused;
    private CharacterController controller;
    //移动方向
    private Vector3  moveDirection= Vector3.zero;
    //private Quaternion rotInitial;
    private LevelManager levelManagerScript;
    //格挡
    public bool isCovered = false;

    // 共攻击音效
    public AudioClip attackMissed;
    public AudioClip attackLeft;
    public AudioClip attackRight;
    AudioSource audioSource;
    void  Start ()
    {
        audioSource = GetComponent<AudioSource>();

    if (!player){
		print("WARNING: You must set one enemy (the player character) for this script in the Inspector View!");
	}
	levelManagerScript = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
	aiAnimationScript = transform.GetComponent<AiAnimation>();
	currentState = enemyState.idle;
    }

    void Update ()
    {
	    if(!isDead)
        {
		    RegenerateStamina();	
		    if(player)
            {
			    // Aplicar AutoRotacion
			    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), rotSpeed * Time.deltaTime);
			    //transform.rotation.x = rotInitial.x;
			    //transform.rotation.z = rotInitial.z;
		    }
		    // Verificar si el estado necesita cambiarse.
		    MakeDecisions();
		    // Ejecutar los comandos del estado seleccionado.
		    switch(currentState)
            {	
			    case enemyState.idle:
				    break;
			    case enemyState.offensive:			
				    Offensive();
				    break;
			    case enemyState.defensive:
				    Defensive();
				    break;
	        }
	}
}

    void MakeDecisions(){

	    if(timeForNextState > 0){
		    timeForNextState -= Time.deltaTime;
		    canChangeState = false;
	    }
	    if(timeForNextState < 0){
		    timeForNextState = 0;
		    canChangeState = true;
	    }
	
	    if(canChangeState){
		    int  randomState= Random.Range(1,4);
		
		    if(randomState == 1){
			    currentState = enemyState.idle;
			    aiAnimationScript.walking = false;
			    timeForNextState += changeStateRatio;
		    }
		    if(randomState == 2){
			    currentState = enemyState.defensive;
			    aiAnimationScript.walking = true;
			    timeForNextState += changeStateRatio / 2;
		    }
		    if(randomState == 3){
			    currentState = enemyState.offensive;
			    aiAnimationScript.walking = true;
			    timeForNextState += changeStateRatio;
		    }
	    }
	
	    if(timeForNextAction > 0){
		    timeForNextAction -= Time.deltaTime;
		    canChangeAction = false;
	    }
	    if(timeForNextAction < 0){
		    timeForNextAction = 0;
		    canChangeAction = true;
	    }
	
	    if(canChangeAction){
		    int  randomAccion= Random.Range(1,11);
		    if(randomAccion < 8){
			    isHit = true;
			    gameObject.SendMessage("Uncovered");
			    isCovered = false;
		    }
		    else if(randomAccion < 10){
			    gameObject.SendMessage("Covered");
			    isCovered = true;
			    timeForNextAction = 1;
		    }
		    else{
			    isHit = false;
			    gameObject.SendMessage("Uncovered");
			    isCovered = false;
			    timeForNextAction = 1;
		    }
	    }
	
	    if(timeForNextAttack > 0){
		    timeForNextAttack -= Time.deltaTime;
		    canAttack = false;
	    }
	    if(timeForNextAttack < 0){
		    timeForNextAttack = 0;
		    canAttack = true;
	    }
	
	    if(isHit){
		    if(canAttack){
			    float distOponente= Vector3.Distance(transform.position, player.transform.position);
				
			    if(distOponente < 3){
				    Attack();
				    timeForNextAction = actionRatio;
			    }				
		    }
	    }
    }

    void Covered(){
	    isCovered = true;
    }

    void Uncovered(){
	    isCovered = false;
    }

    void Offensive(){

	    controller = GetComponent<CharacterController>();
		
	    moveDirection =new Vector3(0, 0, 1);
	    moveDirection = transform.TransformDirection(moveDirection);
	    moveDirection *= movSpeed;
	    controller.Move(moveDirection * Time.deltaTime);
    }

    void Defensive(){

	    controller = GetComponent<CharacterController>();
		
	    moveDirection =new Vector3(0, 0, -1);
	    moveDirection = transform.TransformDirection(moveDirection);
	    moveDirection *= movSpeed;
	    controller.Move(moveDirection * Time.deltaTime);
    }

    void Attack () {
	
	    float randomGolpe= Random.Range(1,5);
	
	    if(randomGolpe == 1)
		    hitType = "jab";
	    if(randomGolpe == 2)
		    hitType = "cross";
	    if(randomGolpe == 3)
		    hitType = "uppercutleft";
	    if(randomGolpe == 4)
		    hitType = "uppercutright";
	
	    aiAnimationScript.Hit(hitType);
	
	    timeForNextAttack += attackRatio;
	
	    StartCoroutine("LoseStamina", 5);
	
	    float dist= Vector3.Distance(transform.position, player.transform.position);
	
	    if(hitType == "jab"){
		    if(dist < 1.95){
			    damageCaused = stamina * damageJab / 100;
			    player.SendMessage("Damage",damageCaused);
			    player.SendMessage("Impact",hitType);
                    StartCoroutine("LoseStamina", 2);

                    audioSource.PlayOneShot(attackLeft);
		    }else{
                    audioSource.PlayOneShot(attackMissed);
		    }
	    }
	    if(hitType == "cross"){
		    if(dist < 1.95){
			    damageCaused = stamina * damageCross / 100;
			    player.SendMessage("Damage",damageCaused);
			    player.SendMessage("Impact",hitType);
                    StartCoroutine("LoseStamina", 2);

                    audioSource.PlayOneShot(attackRight);
		    }else{
                    audioSource.PlayOneShot(attackMissed);
		    }
	    }
	    if(hitType == "uppercutleft"){
		    if(dist < 1.85){
			    damageCaused = stamina * damageUpperLeft / 100;
			    player.SendMessage("Damage",damageCaused);
			    player.SendMessage("Impact",hitType);
                    StartCoroutine("LoseStamina", 5);

                    audioSource.PlayOneShot(attackLeft);
		    }else{
                    audioSource.PlayOneShot(attackMissed);
		    }
	    }
	    if(hitType == "uppercutright"){
		    if(dist < 1.85){
			    damageCaused = stamina * damageUpperRight / 100;
			    player.SendMessage("Damage",damageCaused);
			    player.SendMessage("Impact",hitType);
                StartCoroutine("LoseStamina", 5);
                audioSource.PlayOneShot(attackRight);
		    }else{
                    audioSource.PlayOneShot(attackMissed);
		    }
	    }
	
	    levelManagerScript.AddHit("enemy");
			
    }

    void LoseLife(float amount)
    {
	    float totalAmount;
	    if(isCovered)
        {
		    totalAmount = amount / 5;
	    }
        else
        {
		    totalAmount = amount;
		    // If the player was damaged, add one successful hit to the enemy in LevelManager.js.
		    levelManagerScript.AddSuccessfulHit("player");
	    }
	
	    life -= totalAmount;
	
	    if(life <= 0)
        {
		    life = 0;
		    isDead = true;
		    gameObject.SendMessage("Dead");
	    }
    }

    void Dead()
    {
	    isDead = true;
        StartCoroutine("levelManagerScript.KO", "enemy");
    }

    void Impact(){
	    timeForNextAttack = 0.3f;
    }

    void RegenerateStamina()
    {
        if (canRegenerateStamina)
        {
            stamina += Time.deltaTime * staminaRecuperationFactor;
            stamina = Mathf.Clamp(stamina, 0, 100);
        }
    }

    IEnumerator LoseStamina(float amount)
    {
        stamina -= amount;
        if (stamina < 0)
        {
            stamina = 0;
            canRegenerateStamina = false;
            yield return new WaitForSeconds(3);
            canRegenerateStamina = true;
        }
    }
}
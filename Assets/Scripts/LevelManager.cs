using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {


    /*
    This script controlls all the game rounds.
    */

    // Player and Enemy GameObjects
    public  GameObject player;
    public  GameObject enemy;

    // Game Configuration
    int  roundsPerGame= 3;
    int  secondsPerRound= 60;
        AudioSource audioSource;
    // Audio Clips
    public AudioClip BellAudio;
        public AudioClip PeopleAudio;
        public AudioClip ApplauseAudio;

        // GUI Textures GameObjects
        public GameObject fight;
        public GameObject knock_out;
        public GameObject time_out;
        public GameObject round_one;
        public GameObject round_two;
        public GameObject round_three;
    /*
    If you want more rounds, just add "GameObject round_four;", etc...
    Then, go to Hierarchy, expand the LevelManager childrens, then copy and paste "round_3" game object.
    Rename it to "round_4". Then change the Texture for this game object in the Inspector View.
    */

    // You must dont touch this variables.
    private int  currentRound= 1;
    private int currentSecond;
    private bool   gameFinished= false;
    private Vector3 playerInitPosition;
    private Quaternion playerInitRotation;
    private Vector3 enemyInitPosition;
    private Quaternion enemyInitRotation;

    // This statics variables will be accessed from the Result scene.
    public  static int  playerSuccessfulHits= 0;
    public  static int  playerTotalHits= 0;
    public  static int playerEffectivity;
    public  static int  enemySuccessfulHits= 0;
    public  static int  enemyTotalHits= 0;
    public  static int enemyEffectivity;
    public static string winner;
    public static bool isKnockOut;
        Animation anima;
    void Start(){
        anima = GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();
        if (!player){
		    print("WARNING! You must set the Player GameObject for this script in the Inspector View.");
	    }
	    if(!enemy){
		    print("WARNING! You must set the Enemy GameObject for this script in the Inspector View.");
	    }
	
	    // Resets Player and Enemy position and rotation.
	    playerInitPosition = player.transform.position;
	    playerInitRotation = player.transform.rotation;
	    enemyInitPosition = enemy.transform.position;
	    enemyInitRotation = enemy.transform.rotation;
	
	    // Clear some variables to start new a game.
	    playerSuccessfulHits = 0;
	    playerTotalHits = 0;
	    enemySuccessfulHits = 0;
	    enemyTotalHits = 0;
	    winner = "";
	    isKnockOut = false;

        StartCoroutine("StartNewRound");
    }

    void  Update(){
	    // Every frame I check the time for the current round.
	    if(currentSecond == secondsPerRound){
		    StartCoroutine("TimeOutRound");
	    }
    }

    IEnumerator  StartNewRound(){
		
	    // Play the Textures animation in the beginning of each round.
	    if(currentRound == 1){
            round_one.GetComponent<Animation>().Play();
	    }
	    if(currentRound == 2){
		    round_two.GetComponent<Animation>().Play();
	    }
	    if(currentRound == 3){
		    round_three.GetComponent<Animation>().Play();
	    }
	    fight.GetComponent<Animation>().Play();
	
	    // Before start the round, disable all player and enemy scripts...
	    DisablePlayerScripts();
	    DisableEnemyScripts();

            audioSource.PlayOneShot(PeopleAudio);
	
	    // Wait for 3 seconds...
	    yield return new WaitForSeconds(3);
	
	    // When I start a New Round, I execute every second a function called Clock.
	    // This function increments the seconds for the current round.
	    InvokeRepeating("Clock",1,1);

            // Play the Bell Sound
            audioSource.PlayOneShot(BellAudio);
	
	
	    // And then enable all player and enemy scripts.
	    EnablePlayerScripts();
	    EnableEnemyScripts();

    }

    // This function is executed every second by the function StartNewRound();
    // It's a clock that increase the currentSecond by 1 for every second.
    void Clock(){
	    currentSecond++;
    }

    IEnumerator TimeOutRound(){
	
	    // Reset the currentSecond to 0.
	    currentSecond = 0;
	
	    // When a round is finished, stop the Clock.
	    CancelInvoke("Clock");
	    // Play the Texture animation that show "Time Out" message in the screen.
	    time_out.GetComponent <Animation>().Play();
	
	    // Play the Bell Sound
	    audioSource.PlayOneShot(BellAudio);
	
	    // Disable all player and enemy scripts.
	    DisablePlayerScripts();
	    DisableEnemyScripts();
	
	    // Set the default idle animation for player and enemy.
	    player.GetComponent<Animation>().CrossFade("idle",0.1f);
	    enemy.GetComponent<Animation>().CrossFade("Idle",0.1f);
	
	    // Wait 5 seconds.
	    yield return new WaitForSeconds(5);
	
	    // Increment the round by 1.
	    currentRound++;
	
	    // If all rounds has been played.
	    if(currentRound > roundsPerGame){
		    // End the Game by Time Out.
		    EndGame("timeOut");
	    }else{ // If not, reset player and enemy, and Start a New Round.
		    ResetPlayer();
		    ResetEnemy();	
		    StartCoroutine("StartNewRound");
	    }
    }

    // This function is called externally from playerStatus.js (Dead function).
    // Also is called too from AiScript.js (Dead function).
    // Its a Knockout.
    public void StartKO(string loser)
    {
        StartCoroutine("KO",loser);
    }
     IEnumerator KO(string loser)
        {
	    if(!gameFinished){
		    // Play the Texture animation thats show "KnockOut" message.
		    knock_out.GetComponent<Animation>().Play();
		    // Stop the clock.
		    CancelInvoke("Clock");
		    // Finish the game.
		    gameFinished = true;
		    // Play Applause audioclip
		    audioSource.PlayOneShot(ApplauseAudio);
		    // Wait 3 seconds.		
		    yield return new WaitForSeconds(3);
		
		    // Set the winner.
		    if(loser == "player"){
			    winner = "PC AI";
		    }
		    if(loser == "enemy"){
			    winner = "Player";
		    }
		
		    // End the Game by KnockOut.
		    EndGame("knockOut");
	    }
    }
    // Here I use OnGUI only to show the Clock in ths creen.
    void OnGUI(){
	    GUI.depth = 0;

	    if(currentSecond < 10){
		    GUI.Label( new Rect(Screen.width/2 - 3, 10, 100, 20), "0" + currentSecond.ToString());
	    }else{
		    GUI.Label( new Rect(Screen.width/2 - 3, 10, 100, 20), currentSecond.ToString());
	    }
    }

    void EndGame(string type)
        {
	
	    // Calculate the player and Enemy Effectivity.
	    if(playerTotalHits > 0){
		    playerEffectivity = playerSuccessfulHits * 100 / playerTotalHits;
	    }
	    if(enemyTotalHits > 0){
		    enemyEffectivity = enemySuccessfulHits * 100 / enemyTotalHits;
	    }
	
	    // Set the Winner by Time Out.
	    if(type == "timeOut"){
		    if(playerSuccessfulHits > enemySuccessfulHits){
			    winner = "Player";
		    }
		    if(playerSuccessfulHits < enemySuccessfulHits){
			    winner = "PC AI";
		    }
		    if(playerSuccessfulHits == enemySuccessfulHits){
			    if(playerEffectivity > enemyEffectivity){
				    winner = "Player";
			    }
			    if(playerEffectivity < enemyEffectivity){
				    winner = "PC AI";
			    }
		    }
	    }
	
	    // Set isKnockOut to true. This variable will be accessed from GUIResult.js to know if the game
	    // was finished by KnockOut.
	    if(type == "knockOut"){
		    isKnockOut = true;
	    }
	
	    // Load Results scene.
	    SceneManager.LoadScene("results");
	
    }

    // Disable player scripts to prevent movements.
    void DisablePlayerScripts(){
	    player.GetComponent<playerStatus>().enabled = false;
	    player.GetComponent<playerMovement>().enabled = false;
	    player.GetComponent<playerAnimation>().enabled = false;
	    player.GetComponent<playerCombat>().enabled = false;
    }

    // Disable player scripts to allow movements.
    void EnablePlayerScripts(){
	    player.GetComponent<playerStatus>().enabled = true;
	    player.GetComponent<playerMovement>().enabled = true;
	    player.GetComponent<playerAnimation>().enabled = true;
	    player.GetComponent<playerCombat>().enabled = true;
    }

    // Disable enemy scripts to prevent movements.
    void DisableEnemyScripts(){
	    enemy.GetComponent<AiScript>().enabled = false;
	    enemy.GetComponent<AiAnimation>().enabled = false;
    }

    // Disable enemy scripts to allow movements.
    void EnableEnemyScripts(){
	    enemy.GetComponent<AiScript>().enabled = true;
	    enemy.GetComponent<AiAnimation>().enabled = true;
    }

    void ResetPlayer(){
	    // Reset Player position and rotation.
	    player.transform.position = playerInitPosition;
	    player.transform.rotation = playerInitRotation;
	    // Set stamina to 100.
	    player.GetComponent<playerStatus>().stamina = 100;
	    // Gain some random life.
	    float  randomLifePlayer= Random.Range(10,20);
	    player.GetComponent<playerStatus>().life += randomLifePlayer;
	
	    if(player.GetComponent<playerStatus>().life > 100){
		    player.GetComponent<playerStatus>().life = 100;
	    }
    }

    void ResetEnemy(){
	    // Reset Enemy position and rotation.
	    enemy.transform.position = enemyInitPosition;
	    enemy.transform.rotation = enemyInitRotation;
	    // Set stamina to 100.
	    enemy.GetComponent<AiScript>().stamina = 100;
	    // Gain some random life.
	    float  randomLifeEnemy= Random.Range(10,20);
	    enemy.GetComponent<AiScript>().life += randomLifeEnemy;
	
	    if(enemy.GetComponent<AiScript>().life > 100){
		    enemy.GetComponent<AiScript>().life = 100;
	    }
    }

    // This function is called externally, and count the player and enemy successful hits.
    public  void  AddSuccessfulHit( string character)
        {
	    if(character == "enemy"){
		    enemySuccessfulHits++;
	    }
	    if(character == "player"){
		    playerSuccessfulHits++;
	    }
    }

    // This function is called externally, and count the player and enemy hits.
    public void AddHit( string character)
        {
	    if(character == "enemy"){
		    enemyTotalHits++;
	    }
	    if(character == "player"){
		    playerTotalHits++;
	    }
    }
}
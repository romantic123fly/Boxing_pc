using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {


/*

This script show the Life and Stamina bars for the Player and the Enemy.

*/

public  Texture2D barTexture;

private LevelManager levelManagerScript;
private playerStatus playerStatusScript;
private AiScript enemyStatusScript;

void Start () {
	levelManagerScript = transform.GetComponent<LevelManager>();
	playerStatusScript = levelManagerScript.player.GetComponent<playerStatus>();
	enemyStatusScript = levelManagerScript.enemy.GetComponent<AiScript>();
}

void OnGUI(){

	GUI.depth = 1;

	float widthLifeBar= (Screen.width / 2) - 10;
	float  widthStaminaBar= (Screen.width / 2) - 10;

	if(enemyStatusScript){
		GUI.color = Color.red;
		GUI.DrawTexture(new Rect(10, 10, widthLifeBar * enemyStatusScript.life / 100  , 15), barTexture);
		GUI.color = Color.white;
		
		GUI.color = Color.green;
		GUI.DrawTexture(new Rect(10, 30, widthStaminaBar * enemyStatusScript.stamina / 100  , 8), barTexture);
		GUI.color = Color.white;
	}
	
	if(playerStatusScript){
		GUI.color = Color.red;
		GUI.DrawTexture(new Rect(widthLifeBar + 20, 10, (widthLifeBar - 10) * playerStatusScript.life / 100  , 15), barTexture);
		GUI.color = Color.white;
		
		GUI.color = Color.green;
		GUI.DrawTexture(new Rect(widthStaminaBar + 20, 30, (widthStaminaBar - 10) * playerStatusScript.stamina / 100  , 8), barTexture);
		GUI.color = Color.white;
	}
}
}
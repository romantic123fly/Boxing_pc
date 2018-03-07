using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GUIResult : MonoBehaviour {


/*

This script only show the game results. Also have a Clock counter to show the info step-by-step.

*/

// Make the script also execute in edit mode.
//@script ExecuteInEditMode()

private int  currentSecond= 0;

void Start(){
	InvokeRepeating("Clock",1,1);
}

void OnGUI(){
	GUI.Box( new Rect(10,10,(Screen.width / 2),Screen.height - 20),"RESULTS");

	if(currentSecond > 0.5f){
		GUI.Label( new Rect(20,50,(Screen.width / 2),Screen.height - 20),"Player Total Hits: " + LevelManager.playerTotalHits);
	}
		
	if(currentSecond > 1){
		GUI.Label( new Rect(20,70,(Screen.width / 2),Screen.height - 20),"Player Successful Hits: " + LevelManager.playerSuccessfulHits);
	}
	
	if(currentSecond > 1.5f){
		GUI.Label( new Rect(20,90,(Screen.width / 2),Screen.height - 20),"Player Effectivity: " + LevelManager.playerEffectivity + "%");
	}
	
	if(currentSecond > 2f){
		GUI.Label( new Rect(20,140,(Screen.width / 2),Screen.height - 20),"PC AI Total Hits: " + LevelManager.enemyTotalHits);
	}
		
	if(currentSecond > 2.5f){
		GUI.Label( new Rect(20,160,(Screen.width / 2),Screen.height - 20),"PC AI Successful Hits: " + LevelManager.enemySuccessfulHits);
	}
	
	if(currentSecond > 3f){
		GUI.Label( new Rect(20,180,(Screen.width / 2),Screen.height - 20),"PC AI Effectivity: " + LevelManager.enemyEffectivity + "%");
	}
	
	if(currentSecond > 3.5f){
		if(LevelManager.isKnockOut)
            {
			GUI.Label( new Rect(20,220,(Screen.width / 2),Screen.height - 20),"WINNER: " + LevelManager.winner + " by Knockout!");
		}else{
			GUI.Label( new Rect(20,220,(Screen.width / 2),Screen.height - 20),"WINNER: " + LevelManager.winner + " by Points!");
		}
	}
	
	GUI.enabled = false;
	
	if(currentSecond > 4){
		GUI.enabled = true;
            if (GUI.Button(new Rect(20, Screen.height - 80, (Screen.width / 2) - 30, 60), "Back to Main Menu"))
            {
                SceneManager.LoadScene("intro");
            }
        }
    }

    void Clock()
    {
        currentSecond++;
    }
}
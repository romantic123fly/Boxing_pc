using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GUIMainMenu : MonoBehaviour {


    /*

    This script show the Start New Game button and the Controls Help in the Main Menu screen.

    */

    // Make the script also execute in edit mode.
    //@script ExecuteInEditMode()

public Texture2D controlsTexture;

void OnGUI(){
	
	GUI.depth = 0;
	
	if(GUI.Button( new Rect(30,210,(Screen.width / 2) - 30,100),"Start New Game")){
		SceneManager.LoadScene("scene");
	}
	
	GUI.Box( new Rect(0,Screen.height-110,Screen.width,110),"");
	GUI.DrawTexture( new Rect(20,Screen.height-controlsTexture.height,controlsTexture.width,controlsTexture.height),controlsTexture);
}
}
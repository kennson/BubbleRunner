using UnityEngine;
using System.Collections;

public class startGame : MonoBehaviour {

	public void StartGame()
	{
		Application.LoadLevel("03_Begin");
	}
	
	public void QuitGame()
	{
		Application .Quit();
	}
}

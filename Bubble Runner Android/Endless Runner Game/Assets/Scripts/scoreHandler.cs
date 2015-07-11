using UnityEngine;
using System.Collections;

public class scoreHandler : MonoBehaviour {

	private int _score = 0;
	
	void OnGUI(){
		GUI.color = Color.black;
		GUIStyle _style = GUI.skin.GetStyle ("Label");
		_style.alignment = TextAnchor.UpperLeft;
		_style.fontSize = 20;
		GUI.Label (new Rect (220, 20, 200, 200), "Score: " + _score.ToString (), _style);
	}

	public int Points{
		get{return _score;}
		set{_score = value;}
	}
}

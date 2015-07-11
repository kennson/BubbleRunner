using UnityEngine;
using System.Collections;

public class playerHandler : MonoBehaviour {

	private bool inAir = false;

	public bool jumpPress = false;

	void Update () {
		if (!inAir && Mathf.Abs(this.rigidbody2D.velocity.y) > 0.05f) {
			inAir = true;
		} else if (inAir && this.rigidbody2D.velocity.y == 0.00f) {
			inAir = false;

			if(jumpPress) Jump();
		}
	}

	public void Jump(){
		jumpPress = true;
		if (inAir)
			return;
		this.rigidbody2D.AddForce (Vector2.up * 3000);
		GameObject.Find("Main Camera").GetComponent<playSound>().PlaySound("jump");
	}
}

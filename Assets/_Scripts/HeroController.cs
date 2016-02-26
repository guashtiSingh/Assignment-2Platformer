using UnityEngine;
using System.Collections;

public class HeroController : MonoBehaviour {
	// private instance variables
	private Animator _animator;
	private float _move;
	private float _jump;


	// Use this for initialization
	void Start () {
		this._animator = gameObject.GetComponent<Animator> ();
		this._move = 0f;
		this._jump = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		this._move = Input.GetAxis ("Horizontal");
		this._jump = Input.GetAxis ("Vertical");

		if (this._move != 0) {
			//Call the run button
			this._animator.SetInteger ("AnimState", 1);
		} else {
			//Set default animation state
			this._animator.SetInteger ("AnimState", 0);
		}

		if (this._jump > 0) {
			//Call the jump button
			this._animator.SetInteger ("AnimState", 2);
		}
	}
}






























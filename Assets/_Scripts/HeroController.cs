using UnityEngine;
using System.Collections;


//Velocity Range Utility Class
[System.Serializable]
public class VelocityRange {
	//Public Instance Variables
	public float minimum;
	public float maximum;

	//Constructor
	public VelocityRange (float minimum, float maximum) {
		this.minimum = minimum;
		this.maximum = maximum;
	}
}

public class HeroController : MonoBehaviour {

	//Public Instance Variables
	public VelocityRange velocityRange;
	public float moveForce;
	public float jumpForce;
	public Transform groundCheck;
	public Transform camera;
	public GameController gameController;

	// private instance variables
	private Animator _animator;
	private float _move;
	private float _jump;
	private bool _facingRight;
	private Transform _transform;
	private Rigidbody2D _rigidBody2D;
	private bool _isGrounded;
	private AudioSource[] _audioSources;
	private AudioSource _jumpSound;
	private AudioSource _coinSound;
	private AudioSource _hurtSound;

	// Use this for initialization
	void Start () {
		//Initialize public instance variables
		this.velocityRange = new VelocityRange(300f, 30000f);

		//Initialize private instance vairables
		this._rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
		this._transform = gameObject.GetComponent<Transform> ();
		this._animator = gameObject.GetComponent<Animator> ();
		this._move = 0f;
		this._jump = 0f;
		this._facingRight = true;


		//Setup AudioSources
		this._audioSources = gameObject.GetComponents<AudioSource> ();
		this._jumpSound = this._audioSources [0];
		this._coinSound = this._audioSources [1];
		this._hurtSound = this._audioSources [2];

		//Place the hero in the correct starting position
		this._spawn ();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 currentPosition = new Vector3 (this._transform.position.x, this._transform.position.y, -10f);
		this.camera.position = currentPosition;

			this._isGrounded = Physics2D.Linecast(this._transform.position, 
				this.groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
			Debug.DrawLine (this._transform.position, this.groundCheck.position);

			float forceX = 0f;
			float forceY = 0f;

			// get absolute value of velocity for out gameObject
			float absVelX = Mathf.Abs(this._rigidBody2D.velocity.x);
			float absVelY = Mathf.Abs(this._rigidBody2D.velocity.y);

		if (this._isGrounded) {
			//get a number between -1 and 1 for both horizontal and vertical axes
			this._move = Input.GetAxis ("Horizontal");
			this._jump = Input.GetAxis ("Vertical");

			if (this._move != 0) {
				if (this._move > 0) {
					//movement force
					if (absVelX < this.velocityRange.maximum) {
						forceX = this.moveForce;
					}
					this._facingRight = true;
					this._flip ();
				} 

				if (this._move < 0) {
					//movement force
					if (absVelX < this.velocityRange.maximum) {
						forceX = -this.moveForce;
					}
					this._facingRight = false;
					this._flip ();
				}
				//Call the run button
				this._animator.SetInteger ("AnimState", 1);
			} else {
				//Set default animation state
				this._animator.SetInteger ("AnimState", 0);
			}

			if (this._jump > 0) {
				//jump force
				if (absVelY < this.velocityRange.maximum) {
					this._jumpSound.Play ();
					forceY = this.jumpForce;
				}
			}
		} else {
			//Set jump animation state
			this._animator.SetInteger ("AnimState", 2);
		}
			
			//Apply the forces to the player
		this._rigidBody2D.AddForce(new Vector2(forceX, forceY));
		}


	void OnCollisionEnter2D(Collision2D other) {
		if(other.gameObject.CompareTag("Coin")) {
			this._coinSound.Play ();
			Destroy (other.gameObject);
			this.gameController.ScoreValue += 10;
		}

		if(other.gameObject.CompareTag("Enemy")) {
			this._spawn ();
			this._hurtSound.Play ();
			this.gameController.LivesValue--;
		}

		if(other.gameObject.CompareTag("Death")) {
			this._spawn ();
			this._hurtSound.Play ();
			this.gameController.LivesValue--;
		}


		if(other.gameObject.CompareTag("Goal")) {
			this.gameController.LivesValue = 0;
			Destroy (gameObject);
		}
	}

		//Private Methods
		private void _flip () {
			if (this._facingRight) {
				this._transform.localScale = new Vector2 (1, 1);
			} else {
				this._transform.localScale = new Vector2 (-1, 1);
			}
		}

	private void _spawn() {
		this._transform.position = new Vector3 (-125f, 125f, 0);
	}
}






























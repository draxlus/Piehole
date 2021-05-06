using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player_control : MonoBehaviour {

	public float jump_force = 500f;
	public float horizontal_speed = 50f;
	public float h_speed_multiplier = 0.75f;
	public float grav_scale_multiplier = 1.25f;
	public int fat_state = 0;
	int fat_state_max = 4;
	bool jump = false;
	
	Vector3 scaleoriginal;

	private bool canBoost = true;
	private float boostCooldown = 2f;
	
	float spriteIncrease = 0.3f;
	
	bool[] increase;
	

	Rigidbody2D physics;
	Animator anim;

	// Call once when the game is initially launched
	void Start ()
	{
		physics = this.GetComponent<Rigidbody2D>();
		anim = this.GetComponent<Animator>();
		increase = new bool[fat_state_max];
		for(int i = 0; i < fat_state_max; i++){
			increase[i] = true;
			
		}
		increase[0] = false;
	}


	// Update is called once per frame
	void Update () {
		
		
		
		
		
		
		
		RaycastHit2D[] hits = new RaycastHit2D[2];
		int h = Physics2D.RaycastNonAlloc(transform.position, -Vector2.up, hits); //cast downwards
 
		float angle = Mathf.Abs(Mathf.Atan2(hits[1].normal.x, hits[1].normal.y)*Mathf.Rad2Deg); //get angle
		Debug.Log(angle);
		
		
		
		if(fat_state >= 4){
			//reload level
			//SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			StartCoroutine(resetlevel());
			//physics.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

			
		}
		else{
			//flip sprites to face direction moving
			if((Mathf.Abs(angle) < 10) || (Input.GetAxis("Horizontal") != 0)){
				flip();
			}
		}
		
 
		
		
		
		
		// Get the horizontal movement from the keyboad (A,D,left arrow,right arrow)
		float horiz = Input.GetAxis("Horizontal") * horizontal_speed;
		// Should I be playing walking animation?
		// Animator anim = this.GetComponent<Animator>();
		// anim.SetFloat("Horizontal", Mathf.Abs(horiz));

		if(increase[fat_state]){
			scaleoriginal = this.transform.localScale;
			scaleoriginal.z = 0;
			scaleoriginal.x *= spriteIncrease;
			scaleoriginal.y *= spriteIncrease;
			transform.localScale += scaleoriginal;
			increase[fat_state] = false;
		}
		
		

		
		
		
		
		

		
		// Did the player press the space bar?
		if (Input.GetButtonDown("Jump") && jump)
		{
			Debug.Log ("jumped");
			// Give us an upwards velocity
			physics.AddForce(new Vector2(0, 1.25f * jump_force));
			jump = false;
			anim.SetBool("jump",true);
		}
		// Did the player press to dash
	   if (Input.GetButtonDown("Fire1"))
		{
			Debug.Log ("dashed");
			Vector2 boostSpeed = new Vector2(20,0);
			if(Input.GetAxis("Horizontal") < 0){
				boostSpeed = -boostSpeed;
			}
			
			if(canBoost){
				anim.SetBool("jump",false);
				anim.SetBool("dash", true);
				StartCoroutine(Boost(0.5f, boostSpeed));
			}
		}
		
		
		
		//if (Pie.IsTrigger () == true) {
			//Destroy (Pie);

			
		
			
		// Use our horizontal movement to move left and right
		physics.velocity = new Vector2(horiz, physics.velocity.y);



	}
	
	
	IEnumerator Boost(float boostDur, Vector2 boostSpeed) //Coroutine with a single input of a float called boostDur, which we can feed a number when calling
    {
        float time = 0f; //create float to store the time this coroutine is operating
        canBoost = false; //set canBoost to false so that we can't keep boosting while boosting
		
        while(boostDur > time) //we call this loop every frame while our custom boostDuration is a higher value than the "time" variable in this coroutine
        {
            time += Time.deltaTime; //Increase our "time" variable by the amount of time that it has been since the last update
            physics.velocity = boostSpeed; //set our rigidbody velocity to a custom velocity every frame, so that we get a steady boost direction like in Megaman
            yield return new WaitForSeconds(0); //go to next frame
        }
         yield return new WaitForSeconds(boostCooldown); //Cooldown time for being able to boost again, if you'd like.
		 anim.SetBool("dash", false);
    }
	
	IEnumerator resetlevel(){
		yield return new WaitForSeconds(1);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	
	void Dead(){
		
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		
		
	}
	
	IEnumerator levelselect(){
		
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene("levelselect");
		
		
	}
	
	
	
	
	void flip(){
		//flip sprites if faced other direction
		if(Input.GetAxis("Horizontal") < 0){
			if(transform.localScale.x >= 0){
				Vector3 scale = transform.localScale;
				scale.x *= -1;
				transform.localScale = scale;
			}
		}
		else if (Input.GetAxis("Horizontal") > 0){
			if (transform.localScale.x < 0){
				Vector3 scale = transform.localScale;
				scale.x *= -1;
				transform.localScale = scale;
			}
		}
	}
	
	
	float getAngle(){
		RaycastHit2D[] hits = new RaycastHit2D[2];
		int h = Physics2D.RaycastNonAlloc(transform.position, -Vector2.up, hits); //cast downwards
 
		float angle = Mathf.Abs(Mathf.Atan2(hits[1].normal.x, hits[1].normal.y)*Mathf.Rad2Deg); //get angle
		Debug.Log(angle);
		return angle;
	}


	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Pie") {
			Destroy (col.gameObject);
			physics.gravityScale *= grav_scale_multiplier;
			horizontal_speed *= h_speed_multiplier;
			//Debug.Log("collided!");
			if(fat_state < 4){
				fat_state += 1;
			}
			anim.SetInteger("fat_state", fat_state);
		}

		if (col.gameObject.tag == "Platform" || col.gameObject.tag == "MovingPlatform") {
			if (jump == false) {
				jump = true;
				anim.SetBool("jump", false);
				canBoost = true;
			}
		}
		
		if(col.gameObject.tag == "Boundary") {
			Dead();
			
		}
		
		//hit tagged finish object to go to level select
		if(col.gameObject.tag == "Finish") {
			StartCoroutine(levelselect());
		}
		
		if ((col.transform.tag == "MovingPlatform") && Mathf.Abs(getAngle()) < 0.0000001)
        {
            transform.parent = col.transform;
        }
		
	}
	
	void OnCollisionStay2D(Collision2D col) {
		if (col.gameObject.tag == "Platform" || col.gameObject.tag == "MovingPlatform") {
			if(physics.velocity.x != 0){
				anim.SetBool("walking", true);
			}
			else if(physics.velocity.x <= 0.00f){
				anim.SetBool("walking", false);
			}
		}
	}
		
	void OnCollisionExit2D(Collision2D col) {
		if (col.gameObject.tag == "Platform" || col.gameObject.tag == "MovingPlatform") {
			anim.SetBool("walking", false);
		}	
		
		if (col.transform.tag == "MovingPlatform")
        {
            transform.parent = null;
        }
		
	}


}

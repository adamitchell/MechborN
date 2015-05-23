﻿using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	public Transform theCamera;
	public float movementForce = 7.0f;

	//camera zooming via mousewheel
	public float cameraDistance;
	private float minFov = 10f;
	private float maxFov = 80f;
	private float sensitivity = 10f;

	private Vector3 mouse_pos;
	private Vector3 object_pos;
	private float angle;

	private bool rightScreenTrigger;
	private float rightScreenTimer;
	private bool leftScreenTrigger;
	private float leftScreenTimer;

	public float rotSpeed;
	private Quaternion rightRot;
	private Quaternion leftRot;

	private Vector2 mouse;
	private Vector2 playerScreenPoint;

	private bool notSet;
	//rotation presets:
	//	-left 		= 0,-90,0
	//	-right		= 0,90,0
	//	-up 		= -,-,-
	//	-down       = 
	//	-up-left    = 
	//	-up-right   = 
	//	-down-left  = 
	//	-down-right = 

	private Vector3 offset;
	private bool loop;

	//leg preset for moving right/left = 3.4,270,360
	//leg preset for moving up = 
	//leg preset for idle = 3.4,370,360
	private GameObject leg;

	public Transform target;
	private Animator animator;
	private Animation shield_up;

	private GameObject forceField;

	private GameObject theTarget;
	private Vector2 currLoc;
	private Vector3 newLoc;

	// Use this for initialization
	void Start () 
	{
		theTarget = GameObject.FindGameObjectWithTag ("Target").gameObject;
		//shield_up = Animation ["shield_up"];

		forceField = GameObject.Find ("ForceField").gameObject;
		forceField.SetActive (false);

		animator = GetComponent<Animator> ();

		notSet = true;
		loop = false;

		rotSpeed = 0.1f;
		rightRot = Quaternion.Slerp (transform.rotation,Quaternion.Euler (0,0,90),Time.deltaTime);
		//leftRot = Quaternion.Slerp (transform.rotation,Quaternion.Euler (0,0,-90),Time.deltaTime);

		rightScreenTrigger = false;
		rightScreenTimer = Time.time;
		leftScreenTrigger = false;
		leftScreenTimer = Time.time;

		cameraDistance = 50f;
		offset = new Vector3 (0.0f, 0.0f, -cameraDistance);

		//leg = GameObject.Find ("Hueso108").gameObject;

		//transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		theCamera.LookAt (transform.position);
		//Camera movement
		theCamera.position = Vector3.Lerp (theCamera.position, transform.position + offset, 0.8f * Time.deltaTime);

		//UP
		if(Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
		{
			transform.GetComponent<Rigidbody>().AddForce(0.0f,movementForce,0.0f,ForceMode.Acceleration);

			//UP-RIGHT
			if(Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
			{
				//if mouse is on left side of screen
				if(mouse.x < playerScreenPoint.x)
				{

				}else{
					//else if mouse is on right side of screen
				}

				transform.GetComponent<Rigidbody>().AddForce(movementForce,0.0f,0.0f,ForceMode.Acceleration);

			}

			//UP-LEFT
			else if(Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
			{
				//if mouse is on left side of screen
				if(mouse.x < playerScreenPoint.x)
				{
					
				}else{
					//else if mouse is on right side of screen
				}

				transform.GetComponent<Rigidbody>().AddForce(-movementForce,0.0f,0.0f,ForceMode.Acceleration);
			}
		}

		//RIGHT
		if(Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
		{
			//if mouse is on left side of screen
			if(mouse.x < playerScreenPoint.x)
			{
				
			}else{
				//else if mouse is on right side of screen
			}

			transform.GetComponent<Rigidbody>().AddForce(movementForce,0.0f,0.0f,ForceMode.Acceleration);
		}

		//LEFT
		if(Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
		{
			//if mouse is on left side of screen
			if(mouse.x < playerScreenPoint.x)
			{
				
			}else{
				//else if mouse is on right side of screen
			}

			transform.GetComponent<Rigidbody>().AddForce(-movementForce,0.0f,0.0f,ForceMode.Acceleration);
		}

		//DOWN
		else if(Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
		{
			transform.GetComponent<Rigidbody>().AddForce(0.0f,-movementForce,0.0f,ForceMode.Acceleration);
			
			//DOWN-RIGHT
			if(Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
			{
				//if mouse is on left side of screen
				if(mouse.x < playerScreenPoint.x)
				{
					
				}else{
					//else if mouse is on right side of screen
				}

				transform.GetComponent<Rigidbody>().AddForce(movementForce,0.0f,0.0f,ForceMode.Acceleration);
			}
			
			//DOWN-LEFT
			else if(Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
			{
				//if mouse is on left side of screen
				if(mouse.x < playerScreenPoint.x)
				{
					
				}else{
					//else if mouse is on right side of screen
				}

				transform.GetComponent<Rigidbody>().AddForce(-movementForce,0.0f,0.0f,ForceMode.Acceleration);
			}
		}

		if(Input.GetKey(KeyCode.Mouse1))
		{
			forceField.SetActive(true);
		}else{
			forceField.SetActive(false);
		}

		transform.position = new Vector3 (transform.position.x, transform.position.y, -1f);
	}

	void Update ()
	{ 
		//if they hold down right click for the shield
		if(Input.GetKeyDown (KeyCode.Mouse1))
		{
			StartCoroutine(startAnimation ());
		}
		if(Input.GetKeyUp (KeyCode.Mouse1))
		{
			StartCoroutine(endAnimation());
		}

		newLoc = Input.mousePosition;
		newLoc.z = 48f;

		theTarget.transform.position = Camera.main.ScreenToWorldPoint(newLoc);

		//theCamera.LookAt (transform.position);

		Cursor.visible = false;

		mouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
		playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

		if(mouse.x < playerScreenPoint.x)
		{
			transform.GetComponent<SimpleSmoothMouseLook>().targetDirection.y = -90f*Time.deltaTime;

		}else //if(mouse.x < playerScreenPoint.x && !rightScreenTrigger && !leftScreenTrigger)
		{
			transform.GetComponent<SimpleSmoothMouseLook>().targetDirection.y = 90f*Time.deltaTime;
		}

		//Camera zooming using mousewheel
		float fov = Camera.main.fieldOfView;
		fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
		fov = Mathf.Clamp(fov, minFov, maxFov);
		Camera.main.fieldOfView = fov;
	}


	IEnumerator startAnimation()
	{
		animator.SetBool ("Shield Up",true);
		yield return new WaitForSeconds (.5f);

	}

	IEnumerator endAnimation()
	{
		animator.SetBool("Shield Up",false);
		yield return new WaitForSeconds (.5f);

	}
	
}

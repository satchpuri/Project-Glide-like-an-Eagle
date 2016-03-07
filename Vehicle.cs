using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]

abstract public class Vehicle : MonoBehaviour {

    //-----------------------------------------------------------------------
    // Class Fields
    //-----------------------------------------------------------------------

    //movement
    protected Vector3 acceleration;
    protected Vector3 velocity;
    protected Vector3 desired;

	//to access the game manager
	protected GameManager gm;

	//getter for velocity
    public Vector3 Velocity {
        get { return velocity; }
    }

    //public for changing in Inspector
    //define movement behaviors
    public float maxSpeed = 6.0f;
    public float maxForce = 120.0f;
    public float mass = 1.0f;
    public float radius = 1.0f;


    //access to Character Controller component
   	CharacterController charControl;
    
	//abstract class
    abstract protected void CalcSteeringForces();


    //-----------------------------------------------------------------------
    // Start and Update
    //-----------------------------------------------------------------------
	virtual public void Start(){
        
        acceleration = Vector3.zero;
        velocity = transform.forward;

        charControl = GetComponent<CharacterController>();
		gm = GameObject.Find("GameManagerGO").GetComponent<GameManager>(); 
	}

	
	// Update is called once per frame
	protected void Update () {

		CalcSteeringForces();
		//add velocity to acceleration		
		velocity += acceleration * Time.deltaTime;
		//limit the max vel
		velocity = Vector3.ClampMagnitude (velocity, maxSpeed);

		transform.forward = velocity.normalized;

		charControl.Move (velocity * Time.deltaTime);

		acceleration = Vector3.zero;

	}

    //-----------------------------------------------------------------------
    // Class Methods
    //-----------------------------------------------------------------------
	
	//apply force
	protected void ApplyForce(Vector3 steeringForce) {

		acceleration += steeringForce / mass;
	}
	
	//seek
	protected Vector3 Seek(Vector3 targetPos) {
		
		desired = targetPos - transform.position;
		desired = desired.normalized * maxSpeed;
		desired -= velocity;
		
		return desired;
	}

    //leader following
	protected Vector3 LeaderFollow(GameObject leader){

		Vector3 lead = new Vector3();
        //loop through all the follower butterfies
		for (int i= 0; i< gm.butterflyFollowers.Length; i++) {
            //follow the leader 
			lead += Seek (leader.transform.position - (leader.transform.forward * - 40));
			
            //if too far 
			if (Vector3.Distance(transform.position, leader.transform.position) < 50)
			{
                
				lead -= (Seek(leader.transform.position) * 3);
			}

		}
		return lead;
	}

    //seperation
	 public Vector3 Seperation(float dist)
	{

		Vector3 sum_vector = new Vector3();
		int count = 0;
		
		// For each obj, check the distance from this obj, and if withing a neighbourhood, add to the sum_vector
		for (int i= 0; i< gm.butterflyFollowers.Length; i++)
		{
			float d = Vector3.Distance(transform.position, gm.butterflyFollowers[i].transform.position);
			
			if (d  < dist && d > 0) // d > 0 prevents including obj
			{
				sum_vector += (transform.position - gm.butterflyFollowers[i].transform.position).normalized / d;
				count++;
			}
		}		
		// Average the sum_vector
		if (count > 0)
		{
			sum_vector /= count;
		}
		return  sum_vector;
	} 

    //alignment
	public Vector3 Alignment(float alignment)
	{
		Vector3 sum_vector = new Vector3();
		int count = 0;
		
		// For each obj, check the distance from this obj, and if withing a neighbourhood, add to the sum_vector
		for (int i=0; i< gm.butterflyFollowers.Length; i++)
		{
			float dist = Vector2.Distance(transform.position, gm.butterflyFollowers[i].transform.position);
			
			if (dist < alignment && dist > 0) // dist > 0 prevents including this boid
			{
				sum_vector += velocity -gm.butterflyFollowers[i].transform.position;
				count++;
			}
		}
		
		// Average the sum_vector and clamp magnitude
		if (count > 0)
		{
			sum_vector /= count;
			sum_vector = Vector2.ClampMagnitude(sum_vector, 1);
		}
		
		return sum_vector;
	}

    //cohesion
	public Vector3 Cohesion(float centroid)
	{

		Vector3 sum_vector = new Vector3();
		int count = 0;
		
		// For each obj, check the distance from this obj, and if withing a neighbourhood, add to the sum_vector
		for (int i=0; i< gm.butterflyFollowers.Length; i++)
		{
			float dist = Vector3.Distance(transform.position, gm.butterflyFollowers[i].transform.position);
			
			if (dist < centroid && dist > 0) // dist > 0 prevents including this boid
			{
				sum_vector += gm.butterflyFollowers[i].transform.position;
				count++;
			}
		}
		
		// Average the sum_vector and return value
		if (count > 0)
		{
			sum_vector /= count;
			return  sum_vector - transform.position;
		}
		
		return sum_vector; 

	}
}

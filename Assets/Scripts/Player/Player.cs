﻿using UnityEngine;
using Assets.Scripts.Util;
using Assets.Scripts.Manager;

namespace Assets.Scripts.Player
{

    public class Player : MonoBehaviour, Util.PlaysOnBeat
    {
        private static int shooting;

        public float maxHealth;

        protected float health;
        public int playerNum; //set this yo
        private Vector2 playerInput = new Vector2(0, 0);
        private Vector2 crosshairInput = new Vector2(0, 0);
        private Vector2 triggerInput = new Vector2(0, 0);

        private Vector2 movement = new Vector2(0, 0);

        public float crosshairSpeed = 12f;
        public float crosshairSlowSpeed = 4f;

        public float acceleration = 0.8f;
        public float decceleration = 12f;
        public float maxSpeed = 6f;

        public float jumpSpeed = 15f;
        private float jumpStartTime = 0f;

        public float rotationSpeed = 180;
        public float maxRotation = 15;

        public bool useAcceleration = true;//should the player slide when moving
        public bool shouldBob = false;//should the player bob in the water

        private bool isJumping = false; //is player jumping
        private bool isPlayerMoving = false; //is player asking the character to move
        private bool isShooting = false;//is player pew pewing

        private bool hit;
        private float damage;
        private int state;

        public GameObject myCrosshair;
        public Bullets.BulletPool bullets;
        public Animator anim;


        public float invulerabilityTime = 1f;
        private float invulerability;
        private bool render;


        // Use this for initialization
        void Start()
        {
            shooting = 0;
            transform.position = new Vector3(PlayerUtil.defaultPlayerSpawn.x + 4 * playerNum, 
            								 PlayerUtil.defaultPlayerSpawn.y,
            								 PlayerUtil.defaultPlayerSpawn.z);
            
            Vector3 crosshairSpawn = new Vector3(transform.position.x, 
	            								 transform.position.y + PlayerUtil.defaultCrosshairSpawn.y,
	            								 PlayerUtil.defaultCrosshairSpawn.z);
            myCrosshair = GameObject.Instantiate(myCrosshair, crosshairSpawn, Quaternion.identity) as GameObject;

            if (TempoManager.instance == null)
                FindObjectOfType<TempoManager>().Init();
            TempoManager.instance.objects.Add(this);
            health = maxHealth;
            state = Animator.StringToHash("State");
        }

        protected virtual void Render(bool render)
        {
            GetComponent<SpriteRenderer>().enabled = render;
        }

        // Update is called once per frame
        void Update()
        {
            //Always check Life/Death first
            this.handleHealth();

            this.playerInput = PlayerUtil.getLeftJoystick(playerNum);
            this.crosshairInput = PlayerUtil.getRightJoystick(playerNum);
            this.triggerInput = PlayerUtil.getControllerTriggers(playerNum);

            //Crosshair movement
            if (this.crosshairInput.x != 0 || this.crosshairInput.y != 0)
            {
                this.moveCrosshair(this.crosshairInput);
            }
            //Shooting
            if (this.triggerInput.y > 0)
            {
                this.isShooting = true;
                this.actionShoot();
            }
            else
            {
                this.isShooting = false;
            }

            //Horizontal Movement
            if (this.playerInput.x < 0)
            {
                this.isPlayerMoving = true;
                this.handlePlayerMove(this.playerInput.x);
                this.lean(this.playerInput.x);
            }
            else if (playerInput.x > 0)
            {
                this.isPlayerMoving = true;
                this.handlePlayerMove(this.playerInput.x);
                this.lean(this.playerInput.x);
            }
            else
            {
                if (this.useAcceleration)
                {
                    if (Mathf.Abs(this.movement.x) < 0.5f)
                    {
                        this.movement.x = 0f;
                    }

                    if (Mathf.Abs(this.movement.x) > 0)
                    {
                        var deceVal = (this.movement.x < 0 ? this.decceleration : -this.decceleration) * Time.deltaTime;
                        this.movement.x += deceVal;
                    }
                }
                else
                {
                    this.movement.x = 0;
                }
                this.isPlayerMoving = false;
            }
            //Vertical Movement
            if (this.getJumpPressed() && !this.getAirborne() && this.getIsOnSurface())
            {
                this.actionJump();
            }
            if (this.isJumping)
            {
                this.handleJumping();
            }

            //Passive Movement
            if (this.getShouldBob())
            {
                // TODO properly loop animation
                float percentInRotation = PlayerUtil.getPercentInRotation(transform.localEulerAngles.z, this.maxRotation);

                float leanCurve = Mathf.PingPong(Time.time, 2f) - 1f;
                float leanOffset = percentInRotation;
                float leanValue = leanCurve - leanOffset;

                this.lean(leanValue);
            }
            else
            {
                //rotate back towards center
                float percentInRotation = PlayerUtil.getPercentInRotation(transform.localEulerAngles.z, this.maxRotation);
                this.lean(-percentInRotation);
            }

            //constantly move the character
            this.movePlayer();
            this.handlePlayerAnimation();
        }

        // Handlers
        private void handlePlayerDidCollide(Collider2D collider) {
    		Vector3 myPos = transform.position;
        	Vector3 theirPos = collider.transform.position;
        	// Vector2 halfSize = new Vector2(PlayerUtil.playerSize.x/2, PlayerUtil.playerSize.y/2);
        	
        	Vector3 direction = PlayerUtil.getCollisionDirection(myPos, theirPos);
        	print(direction);
        }

        private void handleHealth() {
			if (hit)
            {
                if (invulerability <= 0)
                {
                    health -= damage;
                    invulerability = invulerabilityTime;
                    SFXManager.instance.Spawn("PlayerGetHit");
                }
                hit = false;
                damage = 0;
            }
            if (invulerability > 0)
            {
                render = !render;
                Render(render);
                invulerability -= Time.deltaTime;
            }
            else if (!render)
            {
                render = true;
                Render(true);
            }
            if (health <= 0)
            {
                Die();
            }
        }

        private void handlePlayerAnimation() {
            Vector2 pos = myCrosshair.transform.position;
            Vector2 target = new Vector2(transform.position.x, 1f);//horizontal divider, vertical divider
            float margin = 1f;

			if (PlayerUtil.nearZero(pos.x - target.x, margin) && PlayerUtil.nearZero(pos.y - target.y, margin) || PlayerUtil.nearZero(pos.x - target.x, margin) && pos.y < target.y)
                anim.SetInteger(state, 0);//torso middle low
            else if (pos.x < target.x && pos.y < target.y)
                anim.SetInteger(state, 1);//torso left low
            else if (pos.x < target.x && pos.y > target.y)
                anim.SetInteger(state, 2);//torso left high
            else if (PlayerUtil.nearZero(pos.x - target.x, margin) && pos.y > 0)
                anim.SetInteger(state, 3);//torso middle high
            else if (pos.x > target.x && pos.y > target.y)
                anim.SetInteger(state, 4);//torso right high
            else if (pos.x > target.x && pos.y < target.y)
                anim.SetInteger(state, 5);//torso right low
        }

        private void handlePlayerMove(float magnitude)
        {
            if (this.useAcceleration)
            {
                this.movement.x = this.movement.x + this.acceleration * magnitude;
            }
            else
            {
                this.movement.x = this.maxSpeed * magnitude;
            }
            if (this.movement.x > this.maxSpeed)
            {
                this.movement.x = this.maxSpeed * magnitude;
            }
        }
        private void movePlayer()
        {
            //check horizontal
            if (this.movement.x > this.maxSpeed)
            {
                this.movement.x = this.maxSpeed;
            }
            else if (this.movement.x < -this.maxSpeed)
            {
                this.movement.x = -this.maxSpeed;
            }

            //check vertical
            if (transform.position.y > PlayerUtil.surfacePos)
            {
                this.movement.y -= 0.5f;
            }
            else if (transform.position.y <= PlayerUtil.surfacePos && (Time.time - this.jumpStartTime) > 0.5f)
            {
                this.movement.y = this.movement.y * 0.5f;
                Vector2 targetPos = new Vector2(transform.position.x, PlayerUtil.surfacePos);
                transform.position = Vector2.Lerp(transform.position, targetPos, 0.1f);
            }

            //move
            transform.Translate(this.movement * Time.deltaTime, Space.World);

            // check bounds
            Vector2 xBounds = GameManager.xBounds;
            Vector2 yBounds = GameManager.yBounds;

            if (transform.position.x < xBounds.x)
            {
                transform.position = new Vector2(xBounds.x, transform.position.y);
                this.movement.x *= -1;
            }
            else if (transform.position.x > xBounds.y)
            {
                transform.position = new Vector2(xBounds.y, transform.position.y);
                this.movement.x *= -1;
            }

            if (transform.position.y > yBounds.x)
            {
                transform.position = new Vector2(transform.position.y, yBounds.x);
                this.movement.y *= -1;
            }
            else if (transform.position.y < yBounds.y)
            {
                transform.position = new Vector2(transform.position.y, yBounds.y);
                this.movement.y *= -1;
            }
        }

        //TODO: change ordering of check
        private void moveCrosshair(Vector2 inputValues)
        {
            Vector2 xBounds = GameManager.xBounds;
            Vector2 yBounds = GameManager.yBounds;

            Vector2 trueCrossSpeed = this.getIsShooting() ?
                                        new Vector2(this.crosshairSlowSpeed, this.crosshairSlowSpeed) :
                                        new Vector2(this.crosshairSpeed, this.crosshairSpeed);

            if (inputValues.x < 0 && myCrosshair.transform.position.x < xBounds.x)
            {
                trueCrossSpeed.x = 0;
            }
            else if (inputValues.x > 0 && myCrosshair.transform.position.x > xBounds.y)
            {
                trueCrossSpeed.x = 0;
            }

            if (inputValues.y < 0 && myCrosshair.transform.position.y > yBounds.x)
            {
                trueCrossSpeed.y = 0;
            }
            else if (inputValues.y > 0 && myCrosshair.transform.position.y < yBounds.y)
            {
                trueCrossSpeed.y = 0;
            }

            //move
            Vector2 crosshairMove = new Vector2(inputValues.x * trueCrossSpeed.x, -1 * inputValues.y * trueCrossSpeed.y);
            myCrosshair.transform.Translate(crosshairMove * Time.deltaTime, Space.World);
        }

        private void handleJumping()
        {
            float timeSinceJumping = (Time.time - this.jumpStartTime);

            if (this.getIsOnSurface() && timeSinceJumping > 0.5f)
            {
                this.isJumping = false;
            }
        }

        //Actions
        private void actionShoot()
        {
            //you're shooting
        }

        private void actionJump()
        {
            if (!this.getAirborne())
            {
                this.isJumping = true;
                this.jumpStartTime = Time.time;
                this.movement.y = this.jumpSpeed;
            }
        }

        //TODO update this to use getRelativeRotation()
        private void lean(float magnitude)
        {
            float currRot = transform.localEulerAngles.z;
            //moving left: rotate positive, moving right: rotate negative
            float rotateAttempt = currRot - magnitude * this.rotationSpeed * Time.deltaTime;

            float direction = Mathf.Ceil(magnitude);
            float max360 = 360f - this.maxRotation;

            if (direction > 0 && (currRot < -this.maxRotation || (currRot > 180 && currRot < max360)))
            {//right
             //do not rotate
            }
            else if (direction < 0 && (currRot < 180 && currRot > this.maxRotation))
            {//left
             //do not rotate
            }
            else
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, rotateAttempt);
            }

        }

        //getters
        public bool getAirborne()
        {
            return this.isJumping;
        }

        public bool getIsShooting()
        {
            return this.isShooting;
        }
        public bool getIsOnSurface()
        {
            if (transform.position.y <= PlayerUtil.surfacePos)
            {
                return true;
            }
            else
            {
                return false;
            }
            // return this.isOnSurface;
        }

        public bool getShouldBob()
        {
            return this.shouldBob && !this.getAirborne() && !this.isPlayerMoving;
        }

        public bool getJumpPressed()
        {
            if (this.triggerInput.x > 0)
            {
                return true;
            }
            return false;
        }

        public void PlayOnBeat()
        {
            if (isShooting)
            {
                if (shooting == 0)
                    shooting = playerNum;
                if (shooting == playerNum)
                    SFXManager.instance.Spawn("Shoot");
                bullets.SpawnFollow(new Vector3(transform.position.x, transform.position.y, 1), myCrosshair.transform);
            }
            else
            {
                if (shooting == playerNum)
                    shooting = 0;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "EnemyBomb")
            {
                hit = true;
                damage = collision.gameObject.GetComponent<Bullets.Bomb>().damage;
            }
            if (collision.gameObject.tag == "Shockwave")
            {
                hit = true;
                damage = 1000000;
            }
            if (collision.gameObject.tag == "Player")
            {
                this.handlePlayerDidCollide(collision);
            }
        }

        private void Die()
        {
            TempoManager.instance.objects.Remove(this);
            GameManager.instance.Remove(this.gameObject);
            Destroy(this.gameObject);
            Destroy(this.myCrosshair);
        }
    }

}
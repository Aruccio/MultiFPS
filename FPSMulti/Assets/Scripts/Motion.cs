using UnityEngine;

namespace FPSMulti
{
    public class Motion : MonoBehaviour
    {

        #region Variables
        public float speed;
        public float sprintModifier;
        public Camera normalCam;
        public Transform weaponParent;
        public float jumpForce;
        public LayerMask ground;
        public Transform groundDetector;


        private Rigidbody rig;
        private float baseFOV; //field of view
        private float sprintFOVModifier=1.25f;
        private bool sprint = false;
        private bool jump = false;
        private bool aim = false;
        private Vector3 weaponParentOriginal;
        private float movementCounter;
        private float idleCounter;

        #endregion

        #region Monobehaviour Callbacks
        private void Start()
        {
            Camera.main.enabled = false;
            rig = GetComponent<Rigidbody>();
            baseFOV = normalCam.fieldOfView;
            weaponParentOriginal = weaponParent.localPosition;
            
        }

        void Update()
        {
            //Axes
            float hMove = Input.GetAxisRaw("Horizontal");
            float vMove = Input.GetAxisRaw("Vertical");
            


            //Controls
            sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            jump = Input.GetKeyDown(KeyCode.Space);
            aim = Input.GetKey(KeyCode.Z) || Input.GetMouseButton(1);

            //States
            bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
            bool isJumping = jump && isGrounded;
            bool isSprinting = sprint && vMove > 0 && !isJumping && isGrounded; //czyli tylko gdy porusza siê w przod
          


            //Jumping
            if (isJumping)
                rig.AddForce(Vector3.up * jumpForce);

            //Headbob
            //if(vMove==0 && hMove==0)//idle
            //{
            //    Headbob(idleCounter, 0.1f, 0.1f);
            //    idleCounter += Time.deltaTime;
            //}
            //else
            //{
            //    Headbob(movementCounter, 0.2f, 0.2f);
            //    movementCounter += Time.deltaTime;
            //}

        }

        // Update is called once per frame
        void FixedUpdate()
        {

            //Axes
            float hMove = Input.GetAxisRaw("Horizontal");
            float vMove = Input.GetAxisRaw("Vertical");


            //Controls
            sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            jump = Input.GetKeyDown(KeyCode.Space);

            //States
            bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
            bool isJumping = jump && isGrounded;
            bool isAiming = aim;
            bool isSprinting = sprint && vMove > 0 && !isJumping && isGrounded && !isAiming ; //czyli tylko gdy porusza siê w przod
            

            //Movement
            Vector3 direction = new Vector3(hMove,0, vMove);
            direction.Normalize();


            
            float adjSpeed = speed;
            //Sprint - velocity and field of view
            if (isSprinting)
            {
                adjSpeed *= sprintModifier;
                normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV*sprintFOVModifier, Time.deltaTime *8f);
            }
            else
            {
                normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV, Time.deltaTime * 8f);
            }


            Vector3 targetVelocity = transform.TransformDirection(direction) * adjSpeed * 10 * Time.deltaTime;
            targetVelocity.y = rig.velocity.y;
            rig.velocity = targetVelocity;
        }

        #endregion

        #region Private Methods
        //void Headbob(float z, float xintensity, float yintensity)
        //{
        //    weaponParent.localPosition = new Vector3((Mathf.Cos(z) * xintensity)/3+0.3f, (Mathf.Sin(z) * yintensity)/3+0.5f , weaponParentOriginal.z);
        //}
        
        
        #endregion

    }
}
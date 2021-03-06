using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 6F;
	public float sensitivityY = 6F;

	static public float sensitivity = 6f;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -80f;//-60F;
	public float maximumY = 70f;//60F;

	public float rotationY = 5F;

	void OnEnable()
	{
		if(Level.current != null && Level.current.Index != 0)
			rotationY = transform.localEulerAngles.x > 0 ? 0 : -transform.localEulerAngles.x;
	}

	private static Quaternion ConvertRotation(Quaternion q)
	{
		return new Quaternion(q.x, q.y, -q.z, -q.w);
	}

	float xExtraAngle = 0;

	void Update ()
	{
		
		if(Input.touchCount > 0)
			this.enabled = false;

		if (axes == RotationAxes.MouseXAndY)
		{
			float rotationX = transform.localEulerAngles.y + (Input.GetAxis("Mouse X") + Input.GetAxis("Joy X")) * sensitivityX;
			
			rotationY += (Input.GetAxis("Mouse Y") + Input.GetAxis("Joy Y")) * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}
		else if (axes == RotationAxes.MouseX)
		{
			

				if (Player.VRMode != Game.VRMode.NONE)
				{
				#if UNITY_ANDROID
				{
					Vector3 gravity = Input.acceleration;
					float fi   = Mathf.Rad2Deg*Mathf.Atan(-gravity.z/(Mathf.Sign(gravity.y)*Mathf.Sqrt(gravity.y*gravity.y+gravity.x*gravity.x*0.01f)));
					float teta = Mathf.Rad2Deg*Mathf.Atan(-gravity.x/(Mathf.Sqrt(gravity.z*gravity.z+gravity.y*gravity.y)));
					rotation = Quaternion.Slerp(rotation,Quaternion.Euler(-fi,Input.compass.magneticHeading-initCompassHeading,teta),Time.deltaTime*4.0f);
					transform.localEulerAngles = new Vector3(0, rotation.eulerAngles.y + xExtraAngle, 0);
				}
				#else
				{
					Debug.LogWarning("sssss");
				if(Input.gyro.enabled)
				{
						
					xExtraAngle += Input.GetAxis("Axis3") * sensitivityX;
					//transform.eulerAngles = new Vector3 (0, Input.gyro.rotationRateUnbiased.y, 0);
					var rot = ConvertRotation (Input.gyro.attitude);
					Vector3 euler = (Quaternion.Euler (90f, 0f, 0f) * rot).eulerAngles;
					transform.eulerAngles = new Vector3 (0, euler.y + xExtraAngle, 0);
				}  
					else
					{
						transform.Rotate(0, (Input.GetAxis("Mouse X") + Input.GetAxis("Axis3")) * sensitivityX, 0); //"Joy X"

					}
				}
				#endif
				}
				else
				{
				
					transform.Rotate(0, (Input.GetAxis("Mouse X") + Input.GetAxis("Axis3")) * sensitivityX, 0); //"Joy X"
				}

		}
		else
		{
			
				if (Player.VRMode != Game.VRMode.NONE)
			{
				#if UNITY_ANDROID
				{
					Vector3 gravity = Input.acceleration;
					float fi   = Mathf.Rad2Deg*Mathf.Atan(-gravity.z/(Mathf.Sign(gravity.y)*Mathf.Sqrt(gravity.y*gravity.y+gravity.x*gravity.x*0.01f)));
					float teta = Mathf.Rad2Deg*Mathf.Atan(-gravity.x/(Mathf.Sqrt(gravity.z*gravity.z+gravity.y*gravity.y)));
					rotation = Quaternion.Slerp(rotation,Quaternion.Euler(-fi,Input.compass.magneticHeading-initCompassHeading,teta),Time.deltaTime*4.0f);
					transform.localEulerAngles = new Vector3(-rotation.eulerAngles.y, transform.localEulerAngles.y, 0);
				}
				#else
				{
				if(Input.gyro.enabled)
				{
					var rot = ConvertRotation (Input.gyro.attitude);
					Vector3 euler = (Quaternion.Euler (90f, 0f, 0f) * rot).eulerAngles;
					transform.localEulerAngles = new Vector3 (euler.x, 0, euler.z);
				}
					else
					{
						rotationY += (Input.GetAxis ("Mouse Y") + Input.GetAxis ("Axis4")) * sensitivityY; // "Joy Y"
						rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

						transform.localEulerAngles = new Vector3 (-rotationY, transform.localEulerAngles.y, 0);
					}
				} 
				#endif
				}

				else
				{
					rotationY += (Input.GetAxis ("Mouse Y") + Input.GetAxis ("Axis4")) * sensitivityY; // "Joy Y"
					rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

					transform.localEulerAngles = new Vector3 (-rotationY, transform.localEulerAngles.y, 0);
				}
			
		}
	}

	public void EnableCompass(bool on)
	{
		if( on )
		{
			Input.compass.enabled = true;
			CancelInvoke ("InitCompassHeading");
			Invoke ("InitCompassHeading",0.1f);
		}
		else
		{
			Input.compass.enabled = false;
			CancelInvoke ("InitCompassHeading");
		}
	}

	void InitCompassHeading()
	{
		initCompassHeading = -rotationY+Input.compass.magneticHeading;
	}
	private float initCompassHeading=0f;
	Quaternion rotation;
//	public Mesh mesh;
//	public Material mat;
//	public void OnPostRender() {
//		// set first shader pass of the material
//		mat.SetPass(0);
//		// draw mesh at the origin
//		Graphics.DrawMeshNow(mesh, Vector3.zero, Quaternion.identity);
//	}
	
	void Start ()
	{

		#if UNITY_ANDROID
			EnableCompass(true);
		#endif
//		mat = Game.BaseMaterial;
//		mat.color = Color.black;
//		mesh = CustomMesh.Circle ();
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}
}
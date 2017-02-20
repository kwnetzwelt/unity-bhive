using UnityEngine;
using System.Collections;

public class Propeller : MonoBehaviour {
	/// <summary>
	/// The Propeller Prefab
	/// </summary>
	public GameObject propeller;
	/// <summary>
	/// The transform the propeller will be put on
	/// </summary>
	public GameObject propellerTarget;

	GameObject instance;
	bool anyAxisInput = true;
	void OnEnable()
	{
		instance = GameObject.Instantiate(propeller) as GameObject;
		instance.layer = LayerMask.NameToLayer("Player");
		instance.transform.parent = propellerTarget.transform;
		instance.transform.localPosition = Vector3.zero;
		instance.transform.localRotation = Quaternion.Euler(0, 270, 0);

		var parentRB = propellerTarget.AddComponent<Rigidbody>();
		parentRB.isKinematic = true;
		parentRB.useGravity = false;

		
		instance.AddComponent<BoxCollider>();
		instance.GetComponent<Collider>().isTrigger = true;
		Rigidbody rb = instance.AddComponent<Rigidbody>();
		rb.isKinematic = false;
		rb.useGravity = false;
		rb.angularDrag = 0.5f;
		rb.maxAngularVelocity = 5000;

		var hj = propellerTarget.AddComponent<HingeJoint>();
		hj.autoConfigureConnectedAnchor = false;
		hj.anchor = Vector3.zero;
		hj.axis = new Vector3(1, 0, 0);
		hj.connectedBody = instance.GetComponent<Rigidbody>();
		hj.connectedAnchor = Vector3.zero;


		hj.useMotor = false;

	}
	void SetMoving(bool newValue)
	{
		if (newValue) {
			float sign = 1;
			if (Input.GetAxis("Vertical") != 0)
				sign = Mathf.Sign( Input.GetAxis("Vertical"));
			else
				sign = Mathf.Sign( Input.GetAxis("Horizontal"));
			instance.GetComponent<Rigidbody>().AddTorque(sign * new Vector3(0, 0, 1) * 1000 * Time.deltaTime, ForceMode.Acceleration);
		}
	}

	void Update()
	{
		if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) 
			SetMoving(true);

	}

	void OnDisable()
	{
		GameObject.Destroy(instance);
		GameObject.Destroy(propellerTarget.gameObject.GetComponent<HingeJoint>());
		GameObject.Destroy(propellerTarget.gameObject.GetComponent<Rigidbody>());
	}
}

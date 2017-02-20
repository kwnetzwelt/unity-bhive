using UnityEngine;
using System.Collections;
using System;

public class CharacterAnimator : MonoBehaviour {
	public Animator animator;
	public CharacterController controller;
	public FPSWalkerEnhanced walker;
	bool setJump = false;
	void Start () {
		walker.OnWillJump += OnJump;
	}

	void OnDestroy()
	{
		walker.OnWillJump -= OnJump;
	}


	void OnJump()
	{
		setJump = true;
	}
	
	void Update () {

		animator.SetFloat("speed", controller.velocity.magnitude);
		animator.SetFloat("rightLeft", Input.GetAxis("Horizontal"));

		animator.SetBool("jump", setJump);
		setJump = false;

	}
}

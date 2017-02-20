using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class AnimalAnimate : MonoBehaviour {

	public Vector3 waveAmp;
	public Vector3 waveSpeed;
	public float disolveSpeed = 1;
	Material material;
	public AnimationCurve curve;

	public float extrudeVis = 0;
	public float extrudeHid = 25;
	public float clipVis = 0;
	public float clipHid = 1;

	public float gridVis = 0.1f;
	public float gridHid = 1.0f;

	float frag = 1;
	Vector3 startPosition;

	void Start()
	{
		visible = true;
		material = GetComponent<Renderer>().material;
		startPosition = this.transform.localPosition;

	}
	public void Dissolve()
	{
		visible = false;
	}
	public void Create()
	{
		visible = true;
	}


	public bool visible;

	void Update () {

		//
		// natural wave animation
		//

		Vector3 pos = Vector3.zero ;
		pos.x = Mathf.Sin(Time.time * waveSpeed.x) * waveAmp.x;
		pos.y = Mathf.Sin(Time.time * waveSpeed.y) * waveAmp.y;
		pos.z = Mathf.Sin(Time.time * waveSpeed.z) * waveAmp.z;
		this.transform.localPosition = startPosition + pos;


		if(Input.GetKeyUp(KeyCode.Space))
		{
			if(visible)
				Dissolve();
			else
				Create();
		}
		UpdateMaterial();
	}
	void UpdateMaterial()
	{
		if(visible)
			frag += Time.deltaTime * disolveSpeed;
		else
			frag -= Time.deltaTime * disolveSpeed;
		frag = Mathf.Clamp01(frag);
		float curveV = curve.Evaluate(frag);

		material.SetFloat("_Extrude", extrudeHid + curveV * (extrudeVis-extrudeHid));
		material.SetFloat("_Clip", clipHid + curveV * (clipVis-clipHid));
		material.SetFloat("_GridSize", gridHid + curveV * (gridVis-gridHid));
	}
}

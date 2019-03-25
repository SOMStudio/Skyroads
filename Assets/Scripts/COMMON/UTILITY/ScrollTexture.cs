using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour {

	public float scrollSpeed = 0.5f;

	Renderer rend;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		float offset = Time.deltaTime * scrollSpeed;
		//rend.material.SetTextureOffset ("_MainTex", new Vector2 (0, offset));
		rend.material.mainTextureOffset = new Vector2 (0.0f, offset);
	}
}

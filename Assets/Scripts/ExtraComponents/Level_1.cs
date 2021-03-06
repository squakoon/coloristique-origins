﻿using UnityEngine;
using System.Collections;

public class Level_1 : MonoBehaviour 
{
	Level level;
	Cell cell;
	GameObject circle;

	InfoTable info;
	GameObject arrow;

	bool drawingEnded = false;

	bool gif = false;

	void AddAnimationForInfo()
	{
		Vector3[] points = new Vector3[]
		{new Vector3(1.5f, 1.5f, -1.5f),
			(new Vector3(1.5f, 1.5f, -1.5f))*1.15f,
			new Vector3(1.5f, 1.5f, -1.5f)};
		
		AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, points, 1f);
		//clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(2, 2f, -2f), new Vector3(1.5f, 1.5f, -1.5f), 1f, 0, clip);
		
		/**/
		
		if(info.text.GetComponent<Animation>() == null)
			info.text.AddComponent<Animation>();
		
		info.text.GetComponent<Animation>().AddClip(clip, "Pulse");
		
	}
	GameObject symbol;

	void CreateMobiusStrip()
	{
		float thick = 0.18f;
		float contourThick = 0.02f;
		float radius = 0.8f;
		Symbol firstStrip = Symbol.Create(Symbol.Type.MOBIUS_STRIP, thick, radius, Obj.Colour.WHITE, contourThick);
		Symbol secondStrip = Symbol.Create(Symbol.Type.MOBIUS_STRIP, thick, radius, Obj.Colour.WHITE, -contourThick);
		//Destroy (firstStrip.GetComponent<Symbol> ());
		//Destroy (firstStrip.GetComponent<Symbol> ());

		Symbol mainStrip = Symbol.Create(Symbol.Type.MOBIUS_STRIP, thick - contourThick, radius, Obj.Colour.BLACK);
		mainStrip.firstStrip = firstStrip.gameObject;
		mainStrip.secondStrip = secondStrip.gameObject;

		GameObject[] symbols = new GameObject[] {
			firstStrip.gameObject,
			secondStrip.gameObject,
			mainStrip.gameObject
		};

		symbol = new GameObject ("Symbol");
		foreach (GameObject obj in symbols)
		{
			obj.transform.parent = symbol.transform;
		}


		symbol.transform.parent = level.room [2].transform;
		symbol.transform.localEulerAngles = Vector3.up * 90f;
		Player.SetPosition (symbol, level.room [2], new Vector3 (30, 1.65f, 50)); //30, 1.65f, 50
	}

	void CreateTesseract()
	{
				GameObject tes = symbol = Tesseract.Create ().gameObject;
				//tes.transform.localEulerAngles = Vector3.up * 45f;
				Player.SetPosition(tes, level.room [2], new Vector3 (50, 1.6f, 50));

				foreach (Side s in level.room[2].side)
				{
					foreach (Line l in s.line)
						l.Repaint ();
				}
	}

	void CreatePenroseTriangle()
	{
		symbol = PenroseTriangle.Create (GameObject.FindObjectsOfType<Camera>());

		symbol.transform.parent = level.room [2].transform;
		symbol.transform.localScale = Vector3.one * 0.02f;
		//symbol.transform.localEulerAngles = Vector3.up * 90f;
		Player.SetPosition (symbol, level.room [2], new Vector3 (30, 1.65f, 50));
	}

	void CreateSymbol()
	{
//		float thick = 0.18f;
//		float contourThick = 0.02f;
//		float radius = 0.8f;
//		GameObject[] symbols = new GameObject[] {
//			Symbol.Create(Symbol.Type.MOBIUS_STRIP, thick, radius, Obj.Colour.WHITE, contourThick).gameObject,
//			Symbol.Create(Symbol.Type.MOBIUS_STRIP, thick, radius, Obj.Colour.WHITE, -contourThick).gameObject,
//			Symbol.Create(Symbol.Type.MOBIUS_STRIP, thick - contourThick, radius, Obj.Colour.BLACK).gameObject
//		};
//
//		symbol = new GameObject ("Symbol");
//		foreach (GameObject obj in symbols)
//		{
//			obj.transform.parent = symbol.transform;
//		}


		//CreateTesseract ();
		//CreateMobiusStrip ();
		CreatePenroseTriangle();
		//symbol.transform.localScale = Vector3.one * 0.02f;
//		GameObject tes = symbol = Tesseract.Create ().gameObject;
//		//tes.transform.localEulerAngles = Vector3.up * 45f;
//		Player.SetPosition(tes, level.room [2], new Vector3 (50, 1.6f, 50));

//		foreach (Side s in level.room[2].side)
//		{
//			foreach (Line l in s.line)
//				l.Repaint ();
//		}
	}

	Vector3 originalArrowPosition;

	void AddAnimationForPointer()
	{
		originalArrowPosition = arrow.transform.position;

		Vector3[] points = new Vector3[]
		{arrow.transform.position,
			arrow.transform.position + Vector3.up*0.1f,
			arrow.transform.position};
		
		AnimationClip clip = Game.CreateAnimationClip (Game.AnimationClipType.POSITION, points, 1f);//Game.CreateAnimationClip(Game.AnimationClipType.SCALE, points, 1f);
		//clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(2, 2f, -2f), new Vector3(1.5f, 1.5f, -1.5f), 1f, 0, clip);
		
		/**/
		
		if(arrow.GetComponent<Animation>() == null)
			arrow.AddComponent<Animation>();
		
		arrow.GetComponent<Animation>().AddClip(clip, "Pulse");

		clip = Game.CreateAnimationClip (Game.AnimationClipType.SCALE, new Vector3 (1, 0, 1), Vector3.one, Game.drawTime);

		arrow.GetComponent<Animation>().AddClip(clip, "Draw");

		clip = Game.CreateAnimationClip (Game.AnimationClipType.SCALE, Vector3.one, new Vector3 (1, 0, 1), 0.35f);
		//clip = Game.CreateAnimationClip (Game.AnimationClipType.SCALE, new Vector3[]{ new Vector3 (1, 0, 1), Vector3.zero}, 0.2f, 0.35f, clip);
		
		arrow.GetComponent<Animation>().AddClip(clip, "ScaleDown");

		clip = Game.CreateAnimationClip (Game.AnimationClipType.SCALE, new Vector3 (1, 0, 1), Vector3.one, 0.35f);
		
		arrow.GetComponent<Animation>().AddClip(clip, "ScaleUp");


		clip = Game.CreateAnimationClip (Game.AnimationClipType.SCALE, new Vector3 (1, 0, 1), Vector3.zero, 0.2f);
		arrow.GetComponent<Animation>().AddClip(clip, "Zero");
	}

	void CreateInfo()
	{
		string useItemButtonName = Game.IsJoystickConnected ? "O" : "E";
		info = InfoTable.NonXmlCreate (useItemButtonName, level.ball [0].gameObject, 0.2f, 0, -0.5f, 0.2f);//0.153f);
		info.transform.GetChild(0).localScale = new Vector3(1.5f, 1.5f, -1.5f);
		info.transform.parent = Level.current.transform;
		if(!info.drawing)
			info.Draw();
	}




	IEnumerator Start() 
	{
		level = Level.current;
		cell = level.room[0].cell[0];

		level.room[0].side[2].GetComponent<MeshRenderer>().enabled = true;
		level.room[0].cell[0].plane.AddComponent<BoxCollider>().size = new Vector3(1, 1, 0);
		//level.room[6].DestroyAnyway();

		//level.room[0].side[0].gameObject.AddComponent<KineticSide>();
		//info = InfoTable.NonXmlCreate("press E to grab the ball", level.ball[1].gameObject, 2.2f, 0.238f);

		CreateInfo ();

		circle = level.room[0].cell[0].plane.transform.GetChild(0).gameObject;
		if(circle.GetComponent<Animation>() == null)
			circle.AddComponent<Animation>();

		//Test ();

		arrow = CustomObject.Arrow ();

		arrow.transform.parent = level.transform;
		arrow.transform.position = level.ball [0].transform.position + Vector3.up;//*0.85f;
//info.text.transform.localScale = Vector3.zero;
		AddAnimationForPointer ();
		arrow.GetComponent<Animation> ().Play ("Draw");

		info.text.transform.localScale = Vector3.zero;
		AddAnimationForInfo();

//
		yield return new WaitForSeconds(Game.drawTime);

		int[] m_queues = new int[]{3000};
		Material[] materials = level.room[0].cell[0].plane.GetComponent<Renderer>().materials;
		for (int i = 0; i < materials.Length && i < m_queues.Length; ++i)
			materials[i].renderQueue = m_queues[i];
		
		circle.GetComponent<Renderer>().material = new Material(Shader.Find("DepthMask"));
		level.room[0].side[2].GetComponent<MeshRenderer>().enabled = false;

		circle.AddComponent<BoxCollider>().isTrigger = true;

		drawingEnded = true;

		Game.DestroyEvent += Destroy;



		CreateSymbol ();
		//info.gameObject.SetActive (false);
	}

	bool IsTriggerEmpty()
	{
		foreach(GameObject obj in cell.trigger.innerObjs)
		{
			if(obj.GetComponent<Ball>() != null)
				return false;
		}

		return true;
	}

	void Destroy()
	{
		Game.DestroyEvent -= Destroy;

		//Destroy(transform.GetChild(0).gameObject);

		Destroy(this);

	}
	/*void LateUpdate()
	{
		if(Level.current.Index != level.Index)
			Destroy(this);
	}*/

	bool needAim = false;
	bool helpVisible = false;

	IEnumerator HideArrow()
	{
		yield return new WaitForSeconds (0.35f);

			arrow.transform.position = originalArrowPosition;

		if (info != null) {
			while (info != null && info.text.GetComponent<Animation>().isPlaying)
				yield return null;

			if (info != null)
			{
			info.text.GetComponent<Animation> ().Play ("Pulse");
			info.text.GetComponent<Animation> ().wrapMode = WrapMode.Loop;
			}
		}
	}

	Tesseract sym;

	void FixedUpdate()
	{
		if (gif)
		{
			Player.player.transform.LookAt (symbol.transform);
			Player.player.transform.eulerAngles = new Vector3 (0, Player.player.transform.eulerAngles.y, 0);

			float angle = Time.time - sym.angle;

			Player.player.transform.position = new Vector3 (
				symbol.transform.position.x + Mathf.Sin (angle * Mathf.Deg2Rad*8f) * 3f, 
				Player.player.transform.position.y, 
				symbol.transform.position.z + Mathf.Cos (angle * Mathf.Deg2Rad*8f) * 3f);
		}
	}

	void Update()
	{
//		if(Player.HasBall && info.drawing)
//		{
//			info.Destroy();
//		}

		if (Input.GetKeyDown (KeyCode.G))
		{
			gif = !gif;

			Player.controller.enabled = false;
			StartCoroutine (Player.DisableControl (20));

//			foreach (Tesseract s in GameObject.FindObjectsOfType<Symbol>())
//			{
//				s.angle = 0;
//			}

			sym = GameObject.FindObjectOfType<Tesseract> ();
		}

		if (arrow) {


			if (Player.HasBall) {

				if(info != null)
				{

					//info.transform.parent = level.ball [0].transform;
					if(info.enabled)
					{
						info.Destroy();
						info.text.transform.localEulerAngles = new Vector3(0, 270, 0);
					}
					else
					{
						info.transform.position = level.ball[0].transform.position;
						info.transform.LookAt(Player.camera.transform);
						//transform.LookAt(Player.camera.transform);
						info.transform.localEulerAngles -= info.transform.localEulerAngles.x * Vector3.right;
					}

				}
				Object.Destroy (arrow, 0.35F);

			} else {
				arrow.transform.LookAt (Player.camera.transform);


				//arrow.transform.localEulerAngles -= transform.localEulerAngles.x * Vector3.right;

				if (Vector3.Distance (arrow.transform.position, Player.camera.transform.position) <= Ball.distance) {
					if (!arrow.GetComponent<Animation> ().IsPlaying ("ScaleDown")
						&& !helpVisible) {

						arrow.GetComponent<Animation> ().Stop ();
						arrow.GetComponent<Animation> ().PlayQueued ("ScaleDown");
						arrow.GetComponent<Animation> ().wrapMode = WrapMode.Once;
						arrow.GetComponent<Animation> ().PlayQueued ("Zero");

						info.text.transform.localScale = new Vector3 (1.5f, 1.5f, -1.5f);
						info.Draw();

						StartCoroutine (HideArrow ());
						//arrow.GetComponent<Animation>()["ScaleDown"].speed = 5;
						helpVisible = true;

					}
				} else if (helpVisible) {
					arrow.GetComponent<Animation> ().wrapMode = WrapMode.Once;
					//arrow.GetComponent<Animation>().Stop();
					//arrow.GetComponent<Animation>()["Draw"].speed = 5f;
					//arrow.transform.localScale = new Vector3(1, 0, 1);
					arrow.GetComponent<Animation> ().PlayQueued ("ScaleUp");
					info.Hide();
					helpVisible = false;

				} else if (!arrow.GetComponent<Animation> ().isPlaying) {
					//arrow.GetComponent<Animation>()["Pulse"].speed = 1f;
					arrow.GetComponent<Animation> ().Play ("Pulse");
					arrow.GetComponent<Animation> ().wrapMode = WrapMode.Loop;
				}
			}
		}


//		if(info.drawing && !info.text.GetComponent<Animation>().isPlaying)
//		{
////			info.text.GetComponent<Animation>().Play("Pulse");
////			info.text.GetComponent<Animation>().wrapMode = WrapMode.Loop;
//
//			arrow.GetComponent<Animation>().Play("Pulse");
//			arrow.GetComponent<Animation>().wrapMode = WrapMode.Loop;
//		}



		if(drawingEnded)
		{
			if(cell.IsActive && !level.room[0].side[2].GetComponent<MeshRenderer>().enabled)
			{
				level.room[0].side[2].GetComponent<MeshRenderer>().enabled = true;
				(level.room[0].cell[0].plane.transform.GetChild(0).GetComponent<Renderer>().material = Game.BaseMaterial).color = Game.Black;
			}
			else if(!cell.IsActive && level.room[0].side[2].GetComponent<MeshRenderer>().enabled && !cell.cell.GetComponent<Animation>().isPlaying)
			{
				level.room[0].side[2].GetComponent<MeshRenderer>().enabled = false;
				level.room[0].cell[0].plane.transform.GetChild(0).GetComponent<Renderer>().material = new Material(Shader.Find("DepthMask"));
			}


			RaycastHit hit;
			if(Physics.Raycast(Player.camera.transform.position, Player.camera.transform.forward, out hit))
			{
				if(hit.transform == circle.transform && IsTriggerEmpty() && circle.transform.localScale.x < 1.5f)
				{
					Player.AimActive(true);
					needAim = true;
				}
				else if(needAim)
				{
					needAim = false;
					Player.AimActive(false);
				}

				if(hit.transform == circle.transform && Game.IsInputActionButtonClick() && IsTriggerEmpty())
				{
					if(circle.transform.localScale.x < 1.5f)
					{
						if(circle.GetComponent<Animation>().isPlaying)
						{
							circle.GetComponent<Animation>().Stop();
						}

						circle.transform.localScale += Vector3.one * Time.deltaTime * time;

						if(circle.transform.localScale.x >= 1.5f)
						{
							cell.plane.GetComponent<BoxCollider>().enabled = false;
							cell.trigger.enabled = false;
						}
					}
					
				}
				else if(circle.transform.localScale.x < 1.5f && circle.transform.localScale.x > 0.1f && !circle.GetComponent<Animation>().isPlaying)
				{
					CircleScaleDown();
					if(needAim)
					{
						Player.AimControl(false);
						needAim = false;
					}
				}
				else if(!cell.trigger.enabled && Player.HasBall)
				{
					CircleScaleDown();
					if(needAim)
					{
						Player.AimControl(false);
						needAim = false;
					}
				}
			}

		}



		//Debug.LogWarning(Player.HasBall);
	}
	float time = 1f;
	void CircleScaleDown()
	{
		Debug.LogWarning("SCALE");

		AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, 
		                                              circle.transform.localScale, 
		                                              Vector3.one * 0.1f, 
		                                              (time/1.4f) * (circle.transform.localScale.x));
		
		
		
		if(circle.GetComponent<Animation>().GetClip("ScaleDown") != null)
		{
			circle.GetComponent<Animation>().RemoveClip("ScaleDown");
		}
		
		circle.GetComponent<Animation>().AddClip(clip, "ScaleDown");
		circle.GetComponent<Animation>().Play("ScaleDown");

		cell.plane.GetComponent<BoxCollider>().enabled = true;
		cell.trigger.enabled = true;
	}

}

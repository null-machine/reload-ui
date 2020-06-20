using System.Collections;
using UnityEngine;

class Clip : MonoBehaviour {
	
	[SerializeField]
	private SpriteRenderer divider, ring, pulse;
	[SerializeField]
	private Transform clear;
	
	[SerializeField] [Range(2, 6)]
	private int size = 3;
	[SerializeField]
	private float fireTime = 0.2f, chargeTime = 2f, reloadTime = 2f;
	[SerializeField]
	private Color regular, charged;
	
	private int clip;
	
	private AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
	
	private bool queued;
	private bool locked;
	
	private void Start() {
		clip = size;
		float gap = 360f / size;
		float shift = gap / 2f;
		ring.sharedMaterial.SetFloat("_Start", shift);
		SetFill(1f);
		ring.color = regular;
		for (int i = 0; i < size; i++) {
			Instantiate(divider, Vector3.zero, (i * gap + shift).Rot(), transform);
		}
	}
	
	private void Update() {
		if (queued && !locked) {
			StartCoroutine(Shoot());
			queued = false;
		}
		if (Input.GetKeyDown(KeyCode.Mouse0)) {
			if (locked) queued = true;
			else StartCoroutine(Shoot());
		}
	}
	
	private IEnumerator Shoot() {
		locked = true;
		clip--;
		float delta = 1f / size;
		float start = Time.fixedTime;
		float i = 0f;
		while (i < 1f) {
			i = (Time.fixedTime - start) / fireTime;
			SetFill((1f + clip) / size - curve.Evaluate(i) * delta);
			ring.color = Color.Lerp(regular, charged, i > 0.5f ? 1f - i : i);
			yield return new WaitForFixedUpdate();
		}
		SetFill((float)clip / size);
		locked = false;
	}
	
	private IEnumerator Reload() {
		float start = Time.fixedTime;
		float i = 0f;
		while (i < reloadTime) {
			if (locked) start = Time.fixedTime;
			i = Time.fixedTime - start;
			yield return new WaitForFixedUpdate();
		}
		start = Time.fixedTime;
		while
		StartCoroutine(Reload());
		// yield break;
	}
	
	private void SetFill(float fill) {
		ring.sharedMaterial.SetFloat("_Fill", fill);
	}
}
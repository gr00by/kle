using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public GameObject explosion;
	public int hp = 100;

	void Start () {
	
	}
	

	void Update () {
		if (hp <= 0) {
						Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
						Instantiate (explosion, transform.position, randomRotation);
						Destroy (gameObject);
				}
	}

	public void Hurt(int Injury){
		hp -= Injury;

	}
}

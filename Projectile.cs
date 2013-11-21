using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	public GameObject explosion;
	
	
	void Start () 
	{
		//niszczarka pocisku po 2 sekundach
		Destroy(gameObject, 2);
	}
	
	
	void OnExplode()
	{
		//randomowa rotacja eksplozji
		Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
		
		Instantiate(explosion, transform.position, randomRotation);
	}
	
	void OnTriggerEnter2D (Collider2D col) 
	{

		if(col.tag == "Enemy")
		{

			col.gameObject.GetComponent<Enemy>().Hurt(20);

			OnExplode();

			Destroy (gameObject);
		}


	}
}

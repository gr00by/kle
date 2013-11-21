using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour
{
	public Rigidbody2D fireball;	
	public float speed = 5f;
	public float projectDist = 0.2f;
	public float cone = 25f;
	private Rigidbody2D rootrigid;
	private Controller playerCtrl;

	
	void Awake()
	{
		// Setting up the references.
		rootrigid = transform.root.gameObject.GetComponent<Rigidbody2D>();
		playerCtrl = transform.root.gameObject.GetComponent<Controller> ();
	}
	
	
	void Update ()
	{

		if (Input.GetButtonDown ("Fire1")) {
			//Znajdujemy najblizszego typjarza
			GameObject enemy = 	FindClosestEnemy();
			//Okreslamy wspolrzedne poczatkowe i koncowe dla kierunku poruszania sie fojerbola
			float from_x = transform.position.x;
			float from_y = transform.position.y;
			float to_x = transform.position.x + rootrigid.velocity.x;
			float to_y = transform.position.y + rootrigid.velocity.y;
			//print ((Azimuth(from_x, from_y, enemy.transform.position.x, enemy.transform.position.y)));
			
			//Jezeli znalezlismy przeciwnika to wspolrzedne koncowe beda jego pozycja
			if(enemy != null)
			{
				to_x = enemy.transform.position.x;
				to_y = enemy.transform.position.y;
			}
			//Obliczenie kierunku w ktorym ma leciec fojerbol, jesli sie nie poruszamy (na podstawie zmiennej direction ze skryptu Controller.cs)
			else if(rootrigid.velocity.x == 0 && rootrigid.velocity.y == 0)
			{
				to_x = transform.position.x + Sign(Mathf.Sin (playerCtrl.direction * Mathf.PI/180));
				to_y = transform.position.y + Sign(Mathf.Cos (playerCtrl.direction * Mathf.PI/180));
			}
			//Obliczamy offset, zeby fojerbol nie pojawial sie na mordzie goscia
			float off_x = Sign(rootrigid.velocity.x) * projectDist;
			float off_y = Sign(rootrigid.velocity.y) * projectDist;
			if(rootrigid.velocity.x == 0 && rootrigid.velocity.y == 0){
				off_x = Sign(Mathf.Sin (playerCtrl.direction * Mathf.PI/180)) * projectDist;
				off_y = Sign(Mathf.Cos (playerCtrl.direction * Mathf.PI/180)) * projectDist;
			}
			//Obliczamy kat miedzy punktem poczatkowym a koncowym
			float angle = Mathf.Atan ((to_y - from_y)/(to_x - from_x));
			//print (angle);
			float curspeed = speed;
			if(to_x < from_x)
				curspeed = -speed;
			//Obliczamy skladowe x, y predkosci pocisku
			float speed_x = curspeed * Mathf.Cos (angle);
			float speed_y = curspeed * Mathf.Sin (angle);
			//print (new Vector2(speed_x, speed_y));
			//Wstawiamy fojerbola do sceny i nadajemy mu odpowiedni kat obrotu
			Rigidbody2D bulletInstance = Instantiate (fireball, new Vector3(from_x + off_x, from_y + off_y,0), Quaternion.Euler (new Vector3 (0, 0, 270 - Azimuth(from_x, from_y, to_x, to_y)))) as Rigidbody2D;

			//Nadajemy mu predkosc
			bulletInstance.velocity = new Vector2 (speed_x, speed_y);
			//bulletInstance.velocity = new Vector2 (Sign(rootrigid.velocity.x)* speed, Sign (rootrigid.velocity.y)*speed);
			//print (Mathf.Pow (speed_x,2) + Mathf.Pow (speed_y, 2));
			//print (playerCtrl.direction);		
		}

	}
	
	//Funkcja zwraca -1 dla wartosci ujemnych, +1 dla dodatnich, 0 dla 0 ;p
	float Sign(float number) {
		return number < -0.01f ? -1 : (number > 0.01f ? 1 : 0);
	}

	//Obliczarka azymutu
	float Azimuth(float x1, float y1, float x2, float y2)
	{
				float quaternion;
				quaternion = Mathf.Atan (Mathf.Abs ((x2 - x1) / (y2 - y1))) * 180 / Mathf.PI;
				if (y2 - y1 < 0 && x2 - x1 > 0)
						quaternion = 180 - quaternion;
				else if (y2 - y1 < 0 && x2 - x1 <= 0)
						quaternion += 180;
				else if (y2 - y1 >= 0 && x2 - x1 < 0)
						quaternion = 360 - quaternion;
				return quaternion;
	}

	//Znajdujemy typiarza
	GameObject FindClosestEnemy() {
		//Tworzymy liste
		GameObject[] gos;
		//Wdupiamy na liste wszystkie obiekty z tagiem enemy
		gos = GameObject.FindGameObjectsWithTag("Enemy");
		//Deklarujemy pusty gameObject
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		//W petli obliczamy ktory gosc znajdujacy sie w zasiegu wzroku jest najblizej nas
		foreach (GameObject go in gos) {
			//Obliczamy czy gosc znajduje sie w zasiegu wzroku +/- zmienna cone, na podstawie azymutu kierunku naszej postaci, oraz azymutu boku zawartego miedzy nasza postacia a przeciwnikiem
			if((Mathf.Abs(Azimuth(transform.position.x, transform.position.y, go.transform.position.x, go.transform.position.y)-playerCtrl.direction)) % (360 - cone) < cone){
				//Obliczamy odleglosc miedzy nami a przeciwnikiem
				Vector3 diff = go.transform.position - position;
				float curDistance = diff.sqrMagnitude;
				if (curDistance < distance) {
					closest = go;
					distance = curDistance;
				}
			}
		}
		return closest;
	}
}

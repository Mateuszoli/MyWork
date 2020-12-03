using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
	patrol,
	pursuit,
	attack,
    search
};
public class EnemyMainScript : MonoBehaviour {

    public LayerMask Mask1;
    public GameObject Player;
    public EnemyState CurrentState;
    GameObject CurrentP;
	Vector3 Player_pos, Look_direction;
	float distance, to_door_dist, hiden_dist;

	[SerializeField] AudioSource mouth;
	[SerializeField] AudioSource feet;


	[SerializeField] AudioClip[] footsteps_walk;
    [SerializeField] AudioClip[] footspes_run;
    [SerializeField] AudioClip[] idle_sounds;
    [SerializeField] AudioClip[] pursuit_sounds;
    [SerializeField] AudioClip[] attack_sounds;
    [SerializeField] AudioClip[] damage_sounds;

    RaycastHit PlayerHit;
    Vector3 LastKnowPoint, PlayerDirect;
    bool wasReset, ContineuePursuit, CoroutineWaiting;
    float previous_speed, minDistance, maxDistance, damage;
	[SerializeField] GameObject PatrolPoint, HidingZones, Door_obst;
	Animator anim;
	NavMeshAgent Navi;

	public GameObject FindClosestPatrolPoint()  //Znajduje najbliższy punkt do patrolu
	{
        GameObject closest = null;
        bool AllPatrolPointsDisabled;
        AllPatrolPointsDisabled = true;

        GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("PatrolPoint");     //Znajduje wszystkie punkty patrolowe
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
        foreach (GameObject go in gos)
		{
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
            if (go.GetComponent<SphereCollider>().enabled) {
                if (curDistance < distance)
                {
                    closest = go;
                    distance = curDistance;
                }
            }
		}
        foreach(GameObject go in gos)
        {   
            if (go.GetComponent<SphereCollider>().enabled)          //SphereCollider to znacznik punktu; Jeżeli jest nieaktywny zanczy że punkt
            {                                                       //patrolu też jest nie aktywny i nie jest brany pod uwagę;
                AllPatrolPointsDisabled = false;
                break;
            }
        }
        if (AllPatrolPointsDisabled)        //Jeżeli wszystkie punkty do patolu są wyłączone, jeden z losowych zostaje aktywowany.
        {                                   //Ma to na celu ciągłe patrolowanie terenu przez przeciwnika;
            gos[Random.Range(0, GameObject.FindGameObjectsWithTag("PatrolPoint").Length)].GetComponent<SphereCollider>().enabled = true;
        }
        return closest;
	}
	public GameObject FindClosestDoor()
	{
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("Door");
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject go in gos)
		{
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance)
			{
				closest = go;
				distance = curDistance;
			}
		}
		return closest;
	}
    void IncreaseVolume(AudioSource S)
    {
        S.volume += 0.03f;
    }
    void DecreaseVolume(AudioSource S)
    {
        S.volume -= 0.03f;
    }
	void Start () {
		HidingZones = GameObject.FindGameObjectWithTag ("HiddingZones");    //Strefy w których gracz jest niewidoczny
		mouth = GetComponent<AudioSource> ();
		Navi = GetComponent<NavMeshAgent> ();
		anim = GetComponentInParent<Animator> ();
		Player = GameObject.FindGameObjectWithTag ("Player");
	}
    IEnumerator HiddingTrouble(float time)        //Pozwala wyłączyć na jakiś czas możliowść ukrycia.
    {
        HidingZones.SetActive(false);
        yield return new WaitForSeconds(time);
        HidingZones.SetActive(true);
    }
    public void GunHit()
    {
        Navi.speed = 0f;
        anim.Play("HitByGun");
        StartCoroutine(HiddingTrouble(10f));        //Kiedy przeciwnik zostaje postrzelony przez gracza, na 10 sekund zostaje wyłączona możliwość ukrycia się;
    }
    IEnumerator Grunts(float time)
    {
        yield return new WaitForSeconds(time);
        mouth.PlayOneShot(idle_sounds[Random.Range(0, idle_sounds.Length)]);
        CoroutineWaiting = true;
    }
    IEnumerator Pursuiting(float time)
    {
        yield return new WaitForSeconds(time);
        mouth.PlayOneShot(pursuit_sounds[Random.Range(0, idle_sounds.Length)]);
        CoroutineWaiting = true;
    }
    void Update () {
		Look_direction.x = Player.transform.position.x;
		Look_direction.z = Player.transform.position.z;
		Look_direction.y = transform.position.y;
        
        PlayerDirect = Player.transform.position - transform.position;
        PatrolPoint = FindClosestPatrolPoint ();
        CurrentP = PatrolPoint;
        Door_obst = FindClosestDoor();
		Player_pos = Player.transform.position;
		distance = Vector3.Distance (Player.transform.position, transform.position);
		to_door_dist = Vector3.Distance (gameObject.transform.position, Door_obst.transform.position);
            if(Vector3.Distance(transform.position, LastKnowPoint) < minDistance)
            {
                LastKnowPoint.x = 0f;
                LastKnowPoint.y = 0f;
                LastKnowPoint.z = 0f;
            }
            if (distance > maxDistance)
            {
                CurrentState = EnemyState.patrol;
            }
            if (distance < maxDistance)
            {
                if (Player.GetComponent<HidingScript>().Hiding)
                {
                    if(LastKnowPoint.x != 0 && LastKnowPoint.y != 0 && LastKnowPoint.z != 0)
                    {
                        CurrentState = EnemyState.search;
                    }
                    else
                    {
                        CurrentState = EnemyState.patrol;
                    }
                }
                else
                {
                    if(Physics.Raycast(transform.position, PlayerDirect, out PlayerHit, Mathf.Infinity, Mask1) || !Player.GetComponent<AudioSource>().isPlaying)
                    {
                        CurrentState = EnemyState.pursuit;
                    }
                    else
                    {
                        if(distance > maxDistance/2)
                        {
                            CurrentState = EnemyState.search;
                        }
                    }
                }
            
            if (distance < minDistance)
            {
                CurrentState = EnemyState.attack;
            }
        }

        if (to_door_dist < 2.5f)    //Jeżeli przeciwnik spotyka na swojej drodze drzwi które są otwarte to je otwiera;
            {
                if (Door_obst.GetComponent<DoorScript>() != null)
                {
                if(Door_obst.GetComponent<DoorScript>().TypeOfSafe == SafeMode.Open) {
                    if (!Door_obst.GetComponent<DoorScript>().enOpen)
                    {
                        Door_obst.GetComponent<DoorScript>().PerformAction();
                    }
                }
            }
            }
            if (CurrentState == EnemyState.patrol)
            {
                DecreaseVolume(mouth);
                ContineuePursuit = false;
                wasReset = true;
                Navi.speed = 1f;
                Navi.SetDestination(PatrolPoint.transform.position);
                if (!mouth.isPlaying && CoroutineWaiting)
                {
                    CoroutineWaiting = false;
                    StartCoroutine(Grunts(5f));
                }
                if (!feet.isPlaying)
                {
                    feet.PlayOneShot(footsteps_walk[Random.Range(0, footsteps_walk.Length)]);
                }
            
            }
        if (CurrentState == EnemyState.search)
        {
            DecreaseVolume(mouth);
            ContineuePursuit = false;
            wasReset = true;
            Navi.speed = 1f;
            Navi.SetDestination(LastKnowPoint);
            if (!mouth.isPlaying)
            {
                StartCoroutine(Grunts(3.5f));
            }
            if (!feet.isPlaying)
            {
                feet.PlayOneShot(footsteps_walk[Random.Range(0, footsteps_walk.Length)]);
            }
        }

        if (CurrentState == EnemyState.pursuit){
            if (ContineuePursuit) {
                Navi.speed = previous_speed;
                ContineuePursuit = false;
            }
            Navi.speed += 0.014f;
            IncreaseVolume(mouth);
            if (wasReset)
            {
                wasReset = false;
                Navi.speed = 1f;
            }

            if (Navi.speed > 8.9f) {    //Maksymalna prędkość przeciwnika;
                Navi.speed = 8.9f;
            }
            previous_speed = Navi.speed;  
			Navi.SetDestination (Player_pos);

			if (!mouth.isPlaying) {
                StartCoroutine(Pursuiting(3f));
            }
			if (!feet.isPlaying) {
				feet.PlayOneShot (footspes_run [Random.Range (0, footspes_run.Length)]);
			}
            LastKnowPoint = Player.transform.position;


        }
        anim.speed = 1f;
		if (CurrentState == EnemyState.attack) {
            ContineuePursuit = true;
			Navi.speed = 0f;
			Navi.SetDestination (Player_pos);
			transform.LookAt (Look_direction);
            IncreaseVolume(mouth);
			Player.GetComponent<PlayerHealth>().Health = Player.GetComponent<layerHealth>().Health - damage;
            if (!mouth.isPlaying) {
				mouth.PlayOneShot (attack_sounds [Random.Range (0, attack_sounds.Length)]);
			}
            
		}   //Animacjami steruje zewnętrzny skrypt. Opiera się on na aktualnym statusie i/lub prędkości poruszania się;
        if (Navi.speed == 0 && CurrentState != EnemyState.attack)
        {
            anim.speed = 1f;
            gameObject.GetComponent<MonsterAnimations>().IsStanding = true;
        }
        if (Navi.speed == 0 && CurrentState == EnemyState.attack)
        {
            anim.speed = 1.7f;
            gameObject.GetComponent<MonsterAnimations>().AttackNow = true;
        }
        if (Navi.speed > 0f && Navi.speed <= 1.5f && CurrentState != EnemyState.attack)
        {
            //anim.speed = Navi.speed;
            if(anim.speed < 1f)
            {
                anim.speed = 1f;
            }
            gameObject.GetComponent<MonsterAnimations>().IsWalking = true;
        }
        if (Navi.speed > 1.5f && Navi.speed < 2.6f && CurrentState != EnemyState.attack)
        {
            anim.speed = 1f;
            gameObject.GetComponent<MonsterAnimations>().AlmostRun = true;
        }
        if (Navi.speed > 2.6f && CurrentState != EnemyState.attack)
        {
            anim.speed = 1.25f;
            gameObject.GetComponent<MonsterAnimations>().Pursuit = true;
        }

    }		
}

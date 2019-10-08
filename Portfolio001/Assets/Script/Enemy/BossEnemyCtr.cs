using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyCtr : MonoBehaviour
{
	public float lookRadius;
	public float stopRadius;

	public float walkSpeed = 5f;
	public float waitTime = 5f;
	public float patronChangeTime = 5f;

	private float lastTime = 0f;
	private int _randomAction;

	private bool isAttackmode = false;

	Rigidbody rigid;
	Transform playerTr;

	// Start is called before the first frame update
	void Start()
	{
		rigid = GetComponent<Rigidbody>();
		playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}

	// Update is called once per frame
	void Update()
	{
		float dist = Vector3.Distance(playerTr.position, transform.position);

		if (dist <= lookRadius)
			_randomAction = 3;
		else
			isAttackmode = false;
		
		if (!isAttackmode)
		{
			if (Time.time - lastTime > waitTime)
			{
				lastTime = Time.time;
				_randomAction = Random.Range(1, 3);
			}
		}

		switch (_randomAction)
		{
			case 1:
				Wait();
				break;
			case 2:
				Move();
				break;
			case 3:
				Attack();
				isAttackmode = true;
				break;
		}
	}

	//待機
	void Wait()
	{
		Debug.Log("WaitMode");
	}

	//移動
	void Move()
	{
		Debug.Log("moveMode");
		//transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
	}

	//攻撃
	void Attack()
	{
		Debug.Log("AttackMode");
		if (Time.time - lastTime > patronChangeTime)
		{
			lastTime = Time.time;
			Debug.Log("Attack");
			int _randomParton = Random.Range(1, 4);

			switch (_randomParton)
			{
				//rollingAttack
				case 1:
					rollingAttack();
					break;
				//attack
				case 2:
					NormalAttack();
					break;
				//Spawnturret
				case 3:
					SpawnTurret();
					break;
			}
		}
	}

	void rollingAttack()
	{
		Debug.Log("rollingAttack");
	}

	void NormalAttack()
	{
		Debug.Log("NormalAttack");
	}

	void SpawnTurret()
	{
		Debug.Log("SpawnTurret");
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, lookRadius);
		Gizmos.DrawWireSphere(transform.position, stopRadius);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCtr : MonoBehaviour
{
	public float lookRadius;							//Playerを見つける範囲
	public float stopRadius;							//止まる範囲

	public float fireRate;								//発射速度
	[SerializeField] private GameObject BulletPrefad;	//弾丸
	[SerializeField] private Transform FirePosition;	//発射移置
	private float lasFireTime;							//発射時の一番最後の時間

	public float walkSpeed;								//敵の歩く速度
	public float damping;								//敵の回転速度

	public int HP;                                      //敵のHP
	[SerializeField] private Slider EnemyHP;            //敵のHPスライダー

	private Transform[] patrolpoins;					//パトロール移置
	private int nextpoin = 1;							//次のパトロール移置

	//状態変数
	private bool isWalk = true;							//歩いているかどうか
	private bool isAttack = false;                      //攻撃しているかどうか
	private bool isDead = false;                        //死んだかどうか
	private bool takeDamage = false;

	Transform playerTr;									//Playerの位置
	Vector3 movePos;									//ターゲットの移置(パトロールかPlayerか)

	//必要なコンポネント
	[SerializeField] private Animator anim;				//アニメーション

	void Start()
	{
		playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		patrolpoins = GameObject.Find("WAY_POINTS").GetComponentsInChildren<Transform>();
		EnemyHP.maxValue = HP;
	}

	void Update()
	{
		if (!isDead)
		{
			float distance = Vector3.Distance(playerTr.position, transform.position);

			if (distance <= lookRadius)
			{
				isAttack = false;
				isWalk = true;
				movePos = playerTr.position;

				if (distance < stopRadius)
				{
					isAttack = true;
					isWalk = false;

					go_Attack();
				}
			}
			else if (distance > lookRadius)
			{
				movePos = patrolpoins[nextpoin].position;
				isWalk = true;
			}

			if (takeDamage)
			{
				movePos = playerTr.position;
			}


			Move_Enemy();

			anim.SetBool("isWalk", isWalk);
			anim.SetBool("isAttack", isAttack);
		}		
	}

    //移動関数
    #region Move_Enemy
    void Move_Enemy()
	{
		Vector3 dirction = (movePos - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dirction.x, 0f, dirction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * damping);
		if (!isAttack)
			transform.Translate(Vector3.forward * Time.deltaTime * walkSpeed);
	}
    #endregion Move_Enemy

    //攻撃関数
    #region go_Attack
    void go_Attack()
	{
		if (Time.time - lasFireTime > 1 / fireRate)
		{
			lasFireTime = Time.time;
			GameObject _bullet = Instantiate(BulletPrefad, FirePosition.position, FirePosition.rotation);
			Destroy(_bullet, 3f);
		}
	}
    #endregion go_Attack

    //コライダー判定
    void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Way_Point")
		{
			nextpoin++;
			if (nextpoin >= patrolpoins.Length)
				nextpoin = 1;
		}
	}

	//範囲表示
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, lookRadius);
		Gizmos.DrawWireSphere(transform.position, stopRadius);
	}

    #region Take_Damage
    public void Take_Damage(int Damage)
	{
		HP -= Damage;
		EnemyHP.value = HP;
		takeDamage = true;

		if (HP <= 0)
			Die();
	}
    #endregion Take_Damage

    #region Die
    void Die()
	{
		isDead = true;
		GetComponent<CapsuleCollider>().enabled = false;
		anim.SetTrigger("isDie");
        for (int i = 0; i < patrolpoins.Length; i++)
        {
            Destroy(patrolpoins[i].gameObject);
        }
        
		Destroy(gameObject, 5f);
	}
    #endregion Die
}

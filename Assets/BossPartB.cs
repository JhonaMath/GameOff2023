using UnityEngine;

public class BulletHellShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int numberOfBullets = 360;
    public float bulletSpeed = 5f;
    public float swingAmplitude = 2f; // Adjust the amplitude of the swing
    public float swingFrequency = 2f; // Adjust the frequency of the swing
bool isDead=false;
    public float zigzagAmplitude = 1f;
    public float zigzagFrequency = 2f;

    public float spinVel=5;

    float spinAcc;

    public float createTimer=0.1f;

    public int deletedBullets = 0;

    public float life = 40;


    void Start()
    {
        // ShootBullets();

        InvokeRepeating("ShootBullets", 1,createTimer);
    }

    void ShootBullets1()
    {
        float angleStep = 360f / numberOfBullets;

        for (int i = 0; i < numberOfBullets; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.right;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = direction * bulletSpeed;
        }
    }

    void ShootBullets()
    {   
        spinAcc=(spinAcc+spinVel)%360f;
        float angleStep = 360f / numberOfBullets;

         for (int i = 0; i < numberOfBullets; i++)
        {
            if (i<deletedBullets){
                
            }else{
                float angle = (i * angleStep) + spinAcc;
                Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.right;

                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.velocity = direction * bulletSpeed;
            }
            
            
        }
    }

    void Update(){
        

        if (isDead){
            this.transform.localScale-= this.transform.localScale*Time.deltaTime;
        }
        else{
            float lifeSize = life/8+5;
            this.transform.localScale= new Vector3(lifeSize,lifeSize,0);
         if (life<=0) {
            Destroy(this.gameObject, 1);
            isDead=true;
            }
        }
            
       
        
    }

    void OnTriggerStay2D(Collider2D col){

        if (col.gameObject.tag=="Weapon"){
            life-=(GameController.gameController.playerStats.dmgWeapon * Time.deltaTime);
            GameController.gameController.bossLife-=(GameController.gameController.playerStats.dmgWeapon * Time.deltaTime);
        }
    }

    
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public bool hasPowerup = false;
    private float powerupStrength = 15.0f;
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public GameObject powerupIndicator;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
    }

    

    // Update is called once per frame
    void Update()
    {
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);//offset of the powerup indicator
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);//use the local transrform of the focal point to move instead of Vector3.forward that uses global cordinates
    }
    private void OnTriggerEnter(Collider other)//if we collide with a powerup we destroy it and start the coroutine 
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            powerupIndicator.gameObject.SetActive(true);
            StartCoroutine("PowerupCountdownRoutine");
        }
    }
    IEnumerator PowerupCountdownRoutine()//coroutine that last 7 seconds and then deactivate the powerup indicator
    {
        yield return new WaitForSeconds(7);
        powerupIndicator.gameObject.SetActive(false);
        hasPowerup = false;
    }
    private void OnCollisionEnter(Collision collision)//if we collide with an enemy and we have the powerup we sent to enemies and impulse force ,away from the player
    {
        
        if (collision.gameObject.CompareTag("Enemy")&&hasPowerup)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            enemyRigidbody.AddForce(awayFromPlayer*powerupStrength, ForceMode.Impulse);

            Debug.Log("Player collided with " + collision.gameObject + " with powerup set to " + hasPowerup);
        }
    }
}

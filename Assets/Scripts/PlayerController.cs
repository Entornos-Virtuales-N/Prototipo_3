using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public float jumpForce;
    public float gravityModifier;
    public bool isOnGround = true;
    public bool gameOver = false;
    private Animator playerAnim;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    private AudioSource playerAudio;

    // Inicio 
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        //playerRb.AddForce(Vector3.up * 1000);
        playerAnim = GetComponent<Animator>();
        // 3.4.6 Reproducir clips de audio al saltar y chocar
        playerAudio = GetComponent<AudioSource>();
    }

    // Condiciones
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver) {
          playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
          isOnGround = false;
          playerAnim.SetTrigger("Jump_trig");
          //3.4.3 Detener particulas cuando salta
          dirtParticle.Stop();
          // 3.4.6 Reproducir clips de audio al saltar
          playerAudio.PlayOneShot(jumpSound, 1.0f);
      }
      
    }

    private void OnCollisionEnter(Collision collision) {
      //isOnGround = true;
      if (collision.gameObject.CompareTag("Ground"))
      {
          isOnGround = true;
          // 3.4.3 Reproducir particulas al contacto con suelo
          dirtParticle.Play();
      } else if (collision.gameObject.CompareTag("Obstacle"))
      {
          gameOver = true;
          Debug.Log("Game Over!");

          // 3.3.5 Configuración de animación de caida, colision de player con Obstacle
          playerAnim.SetBool("Death_b", true);
          playerAnim.SetInteger("DeathType_int",1);
          // 3.4.2 Aparece la partícula en colisión
          explosionParticle.Play();
          // 3.4.3 Detener particulas al colicionar
          dirtParticle.Stop();
          // 3.4.6 Reproducir clips de audio al chocar
          playerAudio.PlayOneShot(crashSound, 1.0f);

      }
    }
}

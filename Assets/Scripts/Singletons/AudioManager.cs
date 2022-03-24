using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSoundClips
{
    MouseClick,
    Win,
    Gameover,
    MagicalAttack,
    PhysicalAttack,
    Dead,
    Damage,
    Negative,
    ReloadMana, 
    CheckPoint,
}

public enum EnviromentSoundClip
{
    LevelMusic,
    MenuMusic
}

public enum EnemySoundClips
{
    FlyAttack,
    FlyDamage,
    FlyDead,
    StaticAttack,
    StaticDamage,
    StaticDead,
    PatrolAttack,
    PatrolDamage,
    PatrolDead,
    ExplosiveAntipation,
    ExplosiveExplosion
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("AudioSources")]
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField, Range(0, 1)] private float musicInitialVolumen;

    [Header("Music")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip levelMusic;
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip gameOverSound;

    [Header("SFX Sounds")]
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip checkPointSound;
    [SerializeField] private AudioClip rewardSound;
    [SerializeField] private AudioClip powerUpSound;
    //[SerializeField] private AudioClip bounceSound;

    [Header("Player Sounds")]
    [SerializeField] private AudioClip magicalAttackSound;
    [SerializeField] private AudioClip physicalAttackSound;
    [SerializeField] private AudioClip reloadManaSound;
    [SerializeField] private AudioClip negativeSound;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip deathSound;

    [Header("Enemy Patrol Sounds")]
    [SerializeField] private AudioClip enemyPatrolAttackSound;
    [SerializeField] private AudioClip enemyPatrolDamageSound;
    [SerializeField] private AudioClip enemyPatrolDeathSound;

    [Header("Enemy Static Sounds")]
    [SerializeField] private AudioClip enemyStaticAttackSound;
    [SerializeField] private AudioClip enemyStaticDamageSound;
    [SerializeField] private AudioClip enemyStaticDeathSound;

    [Header("Enemy Fly Sounds")]
    [SerializeField] private AudioClip enemyFlyAttackSound;
    [SerializeField] private AudioClip enemyFlyDamageSound;
    [SerializeField] private AudioClip enemyFlyDeathSound;

    [Header("Enemy Explosive Sounds")]
    [SerializeField] private AudioClip enemyWickSound;
    [SerializeField] private AudioClip enemyExplosion;

    [Header("Boss Sounds")]
    [SerializeField] private AudioClip bossAttackSound;
    [SerializeField] private AudioClip bossDamageSound;
    [SerializeField] private AudioClip bossDeathSound;

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void EnviromentMusic(EnviromentSoundClip soundClip)
    {
        switch (soundClip)
        {
            case EnviromentSoundClip.LevelMusic:
                musicAudioSource.clip = levelMusic;
                break;
            case EnviromentSoundClip.MenuMusic:
                musicAudioSource.clip = mainMenuMusic;
                break;
        }
        musicAudioSource.volume = musicInitialVolumen;
        musicAudioSource.Play();
    }

    public void PlayPlayerSound(PlayerSoundClips soundClip)
    {
        playerAudioSource.volume = 1f;
        switch (soundClip)
        {
            case PlayerSoundClips.MouseClick:
                playerAudioSource.PlayOneShot(clickSound);
                break;
            case PlayerSoundClips.Win:
                playerAudioSource.PlayOneShot(victorySound);
                break;
            case PlayerSoundClips.Gameover:
                playerAudioSource.PlayOneShot(gameOverSound);
                break;
            case PlayerSoundClips.MagicalAttack:
                playerAudioSource.PlayOneShot(magicalAttackSound);
                break;
            case PlayerSoundClips.PhysicalAttack:
                playerAudioSource.PlayOneShot(physicalAttackSound);
                break;
            case PlayerSoundClips.Dead:
                playerAudioSource.PlayOneShot(deathSound);
                break;
            case PlayerSoundClips.Negative:
                playerAudioSource.PlayOneShot(negativeSound);
                break;
            case PlayerSoundClips.ReloadMana:
                playerAudioSource.PlayOneShot(reloadManaSound);
                break;
            case PlayerSoundClips.Damage:
                playerAudioSource.PlayOneShot(damageSound);
                break;
            case PlayerSoundClips.CheckPoint:
                playerAudioSource.PlayOneShot(checkPointSound);
                break;
            //case PlayerSoundClips.BounceMushroom:
            //    playerAudioSource.PlayOneShot(bounceSound);
            //    break;
        }
    }

    public void PlayEnemySound(EnemySoundClips soundClip)
    {
        sfxAudioSource.volume = 1f;
        switch (soundClip)
        {
            case EnemySoundClips.FlyAttack:
                sfxAudioSource.PlayOneShot(enemyFlyAttackSound);
                break;
            case EnemySoundClips.FlyDamage:
                sfxAudioSource.PlayOneShot(enemyFlyDamageSound);
                break;
            case EnemySoundClips.FlyDead:
                sfxAudioSource.PlayOneShot(enemyFlyDeathSound);
                break;
            case EnemySoundClips.StaticAttack:
                sfxAudioSource.PlayOneShot(enemyStaticAttackSound);
                break;
            case EnemySoundClips.StaticDamage:
                sfxAudioSource.PlayOneShot(enemyStaticDamageSound);
                break;
            case EnemySoundClips.StaticDead:
                sfxAudioSource.PlayOneShot(enemyStaticDeathSound);
                break;
            case EnemySoundClips.PatrolAttack:
                sfxAudioSource.PlayOneShot(enemyPatrolAttackSound);
                break;
            case EnemySoundClips.PatrolDamage:
                sfxAudioSource.PlayOneShot(enemyPatrolDamageSound);
                break;
            case EnemySoundClips.PatrolDead:
                sfxAudioSource.PlayOneShot(enemyPatrolDeathSound);
                break;
            case EnemySoundClips.ExplosiveAntipation:
                sfxAudioSource.PlayOneShot(enemyWickSound);
                break;
            case EnemySoundClips.ExplosiveExplosion:
                sfxAudioSource.PlayOneShot(enemyExplosion);
                break;

        }
    }
}
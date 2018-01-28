﻿using UnityEngine;

public class Enemy : MovingObject
{
    public int PlayerDamage;
    public AudioClip EnemyAttack1;
    public AudioClip EnemyAttack2;

    private Animator _animator;
    private Transform _target;
    private bool _skipMove;

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(_target.position.x - transform.position.x) < float.Epsilon)
            yDir = _target.position.y > transform.position.y ? 1 : -1;
        else
            xDir = _target.position.x > transform.position.x ? 1 : -1;

        AttemptMove<Player>(xDir, yDir);
    }

    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        _animator = GetComponent<Animator>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (_skipMove)
        {
            _skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);

        _skipMove = true;
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;

        _animator.SetTrigger("enemyAttack");

        hitPlayer.LoseFood(PlayerDamage);

        SoundManager.instance.RandomizeSfx(EnemyAttack1, EnemyAttack2);
    }
}
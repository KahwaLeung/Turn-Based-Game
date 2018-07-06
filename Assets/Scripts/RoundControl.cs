using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundControl : MonoBehaviour {

    //游戏状态与回合状态
    public static GameState currentGameState = GameState.Menu;
    public static NowTurn currentTurn = NowTurn.Player;

    private Player player;
    private Enemy enemy;

    //等待玩家输入状态
    private bool isWaitForPlayer = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
    }

    //游戏状态机
    public enum GameState
    {
        Menu,
        Game,
        Over
    }

    //敌我回合
    public enum NowTurn
    {
        Player,
        Enemy
    }

    private void OnGUI()
    {
        //游戏菜单状态
        if (currentGameState == GameState.Menu)
        {
            GUI.Window(0, new Rect(Screen.width / 2 - 100, Screen.height / 2 - 30, 200, 60), CallMenu, "Menu");
        }

        //游戏进行状态
        else if (currentGameState == RoundControl.GameState.Game)
        {
            //玩家回合
            if (isWaitForPlayer)
            {
                if (currentTurn == RoundControl.NowTurn.Player)
                {
                    GUI.Window(1, new Rect(Screen.width / 2 + 200, Screen.height / 2, 150, 100), PlayerUnitAction, "Choose Action");
                }
            }
            //敌人回合
            else
            {
                if (currentTurn == RoundControl.NowTurn.Enemy)
                {
                    EnemyUnitAction();
                }
            }
        }

        //游戏结束状态
        else if (currentGameState == GameState.Over)
        {
            GUI.Window(2, new Rect(Screen.width / 2 - 100, Screen.height / 2 - 30, 200, 60), CallRestart, "Game Over");
        }
    }

    private void Update()
    {
        //有单位无生命值则游戏结束
        if (player.hp <= 0)
        {
            player.animator.SetTrigger("dead");
            RoundControl.currentGameState = RoundControl.GameState.Over;
            RoundControl.currentTurn = RoundControl.NowTurn.Player;
            player.ResetAttribute();
            enemy.ResetAttribute();
        }
        if (enemy.hp <= 0)
        {
            enemy.animator.SetTrigger("dead");
            RoundControl.currentGameState = RoundControl.GameState.Over;
            RoundControl.currentTurn = RoundControl.NowTurn.Player;
            player.ResetAttribute();
            enemy.ResetAttribute();
        }
    }

    //敌人回合的行动
    private void EnemyUnitAction()
    {
        enemy.animator.SetTrigger("attack");
        player.animator.SetTrigger("damage");
        int random = Random.Range(0, 9);
        player.ReceiveDamage(player.CauseDamage(random < 5 ? Unit.DamageType.Physics : Unit.DamageType.Magic));
        print("Player: " + player.hp + "\tEnemy: " + enemy.hp);
        currentTurn = NowTurn.Player;
        isWaitForPlayer = true;
    }

    //玩家回合行动
    private void PlayerUnitAction(int id)
    {
        //选择攻击方式
        if (GUI.Button(new Rect(25, 20, 100, 30), "Attack"))
        {
            player.animator.SetTrigger("attack");
            enemy.animator.SetTrigger("damage");
            enemy.ReceiveDamage(player.CauseDamage(Unit.DamageType.Physics));
            print("Player: " + player.hp + "\tEnemy: " + enemy.hp);
            StartCoroutine("PlayerOverDelay");
            
        }
        if (GUI.Button(new Rect(25, 60, 100, 30), "Magic"))
        {
            player.animator.SetTrigger("attack");
            enemy.animator.SetTrigger("damage");
            enemy.ReceiveDamage(player.CauseDamage(Unit.DamageType.Magic));
            print("Player: " + player.hp + "\tEnemy: " + enemy.hp);
            StartCoroutine("PlayerOverDelay");
        }
    }

    //玩家行动后延迟
    IEnumerator PlayerOverDelay()
    {
        isWaitForPlayer = false;
        yield return new WaitForSeconds(1);
        currentTurn = NowTurn.Enemy;
    }

    //重新开始
    private void CallRestart(int id)
    {
        if(GUI.Button(new Rect(50, 30, 100, 20), "Restart"))
        {
            currentGameState = GameState.Game;
            currentTurn = NowTurn.Player;
            isWaitForPlayer = true;
            enemy.animator.SetTrigger("wait");
            player.animator.SetTrigger("wait");
        }
    }

    //游戏菜单
    private void CallMenu(int id)
    {
        if (GUI.Button(new Rect(50, 30, 100, 20), "Fight"))
        {
            currentGameState = GameState.Game;
            isWaitForPlayer = true;
        }
    }
}
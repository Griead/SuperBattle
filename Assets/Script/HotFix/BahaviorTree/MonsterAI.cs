using System;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    private BehaviorNode rootNode;
    private Transform player;
    private Animator animator;
    private UnityEngine.AI.NavMeshAgent agent;

    [Header("AI参考")] 
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float patrolRadius = 5f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        BuildBehaviorTree();
    }

    private void Update()
    {
        rootNode.Evaluate();
    }

    void BuildBehaviorTree()
    {
        // // 构建行为树结构
        // rootNode = new SelectorNode();
        // {
        //     // 死亡检查
        //     var deathSequence = new SequenceNode();
        //     deathSequence.AddChild(new IsDeadCondition(this));
        //     deathSequence.AddChild(new DeathAction(this));
        //
        //     // 战斗序列器节点
        //     var combatSequence = new SequenceNode();
        //     {
        //         combatSequence.AddChild(new IsInCombatCondition(this));
        //
        //         // 战斗选择器节点
        //         var combatSelector = new SelectorNode();
        //         {
        //             // 攻击序列
        //             var attackSequence = new SequenceNode();
        //             attackSequence.AddChild(new IsInAttackRangeCondition(this, attackRange));
        //             attackSequence.AddChild(new AttackAction(this));
        //
        //             // 追击行为
        //             var chaseAction = new ChaseAction(this);
        //     
        //             // 加入战斗选择器中
        //             combatSelector.AddChild(attackSequence);
        //             combatSelector.AddChild(chaseAction);
        //         }
        //     
        //         combatSequence.AddChild(combatSelector);
        //     }
        //
        //
        //     // 巡逻行为
        //     var patrolAction = new PatrolAction(this, patrolRadius);
        //
        //     // 构建完整树结构
        //     ((SelectorNode)rootNode).AddChild(deathSequence);
        //     ((SelectorNode)rootNode).AddChild(combatSequence);
        //     ((SelectorNode)rootNode).AddChild(patrolAction);
        // }
    }
}
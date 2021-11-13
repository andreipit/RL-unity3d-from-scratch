using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
public class EntryPoint : MonoBehaviour
{
    MDP m_MDP;
    MapGenerator m_MapGen;
    AgentAction m_Agent;


    void Start()
    {
        m_MapGen = GameObject.Find("Map").GetComponent<MapGenerator>();
        m_Agent = GameObject.Find("Agent").GetComponent<AgentAction>();

        //TestNaive();
        TestFrozenLake();
    }

    void TestFrozenLake()
    {
        //*FFF
        //FHFH
        //FFFH
        //HFFG

        //string[,] map = m_MapGen.GenerateBakedMap();
        //m_MDP = FrozenLakeToMDP.BakedWithoutConvert(map);

        string[,] map = m_MapGen.GenerateRandomMap();
        m_MDP = FrozenLakeToMDP.Convert(map);

        m_Agent.Reset();

        // step1 action value
        //float ac_value = step1_ActionValue.get_action_value(m_MDP, null, "(3,2)", "right", 0.9f);
        //Debug.Log(ac_value);

        // --------------------------
        //step2 new state value
        //List<string> actions = m_MDP.get_possible_actions("(0,0)");
        //actions.ForEach(x => Debug.Log(x));

        // --------------------------
        // step3 value iteration
        Dictionary<string, float> new_state_values = step3_ValueIteration.value_iteration(
            mdp: m_MDP,
            state_values: null,
            gamma: 0.9f, // discount for MDP
            num_iter: 6, // maximum iterations, excluding initialization
            min_difference: 0.00001f, // stop VI if new values are this close to old values (or closer)
            debugger: false
            );
        //string debugger = "new states:";
        //foreach (var pair in new_state_values) debugger += " (" + pair.Key + ", " + pair.Value + "),";
        //Debug.Log(debugger); // correct: V(s0) = 3.780   V(s1) = 7.293   V(s2) = 4.201
        //Assert.AreApproximatelyEqual(new_state_values["s0"], 3.781f, tolerance: 0.1f);
        //Assert.AreApproximatelyEqual(new_state_values["s1"], 7.294f, tolerance: 0.1f);
        //Assert.AreApproximatelyEqual(new_state_values["s2"], 4.202f, tolerance: 0.1f);



        // --------------------------
        // step4 play
        StartCoroutine(PlayMode(new_state_values));
        Debug.Log(" step4 play ----------------------");
        


        //    s = mdp.reset()
        //mdp.render()
        //for t in range(100):
        //    a = get_optimal_action(mdp, state_values, s, gamma)
        //    print(a, end = '\n\n')
        //    s, r, done, _ = mdp.step(a)
        //    mdp.render()
        //    if done:
        //        break

    }

    IEnumerator PlayMode(Dictionary<string, float> new_state_values)
    {
        string s = "(0,0)";
        for (int t = 0; t < 10; t++)
        {
            string a = step4_OptimalAction.get_optimal_action(m_MDP, new_state_values, s, 0.9f);
            Debug.Log(s + a);
            s = m_MDP.step(s, a);
            yield return new WaitForSeconds(0.3f);
            m_Agent.Act(a);

            if (s == "(3,3)")
            {
                Debug.Log("Success!");
                break;
            }
        }
        yield return new WaitForSeconds(0.3f);
        TestFrozenLake(); // infinite loop
    }

    void TestNaive()
    {
        m_MDP = new MDP().Generate3StateMDPTest();
        var state_values = new Dictionary<string, float>() { { "s0", 0 }, { "s1", 1 }, { "s2", 2 } };

        // --------------------------
        // step1 action value
        float ac_value = step1_ActionValue.get_action_value(m_MDP, state_values, "s2", "a1", 0.9f);
        Debug.Log("ac_value=" + ac_value);

        // --------------------------
        // step2 new state value
        List<string> actions = m_MDP.get_possible_actions("s2");
        //actions.ForEach(x => Debug.Log(x));

        // --------------------------
        // step3 value iteration
        Dictionary<string, float> new_state_values = step3_ValueIteration.value_iteration(
            mdp: m_MDP,
            state_values: null,
            gamma: 0.9f, // discount for MDP
            num_iter: 100, // maximum iterations, excluding initialization
            min_difference: 0.001f // stop VI if new values are this close to old values (or closer)
            );
        string debugger = "new states:";
        foreach (var pair in new_state_values) debugger += " (" + pair.Key + ", " + pair.Value + "),";
        //Debug.Log(debugger); // correct: V(s0) = 3.780   V(s1) = 7.293   V(s2) = 4.201
        Assert.AreApproximatelyEqual(new_state_values["s0"], 3.781f, tolerance:0.1f);
        Assert.AreApproximatelyEqual(new_state_values["s1"], 7.294f, tolerance: 0.1f);
        Assert.AreApproximatelyEqual(new_state_values["s2"], 4.202f, tolerance: 0.1f);

        // --------------------------
        // step4 optimal action
        foreach (string s in new List<string>() { "s0", "s1", "s2"})
        {
            string optimal_action = step4_OptimalAction.get_optimal_action(m_MDP, new_state_values, s, 0.9f);
            //Debug.Log("in state" + s + " best a=" + optimal_action); // a1 must be
            if (s=="s0") Assert.AreEqual(optimal_action, "a1");
            if (s=="s1") Assert.AreEqual(optimal_action, "a0");
            if (s=="s2") Assert.AreEqual(optimal_action, "a1");
        }




        //for i in range(10):
        //    print("after iteration %i" % i)
        //    state_values = value_iteration(mdp, state_values, num_iter=1)
        //    draw_policy(mdp, state_values)
        //# please ignore iter 0 at each step
    }



    void Update()
    {
        
    }
}

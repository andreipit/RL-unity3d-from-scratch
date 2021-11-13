//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ActionValueTest : MonoBehaviour
//{
//    Dictionary<string, Dictionary<string, Dictionary<string, float>>> TransitionProbs;
//    Dictionary<string, Dictionary<string, Dictionary<string, float>>> Rewards;

//    public bool Test;

//    private void OnDrawGizmos()
//    {
//        if (Test)
//        {
//            Test = false;

//            TestActionValue();
//        }
//    }

//    void Start()
//    {
        
//    }

//    void TestActionValue()
//    {
//        /*
//        state_values = {'s0': 0, 's1': 1, 's2': 2}  # we use just indexes as state_values for test case
//        state = 's2'                                # start from this state
//        action = 'a1'                               # make this action, not others!
//        gamma = 0.9                                 # discounter (reduces reward each step, like inflation)
//        value = 0                                   # result accumulator
//        print('commit:', state, action)
//        for next_state, probability in mdp.get_next_states(state, action).items():
//          print('next_s:',next_state,', proba:', probability, ', rew:', mdp.get_reward(state, action, next_state), ', next_s_value:', state_values[next_state])
//          value += probability * (mdp.get_reward(state, action, next_state) + gamma * state_values[next_state])
//          print(probability, '* (', mdp.get_reward(state, action, next_state), ' + ', gamma, '*', state_values[next_state], ')=', value)
//        print('answer:',value)
//        */

//        TransitionProbs = MDP.GenerateTestProbas();
//        Rewards = MDP.GenerateTestRewards();

//        var state_values = new Dictionary<string, float>() { { "s0", 0 }, { "s1", 1 }, { "s2", 2 } };
//        string state = "s2";
//        string action = "a1";
//        float gamma = 0.9f;
//        float value = 0;
//        Debug.Log("commit:" + state + action);

//        foreach(var pair in get_next_states(state, action))
//        {
//            string next_state = pair.Item1;
//            float probability = pair.Item2;
//            float reward = get_reward(state, action, next_state);

//            Debug.Log("next_s:" + next_state + " proba:" + probability + " rew:" + reward + " next_s_value:" + state_values[next_state]);


//            value += probability * (reward + gamma * state_values[next_state]);
//            Debug.Log(probability + "* (" + reward + " + " + gamma + "*" + state_values[next_state] + ")=" + value);
//        }
//        Debug.Log("answer: " + value); // 0.69
//    }

//    /// <summary> Get next states and their probas </summary>
//    List<Tuple<string, float>> get_next_states(string _State, string _Action)
//    {
//        // example:
//        //'s2': {
//        //          'a0': { 's0': 0.4, 's2': 0.6},
//        //          'a1': { 's0': 0.3, 's1': 0.3, 's2': 0.4}
//        //}
//        // input: s2 a1, output: { 's0': 0.3, 's1': 0.3, 's2': 0.4}
//        //var res = new Dictionary<string, float>() { { "s0", 0.3f }, { "s1", 0.3f }, { "s2", 0.4f } };
//        //return res;

//        var result = new List<Tuple<string, float>>();
//        foreach(var stateActions in TransitionProbs)
//        {
//            if (stateActions.Key != _State) continue;
//            //stateActions  = 's0': {
//            //                          'a0': { 's0': 0.5, 's2': 0.5},
//            //                          'a1': { 's2': 1}
//            //                      },
//            foreach (var actionNextstates in stateActions.Value)
//            {
//                if (actionNextstates.Key != _Action) continue;
//                // actionNextstates = 'a0': { 's0': 0.5, 's2': 0.5}
//                foreach (var stateProba in actionNextstates.Value)
//                {
//                    // stateProba = 's0': 0.5
//                    result.Add(Tuple.Create(stateProba.Key, stateProba.Value));
//                }
//            }
//        }
//        return result;
//    }

//    /// <summary> Get reward for making action between 2 states </summary>
//    float get_reward(string _State, string _Action, string _NextState)
//    {
//        // correct answer:
//        //if (_State == "s2" && _Action == "a1" && _NextState == "s0") 
//        //    return -1;
//        //return 0;

//        //rewards = {
//        //    's1': { 'a0': { 's0': +5} },
//        //    's2': { 'a1': { 's0': -1} }
//        //}
//        foreach (var stateActions in Rewards)
//        {
//            if (stateActions.Key != _State) continue;
//            //stateActions  = 's1': { 'a0': { 's0': +5} },

//            foreach (var actionNextstates in stateActions.Value)
//            {
//                if (actionNextstates.Key != _Action) continue;
//                // actionNextstates = 'a0': { 's0': +5}
//                foreach (var stateProba in actionNextstates.Value)
//                {
//                    if (stateProba.Key != _NextState) continue;
//                    // stateProba = 's0': +5
//                    return stateProba.Value;
//                }
//            }
//        }
//        return 0; // default reward


//    }


//    //def get_action_value(mdp, state_values, state, action, gamma):
//    //""" Computes Q(s,a) as in formula above """

//    //value = 0
//    //for next_state, probability in mdp.get_next_states(state, action).items():
//    //  value += probability * (mdp.get_reward(state, action, next_state) + gamma * state_values[next_state])

//    //return value


//}



///*
// Driver metaphor: state=city, driver=action, each month go to 1 city, proba=director choice, action_value = average month salary
//I am driver (my city - s2, my name - a1)
//Each month director sends me to 1of3 cities: s0 s1 s2
//Sometimes he gives me money on road
//At finish I also get money, but not all (fraud city)
//Sometimes I spend my own money on road and get nothing at finish!
//My wife asks: what is your salary?
//Me: foreach city: road_money + finish_money
//if to s0, road=-1, finish=0, => BAD
//ie just spend my own on road, get nothing at finish

//if to s1, road=0, finish=0.9*1 => NICE
//ie just compensate my own on road, get prize at finish

//if to s2, road=0, finish=0.9*2 => PERFECT city
//ie just compensate my own on road, get BIG prize at finish

//But I don't know where director will send me, I need to find average:
//just weighted sum, weight = proba of going to city
// */
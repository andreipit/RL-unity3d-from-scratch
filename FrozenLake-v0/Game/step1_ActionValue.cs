// average salary per month

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class step1_ActionValue : MonoBehaviour
{

    public static float get_action_value(MDP _MDP, Dictionary<string,float> state_values, string state, string action, float gamma)
    {

        if (state_values == null || state_values.Count == 0) // make each city value = 0, if absent
            state_values = _MDP.GenerateZeroStateValues();

            float value = 0;
        //Debug.Log("commit:" + state + action);
        foreach (var pair in _MDP.get_next_states(state, action))
        {
            string next_state = pair.Item1;
            float probability = pair.Item2;
            float reward = _MDP.get_reward(state, action, next_state);
            //Debug.Log("next_s:" + next_state + " proba:" + probability + " rew:" + reward + " next_s_value:" + state_values[next_state]);
            value += probability * (reward + gamma * state_values[next_state]);
            //Debug.Log(probability + "* (" + reward + " + " + gamma + "*" + state_values[next_state] + ")=" + value);
        }
        //Debug.Log("answer: " + value); // 0.69
        return value;
    }
}
/*
 Driver metaphor: state=city, driver=action, each month go to 1 city, proba=director choice, action_value = average month salary
I am driver (my city - s2, my name - a1)
Each month director sends me to 1of3 cities: s0 s1 s2
Sometimes he gives me money on road
At finish I also get money, but not all (fraud city)
Sometimes I spend my own money on road and get nothing at finish!
My wife asks: what is your salary?
Me: foreach city: road_money + finish_money
if to s0, road=-1, finish=0, => BAD
ie just spend my own on road, get nothing at finish

if to s1, road=0, finish=0.9*1 => NICE
ie just compensate my own on road, get prize at finish

if to s2, road=0, finish=0.9*2 => PERFECT city
ie just compensate my own on road, get BIG prize at finish

But I don't know where director will send me, I need to find average:
just weighted sum, weight = proba of going to city
 */

/*
def get_action_value(mdp, state_values, state, action, gamma):
""" Computes Q(s,a) as in formula above """

value = 0
for next_state, probability in mdp.get_next_states(state, action).items():
  value += probability * (mdp.get_reward(state, action, next_state) + gamma * state_values[next_state])

return value
*/
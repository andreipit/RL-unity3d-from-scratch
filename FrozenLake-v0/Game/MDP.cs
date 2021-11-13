// MDP is markov decision process
// our main algo is written for graph
// so we must reformulate frozen-lake-v0 as MDP, ie convert it to graph

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MDP
{
    public Dictionary<string, Dictionary<string, Dictionary<string, float>>> TransitionProbs;
    public Dictionary<string, Dictionary<string, Dictionary<string, float>>> Rewards;


    #region Public methods

    public string step(string state, string action)
    {
        int r = System.Convert.ToInt32(state.Substring(1, 1));
        int c = System.Convert.ToInt32(state.Substring(3, 1));

        switch(action)
        {
            case "left": return "(" + (r).ToString() + "," + (c - 1).ToString() + ")";
            case "down": return "(" + (r+1).ToString() + "," + (c).ToString() + ")";
            case "right": return "(" + (r).ToString() + "," + (c + 1).ToString() + ")";
            case "up": return "(" + (r-1).ToString() + "," + (c).ToString() + ")";
        }
        return "error";
    }

    //public string Reset()
    //{
    //    return TransitionProbs;
    //}

    /// <summary> Get next states and their probas </summary>
    public List<Tuple<string, float>> get_next_states(string _State, string _Action)
    {
        var result = new List<Tuple<string, float>>();
        foreach (var stateActions in TransitionProbs)
        {
            if (stateActions.Key != _State) continue;
            //stateActions  = 's0': {
            //                          'a0': { 's0': 0.5, 's2': 0.5},
            //                          'a1': { 's2': 1}
            //                      },
            foreach (var actionNextstates in stateActions.Value) // actionNextstates = 'a0': { 's0': 0.5, 's2': 0.5}
            {
                if (actionNextstates.Key != _Action) continue;

                foreach (var stateProba in actionNextstates.Value) // stateProba = 's0': 0.5
                {

                    result.Add(Tuple.Create(stateProba.Key, stateProba.Value));
                }
            }
        }
        return result;
    }

    /// <summary> Get reward for making action between 2 states </summary>
    public float get_reward(string _State, string _Action, string _NextState)
    {
        foreach (var stateActions in Rewards) //stateActions  = 's1': { 'a0': { 's0': +5} },
        {
            if (stateActions.Key != _State) continue;
            foreach (var actionNextstates in stateActions.Value) // actionNextstates = 'a0': { 's0': +5}
            {
                if (actionNextstates.Key != _Action) continue;
                foreach (var stateProba in actionNextstates.Value)// stateProba = 's0': +5
                {
                    if (stateProba.Key != _NextState) continue;
                    return stateProba.Value;
                }
            }
        }
        return 0; // default reward
    }

    /// <summary> get list of all actions </summary>
    public List<string> get_possible_actions(string _State)
    {
        var result = new List<string>();
        foreach (var stateActions in TransitionProbs)
        {
            if (stateActions.Key != _State) continue;
            //stateActions  = 's0': {
            //                          'a0': { 's0': 0.5, 's2': 0.5},
            //                          'a1': { 's2': 1}
            //                      },
            foreach (var actionNextstates in stateActions.Value) // actionNextstates = 'a0': { 's0': 0.5, 's2': 0.5}
            {
                result.Add(actionNextstates.Key);
            }
        }
        return result;
    }

    /// <summary> get list of all states </summary>
    public List<string> get_all_states()
    {
        var result = new List<string>();
        foreach (var stateActions in TransitionProbs)
        {
            result.Add(stateActions.Key);
        }
        return result;
    }

    public MDP Generate3StateMDPTest()
    {
        var mdp = new MDP();
        mdp.TransitionProbs = GenerateTestProbas();
        mdp.Rewards = GenerateTestRewards();
        return mdp;
    }

    public Dictionary<string, float> GenerateZeroStateValues()
    {
        var state_values = new Dictionary<string, float>();
        foreach (string s in get_all_states()) state_values[s] = 0;
        return state_values;
    }

    #endregion


    #region Private methods

    Dictionary<string, Dictionary<string, Dictionary<string, float>>> GenerateTestRewards()
    {
        //rewards = {
        //    's1': { 'a0': { 's0': +5} },
        //    's2': { 'a1': { 's0': -1} }
        //}
        return new Dictionary<string, Dictionary<string, Dictionary<string, float>>>()
        {
            {"s1", new Dictionary<string, Dictionary<string, float>>(){
                { "a0", new Dictionary<string, float>(){ {"s0", 5f } } },
            }},
            {"s2", new Dictionary<string, Dictionary<string, float>>(){
                { "a1", new Dictionary<string, float>(){ {"s0", -1f } }}
            }}
        };
    }

    Dictionary<string, Dictionary<string, Dictionary<string, float>>> GenerateTestProbas()
    {
        /*
        transition_probs = {
            's0': {
                'a0': {'s0': 0.5, 's2': 0.5},
                'a1': {'s2': 1}
            },
            's1': {
                'a0': {'s0': 0.7, 's1': 0.1, 's2': 0.2},
                'a1': {'s1': 0.95, 's2': 0.05}
            },
            's2': {
                'a0': {'s0': 0.4, 's2': 0.6},
                'a1': {'s0': 0.3, 's1': 0.3, 's2': 0.4}
            }
        }
        */
        return new Dictionary<string, Dictionary<string, Dictionary<string, float>>>()
        {
            {"s0", new Dictionary<string, Dictionary<string, float>>(){
                { "a0", new Dictionary<string, float>(){ {"s0", 0.5f },{"s2", 0.5f } } },
                { "a1", new Dictionary<string, float>(){ {"s2", 1f } }}
            }},
            {"s1", new Dictionary<string, Dictionary<string, float>>(){
                { "a0", new Dictionary<string, float>(){ {"s0", 0.7f },{"s1", 0.1f }, { "s2", 0.2f} } },
                { "a1", new Dictionary<string, float>(){ {"s1", 0.95f }, {"s2", 0.05f } }}
            }},
            {"s2", new Dictionary<string, Dictionary<string, float>>(){
                { "a0", new Dictionary<string, float>(){ {"s0", 0.4f }, {"s2", 0.6f } } },
                { "a1", new Dictionary<string, float>(){ {"s0", 0.3f }, {"s1", 0.3f }, {"s2", 0.4f } }}
            }}
        };

    }

    #endregion
}

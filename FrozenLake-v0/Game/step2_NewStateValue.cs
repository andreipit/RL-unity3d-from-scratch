// mean salary of the RICHEST driver in city

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class step2_NewStateValue : MonoBehaviour
{

    /// <summary>
    /// !!!mean salary of the RICHEST driver in city!!!
    /// * state = city, action = driver, 
    ///    ** next states = cities where this driver can go
    ///    ** probas = director choice, where to go
    /// * action_value = mean salary of ONE driver in city
    /// * new_state_value = mean salary of the RICHEST driver in city
    /// </summary>
    /// <param name="mdp"> graph (states and actions are nodes) </param>
    /// <param name="state_values"> float assigned to each state </param>
    /// <param name="state"> wanna find Richest salary in this city </param>
    /// <param name="gamma"> discount factor </param>
    /// <returns></returns>
    public static float get_new_state_value(MDP mdp, Dictionary<string, float> state_values, string state,  float gamma)
    {
        List<string> possible_actions = mdp.get_possible_actions(state);
        if (possible_actions.Count == 0)
            return 0;

        List<float> qvalues_for_actions = new List<float>();
        foreach(string action in possible_actions)
            qvalues_for_actions.Add(step1_ActionValue.get_action_value(mdp, state_values, state, action, gamma));
        return qvalues_for_actions.Max();
    }
}

/*
def get_new_state_value(mdp, state_values, state, gamma):
    """ Computes next V(s) as in formula above. Please do not change state_values in process. """
    if mdp.is_terminal(state): # terminal means "end"
        return 0

    possible_actions = mdp.get_possible_actions(state)
    qvalues_for_actions = [get_action_value(mdp, state_values, state, action, gamma) for action in possible_actions]
    
    return max(qvalues_for_actions)
*/

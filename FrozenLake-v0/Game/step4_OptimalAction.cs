using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class step4_OptimalAction : MonoBehaviour
{

    public static string get_optimal_action(MDP mdp, Dictionary<string,float> state_values, string state, float gamma = 0.9f)
    {
        bool DebugMode = false;
        // 1) in city "state" we have 2 drivers: a0 and a1
        List<string> possible_actions = mdp.get_possible_actions(state);

        // 2) find average salary of each driver: {driver1: 100$, driver2: 200$}
        var values_for_actions = new Dictionary<string, float>();
        foreach (string a in possible_actions)
        {
            values_for_actions[a] = step1_ActionValue.get_action_value(mdp, state_values, state, a, gamma);
            if (DebugMode) Debug.Log("in" + state + " " + a + " has value=" + values_for_actions[a]);
        }

        // 3) find richest driver in city "state"
        string richest_driver = possible_actions[0];
        float max_salary = values_for_actions[richest_driver];
        foreach(var pair in values_for_actions)
        {
            if (pair.Value > max_salary)
            { 
                richest_driver = pair.Key;
                max_salary = pair.Value;
            }
        }

        return richest_driver;
    }

}

//def get_optimal_action(mdp, state_values, state, gamma= 0.9):
//    """ Finds optimal action using formula above. """
//    if mdp.is_terminal(state):
//        return None

//    possible_actions = mdp.get_possible_actions(state)
//    values_for_actions = { a: get_action_value(mdp, state_values, state, a, gamma) for a in possible_actions}

//return max(values_for_actions, key = values_for_actions.get)


/*
 # Interpretation of get_optimal_action(city) = richest driver in city
## taxi: make proba of best action higher
## here: we always select concrete action, but action result is not determinated (we can choose "left", but go "fwd")
* foreach city we calculated value (payment to arrived drivers, also equals to mean salary of richest driver in this city)
* now we know how much money each driver earns going to ANY city
* so we can find mean salary and select richest guy in each city
 */
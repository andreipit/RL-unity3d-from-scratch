// 100 graph shakes

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class step3_ValueIteration : MonoBehaviour
{
    /// <summary>performs num_iter value iteration steps starting from state_values. Same as before but in a function  </summary>
    public static Dictionary<string, float> value_iteration(MDP mdp, Dictionary<string, float> state_values = null, float gamma = 0.9f, int num_iter = 1000, float min_difference = 0.00001f, bool debugger = false)
    {
        if (state_values == null || state_values.Count == 0) // make each city value = 0, if absent
        {
            state_values = mdp.GenerateZeroStateValues();
            //state_values = new Dictionary<string, float>();
            //foreach(string s in mdp.get_all_states()) state_values[s] = 0;
        }

        for (int i = 0; i < num_iter; i++)
        {
            // 1) Fix cities payments to arrived. Send from each city ALL drivers.
            // Find richesh in each city.
            // At init assume, that driver will earn money only for road, and no money for arriving.
            var new_state_values = new Dictionary<string, float>();
            foreach (string state in mdp.get_all_states())
            {
                /// mean salary of the RICHEST driver in city!!! At start they just get money on road, no money at finish
                new_state_values[state] = step2_NewStateValue.get_new_state_value(mdp, state_values, state, gamma);
            }

            // 2) Find how much each city changed it's value. Select max change.
            List<float> diffs = new List<float>();
            foreach(var pair in state_values) diffs.Add((float)Math.Abs(pair.Value - new_state_values[pair.Key]));
            float diff = diffs.Max();

            // 3) update each city value
            state_values = new_state_values;

            if (debugger)
            {
                // test
                var map = GameObject.Find("Map").GetComponent<MapGenerator>().GenerateBakedMap();
                var m_MDP = FrozenLakeToMDP.BakedWithoutConvert(map);
                //Debug.Log(i + " " + step4_OptimalAction.get_optimal_action(m_MDP, state_values, "(0,1)", 0.9f));

                var a00 = step4_OptimalAction.get_optimal_action(m_MDP, state_values, "(0,0)", 0.9f);
                var a01 = step4_OptimalAction.get_optimal_action(m_MDP, state_values, "(0,1)", 0.9f);
                var a10 = step4_OptimalAction.get_optimal_action(m_MDP, state_values, "(1,0)", 0.9f);

                if ((a00 == "right") && (a01 == "right")) Debug.Log("best index:" + i);
                if ((a00 == "down") && (a10 == "down")) Debug.Log("best index:" + i);


                Debug.Log("iter:" + i + " diff:" + diff + " new value of init state" + new_state_values[mdp.get_all_states()[0]]);
                PrintStateValues(state_values);
            }
            // 4) check if precision is enough
            if (diff < min_difference) break;
        }
        return state_values;
    }

    public static void PrintStateValues(Dictionary<string, float> new_state_values)
    {
        string row0 = "";
        string row1 = "";
        string row2 = "";
        string row3 = "";
        int id = -1;
        foreach (var pair in new_state_values)
        {
            id++;
            if (id < 4) row0 += " (" + pair.Value + "),";
            else if (id < 8) row1 += " (" + pair.Value + "),";
            else if (id < 12) row2 += " (" + pair.Value + "),";
            else if (id < 16) row3 += " (" + pair.Value + "),";
        }
        Debug.Log("-------------------");
        Debug.Log(row0);
        Debug.Log(row1);
        Debug.Log(row2);
        Debug.Log(row3);
        Debug.Log("-------------------");
    }

}

/*
def value_iteration(mdp, state_values=None, gamma=0.9, num_iter=1000, min_difference=1e-5):
    """ performs num_iter value iteration steps starting from state_values. Same as before but in a function """
    state_values = state_values or {s: 0 for s in mdp.get_all_states()}
    for i in range(num_iter):

        # Compute new state values using the functions you defined above. It must be a dict {state : new_V(state)}
        new_state_values = {state: get_new_state_value(mdp, state_values, state, gamma) for state in mdp.get_all_states()}

        assert isinstance(new_state_values, dict)

        # Compute difference
        diff = max(abs(new_state_values[s] - state_values[s])
                   for s in mdp.get_all_states())
        
        print("iter %4i   |   diff: %6.5f   |   Value of start state: %.3f " %
              (i, diff, new_state_values[mdp._initial_state])) # init = (0,0)

        state_values = new_state_values
        if diff < min_difference:
            break

    return state_values
*/

/*
Finally, let's combine everything we wrote into a working value iteration algo.

Interpretation
    city pays BETTER at finish if it's richest driver mean salary is BETTER
    city good/bad called "state_value"
    salary good/bad called "action_value"
    but salary depends on other cities payments (values)
    so, it is loop: city1_value -> driver1_salary -> city2_value -> driver2_salary -> city1_value
    repeat it num_iter times

Algo:
    all old_state_values = 0 # like "cities_values"
    new_state_values = in each city find richest driver (and his mean salary)
    find difference: ex: city1 value old=0, new=-15, diff=15
    get city with max difference
    check, how it is big
    if smaller then threshold (min_difference = 0.001), then break loop
    intuition: if our fun doesn't change cities values a lot, we are done, it is enough precision, salary and city_payments are balanced
    like shake (sift) flovour for 3 sizes
*/

/*
# parameters
gamma = 0.9            # discount for MDP
num_iter = 100         # maximum iterations, excluding initialization
# stop VI if new values are this close to old values (or closer)
min_difference = 0.001

# initialize V(s)
state_values = {s: 0 for s in mdp.get_all_states()} # make all state values zero

if has_graphviz:
    display(plot_graph_with_state_values(mdp, state_values))

for i in range(num_iter):

    # Compute new state values using the functions you defined above.
    # It must be a dict {state : float V_new(state)}
    new_state_values = {state: get_new_state_value(mdp, state_values, state, gamma)
      for state in mdp.get_all_states()}

    assert isinstance(new_state_values, dict)

    # Compute difference
    diff = max(abs(new_state_values[s] - state_values[s])
               for s in mdp.get_all_states())
    print("iter %4i   |   diff: %6.5f   |   " % (i, diff), end="")
    print('   '.join("V(%s) = %.3f" % (s, v) for s, v in state_values.items()))
    state_values = new_state_values

    if diff < min_difference:
        print("Terminated")
        break
*/
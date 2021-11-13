// convert frozen lake to graph

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenLakeToMDP : MonoBehaviour
{
    //*FFF
    //FHFH
    //FFFH
    //HFFG

    //_LakeMap = new string[,] { 
    //    { "S", "F", "F", "F" }, 
    //    { "F", "H", "F", "H" }, 
    //    { "F", "F", "F", "H" }, 
    //    { "H", "F", "F", "G" } 
    //};

    public static MDP Convert(string[,] _Map)
    {
        MDP res = new MDP();
        var tProbs = new Dictionary<string, Dictionary<string, Dictionary<string, float>>>();

        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                string state = "(" + row + "," + col + ")";
                tProbs[state] = new Dictionary<string, Dictionary<string, float>>();
                if (_Map[row, col] == "F" || _Map[row, col] == "S" )
                {
                    string left = (col - 1 > -1) ? "(" + (row) + "," + (col - 1) + ")" : state;
                    string down = (row + 1 < 4) ? "(" + (row+1) + "," + (col) + ")" : state;
                    string right= (col + 1 < 4) ? "(" + (row) + "," + (col + 1) + ")" : state;
                    string up= (row - 1 > -1) ? "(" + (row-1) + "," + (col) + ")" : state;

                    tProbs[state]["left"] = new Dictionary<string, float>() { { left, 1f } };
                    tProbs[state]["down"] = new Dictionary<string, float>() { { down, 1f } };
                    tProbs[state]["right"] = new Dictionary<string, float>() { { right, 1f } };
                    tProbs[state]["up"] = new Dictionary<string, float>() { { up, 1f } };
                }
                //if (row == 0 && col == 0 || row == 3 && col == 3)
                //    continue;
            }
        }
        res.TransitionProbs = tProbs;
        res.Rewards = GetRewards(_Map[3,2], _Map[2,3]);
        return res;
    }


    public static MDP BakedWithoutConvert(string[,] _LakeMap)
    {
        MDP res = new MDP();
        res.TransitionProbs = new Dictionary<string, Dictionary<string, Dictionary<string, float>>>()
        {
            {"(0,0)", new Dictionary<string, Dictionary<string, float>>(){
                { "left", new Dictionary<string, float>(){ {"(0,0)", 1f } } },
                { "down", new Dictionary<string, float>(){ {"(1,0)", 1f } } },
                { "right", new Dictionary<string, float>(){ {"(0,1)", 1f } } },
                { "up", new Dictionary<string, float>(){ {"(0,0)", 1f } } },
            }},
            {"(0,1)", new Dictionary<string, Dictionary<string, float>>(){
                { "left", new Dictionary<string, float>(){ {"(0,0)", 1f } } },
                { "down", new Dictionary<string, float>(){ {"(1,1)", 1f } } },
                { "right", new Dictionary<string, float>(){ {"(0,2)", 1f } } },
                { "up", new Dictionary<string, float>(){ {"(0,1)", 1f } } },
            }},
            {"(0,2)", new Dictionary<string, Dictionary<string, float>>(){
                { "left", new Dictionary<string, float>(){ {"(0,1)", 1f } } },
                { "down", new Dictionary<string, float>(){ {"(1,2)", 1f } } },
                { "right", new Dictionary<string, float>(){ {"(0,3)", 1f } } },
                { "up", new Dictionary<string, float>(){ {"(0,2)", 1f } } },
            }},
            {"(0,3)", new Dictionary<string, Dictionary<string, float>>(){
                { "left", new Dictionary<string, float>(){ {"(0,2)", 1f } } },
                { "down", new Dictionary<string, float>(){ {"(1,3)", 1f } } },
                { "right", new Dictionary<string, float>(){ {"(0,3)", 1f } } },
                { "up", new Dictionary<string, float>(){ {"(0,3)", 1f } } },
            }},

            {"(1,0)", new Dictionary<string, Dictionary<string, float>>(){
                { "left", new Dictionary<string, float>(){ {"(1,0)", 1f } } },
                { "down", new Dictionary<string, float>(){ {"(2,0)", 1f } } },
                { "right", new Dictionary<string, float>(){ {"(1,1)", 1f } } },
                { "up", new Dictionary<string, float>(){ {"(0,0)", 1f } } },
            }},
            {"(1,1)", new Dictionary<string, Dictionary<string, float>>()},
            {"(1,2)", new Dictionary<string, Dictionary<string, float>>(){
                { "left", new Dictionary<string, float>(){ {"(1,1)", 1f } } },
                { "down", new Dictionary<string, float>(){ {"(2,2)", 1f } } },
                { "right", new Dictionary<string, float>(){ {"(1,3)", 1f } } },
                { "up", new Dictionary<string, float>(){ {"(0,2)", 1f } } },
            }},
            {"(1,3)", new Dictionary<string, Dictionary<string, float>>()},

            {"(2,0)", new Dictionary<string, Dictionary<string, float>>(){
                { "left", new Dictionary<string, float>(){ {"(2,0)", 1f } } },
                { "down", new Dictionary<string, float>(){ {"(3,0)", 1f } } },
                { "right", new Dictionary<string, float>(){ {"(2,1)", 1f } } },
                { "up", new Dictionary<string, float>(){ {"(1,0)", 1f } } },
            }},
            {"(2,1)", new Dictionary<string, Dictionary<string, float>>(){
                { "left", new Dictionary<string, float>(){ {"(2,0)", 1f } } },
                { "down", new Dictionary<string, float>(){ {"(3,1)", 1f } } },
                { "right", new Dictionary<string, float>(){ {"(2,2)", 1f } } },
                { "up", new Dictionary<string, float>(){ {"(1,1)", 1f } } },
            }},
            {"(2,2)", new Dictionary<string, Dictionary<string, float>>(){
                { "left", new Dictionary<string, float>(){ {"(2,1)", 1f } } },
                { "down", new Dictionary<string, float>(){ {"(3,2)", 1f } } },
                { "right", new Dictionary<string, float>(){ {"(2,3)", 1f } } },
                { "up", new Dictionary<string, float>(){ {"(1,2)", 1f } } },
            }},
            {"(2,3)", new Dictionary<string, Dictionary<string, float>>()},

            {"(3,0)", new Dictionary<string, Dictionary<string, float>>()},
            {"(3,1)", new Dictionary<string, Dictionary<string, float>>(){
                { "left", new Dictionary<string, float>(){ {"(3,0)", 1f } } },
                { "down", new Dictionary<string, float>(){ {"(3,1)", 1f } } },
                { "right", new Dictionary<string, float>(){ {"(3,2)", 1f } } },
                { "up", new Dictionary<string, float>(){ {"(2,1)", 1f } } },
            }},
            {"(3,2)", new Dictionary<string, Dictionary<string, float>>(){
                { "left", new Dictionary<string, float>(){ {"(3,1)", 1f } } },
                { "down", new Dictionary<string, float>(){ {"(3,2)", 1f } } },
                { "right", new Dictionary<string, float>(){ {"(3,3)", 1f } } },
                { "up", new Dictionary<string, float>(){ {"(2,2)", 1f } } },
            }},
            {"(3,3)", new Dictionary<string, Dictionary<string, float>>()},


        };
        res.Rewards = GetRewards(_LakeMap[3,2], _LakeMap[2,3]);
        return res;
    }


    /// <summary>
    /// All maps have the same rewards (all 0 except last step). Only from 3,2 and 2,3 we can get 1.
    /// </summary>
    /// <returns></returns>
    static Dictionary<string, Dictionary<string, Dictionary<string, float>>> GetRewards(string _Place32, string _Place23)
    {

        var zeroRewards = new Dictionary<string, float>(){
            {"(0,0)", 0f },
            {"(0,1)", 0f },
            {"(0,2)", 0f },
            {"(0,3)", 0f },
            {"(1,0)", 0f },
            {"(1,1)", 0f },
            {"(1,2)", 0f },
            {"(1,3)", 0f },
            {"(2,0)", 0f },
            {"(2,1)", 0f },
            {"(2,2)", 0f },
            {"(2,3)", 0f },
            {"(3,0)", 0f },
            {"(3,1)", 0f },
            {"(3,2)", 0f },
            {"(3,3)", 0f },// only here we can win from (3,2)+right or (2,3)+down !!!
        };

        var zeroActionsRewards = new Dictionary<string, Dictionary<string, float>>(){
                { "left", zeroRewards },
                { "down", zeroRewards },
                { "right", zeroRewards },
                { "up", zeroRewards },
        };

        var rewards = new Dictionary<string, Dictionary<string, Dictionary<string, float>>>()
        {
            {"(0,0)", zeroActionsRewards},
            {"(0,1)", zeroActionsRewards},
            {"(0,2)", zeroActionsRewards},
            {"(0,3)", zeroActionsRewards},
            {"(1,0)", zeroActionsRewards},
            {"(1,1)", zeroActionsRewards},
            {"(1,2)", zeroActionsRewards},
            {"(1,3)", zeroActionsRewards},
            {"(2,0)", zeroActionsRewards},
            {"(2,1)", zeroActionsRewards},
            {"(2,2)", zeroActionsRewards},
            {"(2,3)", zeroActionsRewards},
            {"(3,0)", zeroActionsRewards},
            {"(3,1)", zeroActionsRewards},
            {"(3,2)", zeroActionsRewards},
            {"(3,3)", zeroActionsRewards}
        };
        if (_Place32 == "F") rewards["(3,2)"]["right"]["(3,3)"] = 1;
        if (_Place23 == "F") rewards["(2,3)"]["right"]["(3,3)"] = 1;
        return rewards;
    }

}


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
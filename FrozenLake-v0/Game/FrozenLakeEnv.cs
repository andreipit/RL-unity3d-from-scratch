﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenLakeEnv : MonoBehaviour
{
    /*
    Winter is here.You and your friends were tossing around a frisbee at the
    park when you made a wild throw that left the frisbee out in the middle of
    the lake.The water is mostly frozen, but there are a few holes where the
    ice has melted.If you step into one of those holes, you'll fall into the
    freezing water. At this time, there's an international frisbee shortage, so
    it's absolutely imperative that you navigate across the lake and retrieve
    the disc. However, the ice is slippery, so you won't always move in the
    direction you intend.
    The surface is described using a grid like the following
        SFFF
        FHFH
        FFFH
        HFFG
    S : starting point, safe
    F : frozen surface, safe
    H : hole, fall to your doom
    G : goal, where the frisbee is located
    The episode ends when you reach the goal or fall in a hole.
    You receive a reward of 1 if you reach the goal, and zero otherwise.
    */

    void Init(string desc= "None", string map_name= "4x4", bool is_slippery= true)
    {
        void inc(int row, int col, int a)
        {

        }

        void update_probability_matrix(int row, int col, int action)
        {

        }
    }

    void Render(string mode = "human")
    {
        
    }
}

using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public List<Transform> Frozens;

    public bool DebugGenerate;


    void Update()
    {
        if (DebugGenerate)
        {
            DebugGenerate = false;
            GenerateRandomMap();
        }
    }

    public Transform GetFrozen(int _Row, int _Col)
    {
        return Frozens.Where(x => x.name.ToString().Split(new string[]{"_"}, System.StringSplitOptions.None)[1]==(_Row.ToString() + _Col.ToString())).FirstOrDefault();
    }


    public string[,] GenerateBakedMap()
    {
        var res =  new string[,] {
            { "S", "F", "F", "F" },
            { "F", "H", "F", "H" },
            { "F", "F", "F", "H" },
            { "H", "F", "F", "G" }
        };
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (row == 0 && col == 0 || row == 3 && col == 3)
                    continue;
                GetFrozen(row, col).GetComponent<MeshRenderer>().enabled = (res[row, col] == "F");
            }
        }
        return res;
    }

    /// <summary>
    /// Generates a random valid map (one that has a path from start to goal)
    /// </summary>
    /// <param name="_Size"> size of each side of the grid </param>
    /// <param name="_P">  probability that a tile is frozen </param>
    public string[,] GenerateRandomMap(int _Size = 4, float _P = 0.8f)
    {
        bool valid = false;
        string[,] res = new string[4,4];
        //for (int i = 0; i < 1; i++)
        while (!valid)
        {
            var ran = new System.Random();

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    int enabled = ran.Next(0, 2); // 0 or 1

                    res[row,col] = (enabled == 1) ? "F" : "H";
                }
            }
            //res = new string[,] { 
            //    { "F", "F", "F", "H" }, 
            //    { "H", "H", "F", "H" }, 
            //    { "F", "H", "F", "H" }, 
            //    { "F", "H", "F", "H" } 
            //};

            res[0,0] = "S";
            res[3,3] = "G";
            valid = is_valid(res);
            //Debug.Log("valid=" + valid);
            if (valid) 
                break;
        }

        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (row == 0 && col == 0 || row == 3 && col == 3)
                    continue;
                GetFrozen(row, col).GetComponent<MeshRenderer>().enabled = (res[row,col] == "F");
            }
        }
        return res;
    }

    /// <summary>
    /// Check if we can go from start to goal
    /// </summary>
    /// <param name="_Res"> new string[,] { { "F", "H", "F", "H" }, {..}, {..}, {..} }; </param>
    /// <param name="_Size"> rows and cols count </param>
    /// <returns></returns>
    bool is_valid(string[,] _Res, int _Size = 4)
    {
        //foreach (var pair in _Res) Debug.Log(pair);
        //return true;

        List<List<int>> frontier = new List<List<int>>();
        List<List<int>> discovered = new List<List<int>>();
        frontier.Add(new List<int>() { 0, 0 });
        //for (int i = 0; i < 10; i++)
        while (frontier.Count > 0)
        {
            var last = frontier[frontier.Count - 1];
            frontier.RemoveAt(frontier.Count - 1);

            int r = last[0]; // row
            int c = last[1]; // col

            bool discoveredContains = false;
            foreach (var pair in discovered) if (pair[0] == r && pair[1] == c) discoveredContains = true;
            //if (!discovered.Contains(new List<int>() { r, c })) ---> BIG ERROR!!!
            if (!discoveredContains)
            {
                discovered.Add(new List<int>() { r, c });
                var directions = new List<List<int>>() { new List<int>() { 1, 0 }, new List<int>() { 0, 1 }, new List<int>() { -1, 0 }, new List<int>() { 0, -1 } };
                foreach(var pair in directions)
                {
                    int x = pair[0];
                    int y = pair[1];

                    int r_new = r + x;
                    int c_new = c + y;
                    //Debug.Log("r_new=" + r_new + " c_new=" + c_new);

                    if (r_new < 0 || r_new >= _Size || c_new < 0 || c_new >= _Size) // we go out of bounds
                        continue;
                    if (_Res[r_new, c_new] == "G") // we are at finish
                        return true;
                    if (_Res[r_new, c_new] != "H") // not hole, ie frozen or start
                        frontier.Add(new List<int>() { r_new, c_new }); // go to it and go further
                }
            }

            //string result = frontier.Count + " items: ";
            //foreach (var item in frontier) result += "(" + item[0].ToString() + ", " + item[1].ToString() + "), ";
            //Debug.Log("frontier=" + result);
            if (frontier.Count == 0)
                break;
        }
        return false;
    }

}


//def generate_random_map(size=8, p=0.8):
//    """Generates a random valid map (one that has a path from start to goal)
//    :param size: size of each side of the grid
//    :param p: probability that a tile is frozen
//    """
//    valid = False

//    # DFS to check that it's a valid path.
//    def is_valid(res):
//        frontier, discovered = [], set()
//        frontier.append((0, 0))
//        while frontier:
//            r, c = frontier.pop()
//            if not (r, c) in discovered:
//                discovered.add((r, c))
//                directions = [(1, 0), (0, 1), (-1, 0), (0, -1)]
//                for x, y in directions:
//                    r_new = r + x
//                    c_new = c + y
//                    if r_new < 0 or r_new >= size or c_new < 0 or c_new >= size:
//                        continue
//                    if res[r_new][c_new] == "G":
//                        return True
//                    if res[r_new][c_new] != "H":
//                        frontier.append((r_new, c_new))
//        return False

//    while not valid:
//        p = min(1, p)
//        res = np.random.choice(["F", "H"], (size, size), p=[p, 1 - p])
//        res[0][0] = "S"
//        res[-1][-1] = "G"
//        valid = is_valid(res)
//    return ["".join(x) for x in res]

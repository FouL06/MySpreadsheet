///<summary>
/// Author: Ashton Foulger, CS 3500 - 001 Fall 2021
/// 
/// Version : 0.1 : Sep 4th 2021
/// 
///</summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// DependecyGraph makes use of nodes to which dependents or dependees can be written for each cell in the spreadsheet.
    /// With the primary purpose of keeping track of cells is dependents and dependees before a cells expression can be evaluated.
    /// This is so that the cells can grab nessesary data from those dependents or dependess when called upon.
    /// </summary>
    public class DependencyGraph
    {
        //Private Data Structures
        private Dictionary<string, HashSet<string>> dependents = new Dictionary<string, HashSet<string>>();
        private Dictionary<string, HashSet<string>> dependees = new Dictionary<string, HashSet<string>>();
        private int size = 0;

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
        }

        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return size; }
        }

        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get 
            {
                if (dependees.ContainsKey(s))
                {
                    return dependees[s].Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (dependents.ContainsKey(s))
            {
                return (dependents[s].Count != 0);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (dependees.ContainsKey(s))
            {
                return (dependees[s].Count != 0);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (dependents.ContainsKey(s))
            {
                return new HashSet<string>(dependents[s]);
            }
            else
            {
                return new HashSet<string>();
            }
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (dependees.ContainsKey(s))
            {
                return new HashSet<string>(dependees[s]);
            }
            else
            {
                return new HashSet<string>();
            }
        }

        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            //Add Dependent if it does not exist
            if (!dependents.ContainsKey(s))
            {
                HashSet<string> temp = new HashSet<string>();
                temp.Add(t);
                dependents.Add(s, temp);
                size++;
            }
            else
            {
                //Add data to already existing dependent
                if (dependents[s].Add(t))
                {
                    size++;
                }
            }

            //Add Dependee if it does not exist
            if (!dependees.ContainsKey(t))
            {
                HashSet<string> temp = new HashSet<string>();
                temp.Add(s);
                dependees.Add(t, temp);
            }
            else
            {
                //Add data to already existing dependee
                dependees[t].Add(s);
            }
        }

        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            //Remove existing Dependent
            if (dependents.ContainsKey(s))
            {
                //Remove and decrement size
                if (dependents[s].Contains(t))
                {
                    dependents[s].Remove(t);
                    size--;
                }
            }

            //Remove existing Dependee
            if (dependees.ContainsKey(t))
            {
                if (dependees[t].Contains(s))
                {
                    dependees[t].Remove(s);
                }
            }
        }

        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            //If the key exists remove all dependents from list
            if (dependents.ContainsKey(s))
            {
                foreach(string d in dependents[s].ToList())
                {
                    RemoveDependency(s, d);
                }
            }

            //Replace the removed list with the new dependents
            foreach (string t in newDependents)
            {
                AddDependency(s, t);
            }
        }

        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            //If the key exists remove all dependees from list
            if (dependees.ContainsKey(s))
            {
                foreach (string d in dependees[s].ToList())
                {
                    RemoveDependency(d, s);
                }
            }

            //Replace the removed list with the new dependees
            foreach (string t in newDependees)
            {
                AddDependency(t, s);
            }
        }
    }
}
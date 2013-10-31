﻿using System.Collections.Generic;

namespace VisioPS.Commands
{
    public class CellValueMap
    {
        Dictionary<string, string> dic;

        private System.Text.RegularExpressions.Regex regex_cellname;
        private System.Text.RegularExpressions.Regex regex_cellname_wildcard;

        private CellMap srcmap;

        public CellValueMap( CellMap srcMap)
        {
            this.regex_cellname = new System.Text.RegularExpressions.Regex("^[a-zA-Z]*$");
            this.regex_cellname_wildcard = new System.Text.RegularExpressions.Regex("^[a-zA-Z\\*\\?]*$");
            this.dic = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);
            this.srcmap = srcMap;
        }

        public VisioAutomation.ShapeSheet.SRC GetSRC(string name)
        {
            return this.srcmap[name];
        }

        public string this[string name]
        {
            get { return this.dic[name]; }
            set
            {
                this.CheckCellName(name);
                if (!this.srcmap.HasCellName(name))
                {
                    string msg = string.Format("Unknown Cell name \"{0}\"", name);
                    throw new System.ArgumentOutOfRangeException(msg);
                }
                this.dic[name] = value;
            }
        }

        public void SetIf(string name, string value)
        {
            if (value != null)
            {
                this.dic[name] = value;
            }            
        }

        public void SetIf(int id, string name, string value)
        {
            if (value != null)
            {
                this.dic[name] = value;
            }

        }

        public Dictionary<string, string>.KeyCollection CellNames
        {
            get
            {
                return this.dic.Keys;
            }
        }

        public bool IsValidCellName(string name)
        {
            return this.regex_cellname.IsMatch(name);
        }

        public bool IsValidCellNameWildCard(string name)
        {
            return this.regex_cellname_wildcard.IsMatch(name);
        }


        public void CheckCellName(string name)
        {
            if (this.IsValidCellName(name))
            {
                return;
            }

            string msg = string.Format("Cell name \"{0}\" is not valid", name);
            throw new System.ArgumentOutOfRangeException(msg);
        }

        public void CheckCellNameWildcard(string name)
        {
            if (this.IsValidCellNameWildCard(name))
            {
                return;
            }

            string msg = string.Format("Cell name pattern \"{0}\" is not valid", name);
            throw new System.ArgumentOutOfRangeException(msg);
        }

        public IEnumerable<string> ResolveName(string cellname)
        {
            if (cellname.Contains("*") || cellname.Contains("?"))
            {
                this.CheckCellNameWildcard(cellname);
                string pat = "^" + System.Text.RegularExpressions.Regex.Escape(cellname)
                    .Replace(@"\*", ".*").
                    Replace(@"\?", ".") + "$";

                var regex = new System.Text.RegularExpressions.Regex(pat, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                foreach (string k in this.CellNames)
                {
                    if (regex.IsMatch(k))
                    {
                        yield return k;
                    }
                }
            }
            else
            {
                this.CheckCellName(cellname);
                if (!this.dic.ContainsKey(cellname))
                {
                    throw new System.ArgumentException("cellname not defined in map");
                }
                yield return cellname;
            }
        }

        public IEnumerable<string> ResolveNames(string[] cellnames)
        {
            foreach (var name in cellnames)
            {
                foreach (var resolved_name in this.ResolveName(name))
                {
                    yield return resolved_name;
                }
            }
        }


        public void UpdateValueMap(System.Collections.Hashtable Hashtable)
        {
            if (Hashtable != null)
            {
                foreach (object key_o in Hashtable.Keys)
                {
                    if (!(key_o is string))
                    {
                        string message = "Only string values can be keys in the hashtable";
                        throw new System.ArgumentOutOfRangeException(message);
                    }
                    string key_string = (string)key_o;

                    object value_o = Hashtable[key_o];
                    if (value_o == null)
                    {
                        string message = "Null values not allowed for cellvalues";
                        throw new System.ArgumentOutOfRangeException(message);
                    }
                    if (value_o is string)
                    {
                        string value_string = (string)value_o;
                        this[key_string] = value_string;
                    }
                    else if (value_o is int)
                    {
                        int value_int = (int)value_o;
                        string value_string = value_int.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        this[key_string] = value_string;
                    }
                    else if (value_o is float)
                    {
                        float value_float = (float)value_o;
                        string value_string = value_float.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        this[key_string] = value_string;
                    }
                    else if (value_o is double)
                    {
                        double value_double = (double)value_o;
                        string value_string = value_double.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        this[key_string] = value_string;
                    }
                    else
                    {
                        string message = string.Format("Cell values cannot be of type {0} ", value_o.GetType().Name);
                        throw new System.ArgumentOutOfRangeException(message);
                    }
                }
            }
        }

    }
}
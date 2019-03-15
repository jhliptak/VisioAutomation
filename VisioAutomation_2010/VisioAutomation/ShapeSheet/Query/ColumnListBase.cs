using System.Collections.Generic;

namespace VisioAutomation.ShapeSheet.Query
{
    public class ColumnListBase : IEnumerable<ColumnBase> 
    {
        protected IList<ColumnBase> _items;
        protected Dictionary<string, ColumnBase> map_name_to_item;
        protected Dictionary<ShapeSheet.Src, ColumnBase> dic_src_to_col;

        internal ColumnListBase() : this(0)
        {
        }

        internal ColumnListBase(int capacity)
        {
            this._items = new List<ColumnBase>(capacity);
            this.map_name_to_item = new Dictionary<string, ColumnBase>(capacity);
        }

        public IEnumerator<ColumnBase> GetEnumerator()
        {
            return (this._items).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public ColumnBase this[int index] => this._items[index];

        public ColumnBase this[string name] => this.map_name_to_item[name];

        public bool Contains(string name) => this.map_name_to_item.ContainsKey(name);

        protected string normalize_name(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = string.Format("Col{0}", this._items.Count);
            }
            return name;
        }

        public int Count => this._items.Count;

        protected void check_duplicate_column_name(string name)
        {
            if (this.map_name_to_item.ContainsKey(name))
            {
                throw new System.ArgumentException("Duplicate Column Name");
            }
        }

        protected void check_deplicate_src(Src src)
        {
            if (this.dic_src_to_col == null)
            {
                this.dic_src_to_col = new Dictionary<ShapeSheet.Src, ColumnBase>();
            }

            if (this.dic_src_to_col.ContainsKey(src))
            {
                string msg = string.Format("Duplicate {0}({1},{2},{3})", nameof(Src), src.Section, src.Row, src.Cell);
                throw new System.ArgumentException(msg);
            }
        }


        public ColumnBase Add(ShapeSheet.Src src, string name)
        {
            if (name == null)
            {
                throw new System.ArgumentNullException(nameof(name));
            }

            check_deplicate_src(src);
            string norm_name = this.normalize_name(name);
            check_duplicate_column_name(norm_name);

            int ordinal = this._items.Count;
            var col = new ColumnBase(ordinal, norm_name, src);
            this._items.Add(col);

            this.map_name_to_item[norm_name] = col;
            this.dic_src_to_col.Add(src, col);
            return col;
        }

    }
}
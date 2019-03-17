﻿using VASS = VisioAutomation.ShapeSheet;
using IVisio = Microsoft.Office.Interop.Visio;
using System.Collections.Generic;
using System.Linq;

namespace VisioAutomation.ShapeSheet.Query
{

    public class RowBase<T> : IEnumerable<T>
    {
        public int ShapeID { get; private set; }
        private readonly VASS.Internal.ArraySegment<T> Cells;

        internal RowBase(int shapeid, VASS.Internal.ArraySegment<T> cells)
        {
            this.ShapeID = shapeid;
            this.Cells = cells;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.Cells.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int Count
        {
            get
            {
                return this.Cells.Count;
            }
        }

        public T this[int index]
        {
            get
            {
                return this.Cells[index];
            }
        }
    }

}